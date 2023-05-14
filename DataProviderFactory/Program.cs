using System;
using System.Data.Common;
using System.Data.Odbc;
#if PC
using System.Data.OleDb;
#endif
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataProviderFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("****** Fun with Data Provider Factories *****\n");
            var (provider, connectionString) = GetProviderFromConfiguration();
            DbProviderFactory factory = GetDbProviderFactory(provider);

            // Теперь получить объект подключения
            using (DbConnection connection = factory.CreateConnection())
            {
                if(connection == null)
                {
                    Console.WriteLine($"Unable to create the connection object");
                    return;
                }

                Console.WriteLine($"Your connection object is a: {connection.GetType().Name}");
                connection.ConnectionString = connectionString;
                connection.Open();

                // Создать объект команды
                DbCommand command = connection.CreateCommand();
                if(command == null)
                {
                    Console.WriteLine($"Unable to create the command object");
                    return;
                }

                Console.WriteLine($"Your command object is a: {command.GetType().Name}");
                command.Connection = connection;
                command.CommandText = "Select i.Id, m.Name From Inventory i inner join Makes m on m.Id = i.MakeId";

                // Вывести данные с помощью объекта чтения данных
                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    Console.WriteLine($"Your data reader object is a: {dataReader.GetType().Name}");

                    Console.WriteLine("\n***** Current Inventory *****");
                    while (dataReader.Read())
                    {
                        Console.WriteLine($"-> Car #{dataReader["Id"]} is a {dataReader["Name"]}.");
                    }
                }
            }
            Console.ReadLine();
        }

        static DbProviderFactory? GetDbProviderFactory(DataProviderEnum provider) => provider switch
        {
            DataProviderEnum.SqlServer => SqlClientFactory.Instance,
            DataProviderEnum.Odbc => OdbcFactory.Instance,
#if PC
            DataProviderEnum.OleDb => OleDbFactory.Instance,
#endif
            _ => null
        };

        static (DataProviderEnum Provider, string ConnectionString) GetProviderFromConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var providerName = config["ProviderName"];
            if(Enum.TryParse<DataProviderEnum>(providerName, out DataProviderEnum provider))
            {
                return (provider, config[$"{providerName}:ConnectionString"]);
            };
            throw new Exception("Invalid data provider value supplied.");
        }
    }
}