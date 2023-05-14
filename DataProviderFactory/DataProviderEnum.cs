namespace DataProviderFactory
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
