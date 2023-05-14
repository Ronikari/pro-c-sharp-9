﻿using AutoLot.Dal.EfStructures;
using AutoLot.Models.Entities;
using AutoLot.Dal.Repos.Base;
using AutoLot.Dal.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoLot.Models.ViewModels;

namespace AutoLot.Dal.Repos
{
    public class OrderRepo : BaseRepo<Order>, IOrderRepo
    {
        public OrderRepo(ApplicationDbContext context) : base(context) { }
        internal OrderRepo(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public IQueryable<CustomerOrderViewModel> GetOrdersViewModel()
        {
            return Context.CustomerOrderViewModels!.AsQueryable();
        }
    }
}
