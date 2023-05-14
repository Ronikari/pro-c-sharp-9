using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConnectionFactory
{
    // Пакет OleDb предназначен только для Windows и в .NET Core не поддерживается
    enum DataProviderEnum
    {
        SqlServer,
#if PC
        OleDb,
#endif
        Odbc,
        None
    }
}
