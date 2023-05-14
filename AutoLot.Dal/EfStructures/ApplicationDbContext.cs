using System;
using System.Collections;
using System.Collections.Generic;
using AutoLot.Models.Entities;
using AutoLot.Models.Entities.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoLot.Dal.Exceptions;
using AutoLot.Models.ViewModels;

namespace AutoLot.Dal.EfStructures
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.Tracked += ChangeTracker_Tracked;
            ChangeTracker.StateChanged += ChangeTracker_StateChanged;

            base.SavingChanges += (sender, args) =>
            {
                Console.WriteLine($"Saving changes for {((ApplicationDbContext)sender)!.Database!.GetConnectionString()}");
            };
            base.SavedChanges += (sender, args) =>
            {
                Console.WriteLine($"Saved {args!.EntitiesSavedCount} changes for" +
                    $"{((ApplicationDbContext)sender)!.Database!.GetConnectionString()}");
            };
            base.SaveChangesFailed += (sender, args) =>
            {
                Console.WriteLine($"An exception occurred! {args.Exception.Message} entities");
            };
        }

        private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
        {
            var source = (e.FromQuery) ? "Database" : "Code";
            if (e.Entry.Entity is Car c)
            {
                Console.WriteLine($"Car entry {c.PetName} was added from {source}");
            }
        }

        private void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            if (e.Entry.Entity is not Car c)
            {
                return;
            }
            var action = string.Empty;
            Console.WriteLine($"Car {c.PetName} was {e.OldState} before the state changed to {e.NewState}");
            switch (e.NewState)
            {
                case EntityState.Unchanged:
                    action = e.OldState switch
                    {
                        EntityState.Added => "Added",
                        EntityState.Modified => "Edited",
                        _ => action
                    };
                    Console.WriteLine($"The object was {action}");
                    break;
            }
        }

        public DbSet<SeriLogEntry>? LogEntries { get; set; }
        public DbSet<CreditRisk>? CreditRisks { get; set; }
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<Make>? Makes { get; set; }
        public DbSet<Car>? Cars { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<CustomerOrderViewModel>? CustomerOrderViewModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeriLogEntry>(entity =>
            {
                entity.Property(e => e.Properties).HasColumnType("Xml");
                entity.Property(e => e.TimeStamp).HasDefaultValueSql("GetDate()");
            });

            modelBuilder.Entity<CreditRisk>(entity =>
            {
                entity.HasOne(d => d.CustomerNavigation)
                .WithMany(p => p!.CreditRisks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_CreditRisks_Customers");

                entity.OwnsOne(o => o.PersonalInformation,
                    pd =>
                    {
                        pd.Property<string>(nameof(Person.FirstName)).HasColumnName(nameof(Person.FirstName))
                        .HasColumnType("nvarchar(50)");
                        pd.Property<string>(nameof(Person.LastName)).HasColumnName(nameof(Person.LastName))
                        .HasColumnType("nvarchar(50)");
                        pd.Property(p => p.FullName).HasColumnName(nameof(Person.FullName))
                        .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
                    });
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.OwnsOne(o => o.PersonalInformation,
                    pd =>
                    {
                        pd.Property<string>(nameof(Person.FirstName)).HasColumnName(nameof(Person.FirstName));
                        pd.Property<string>(nameof(Person.LastName)).HasColumnName(nameof(Person.LastName));
                        pd.Property(p => p.FullName).HasColumnName(nameof(Person.FullName))
                        .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
                    });
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasQueryFilter(c => c.IsDrivable);
                entity.Property(p => p.IsDrivable).HasField("_isDrivable").HasDefaultValue(true);
                entity.HasOne(d => d.MakeNavigation)
                    .WithMany(p => p!.Cars)
                    .HasForeignKey(d => d.MakeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Make_Inventory");
            });

            modelBuilder.Entity<CustomerOrderViewModel>().HasNoKey().ToView("CustomerOrderView", "dbo");

            modelBuilder.Entity<Make>(entity =>
            {
                entity.HasMany(e => e.Cars)
                .WithOne(c => c.MakeNavigation!)
                .HasForeignKey(k => k.MakeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Make_Inventory");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(d => d.CarNavigation)
                .WithMany(p => p!.Orders)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Inventory");

                entity.HasOne(d => d.CustomerNavigation)
                .WithMany(p => p!.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_Customers");
            });

            modelBuilder.Entity<Order>().HasQueryFilter(e => e.CarNavigation!.IsDrivable);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Произошла ошибка параллелизма.
                // Подлежит регистрации в журнале и надлежащей обработке.
                throw new CustomConcurrencyException("A concurrency error happened.", ex); // произошла ошибка параллелизма
            }
            catch (RetryLimitExceededException ex)
            {
                // Превышен предел на количество повторных попыток DbResiliency.
                // Подлежит регистрации в журнале и надлежащей обработке
                throw new CustomRetryLimitExceededException("There is a problem with SQL Server.", ex);
            }
            catch (DbUpdateException ex)
            {
                // Подлежит регистрации в журнале и надлежащей обработке
                throw new CustomDbUpdateException("An error occurred updating the database.", ex);
            }
            catch (Exception ex)
            {
                // Подлежит регистрации в журнале и надлежащей обработке
                throw new CustomException("An error occurred updating the database.", ex);
            }
        }
    }
}
