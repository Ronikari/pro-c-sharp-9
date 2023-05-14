using System;
using System.Data;
using System.Data.Odbc;
#if PC
using System.Data.OleDb;
#endif
using Microsoft.Data.SqlClient;
namespace MyConnectionFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Very Simple Connection Factory *****\n");
            Setup(DataProviderEnum.SqlServer);
#if PC
            Setup(DataProviderEnum.OleDb);
#endif
            Setup(DataProviderEnum.Odbc);
            Setup(DataProviderEnum.None);

            Console.ReadLine();

            void Setup(DataProviderEnum provider)
            {
                // Получить конкретное подключение
                IDbConnection myConnection = GetConnection(provider);
                Console.WriteLine($"Your connection is a {myConnection?.GetType().Name ?? "unrecognized type"}");
            }

            IDbConnection? GetConnection(DataProviderEnum dataProvider) => dataProvider switch
            {
                DataProviderEnum.SqlServer => new SqlConnection(),
#if PC
                // Не поддерживается в macOS
                DataProviderEnum.OleDb => new OleDbConnection(),
#endif
                DataProviderEnum.Odbc => new OdbcConnection(),
                _ => null,
            };
        }
    }
}