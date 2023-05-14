using System;
using System.Linq;
namespace FunWithProbingPaths
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with Probing Paths *****");
            Console.WriteLine("TRUSTED_PLATFORM_ASSEMBLIES:");
            // Для платформ, отличающихся от Windows, необходимо использовать ':'
            var list = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES").ToString().Split(';');
            foreach(var dir in list)
            {
                Console.WriteLine(dir);
            }
            Console.WriteLine();
            Console.WriteLine($"PLATFORM_RESOURCE_ROOTS: {AppContext.GetData("PLATFORM_RESOURCE_ROOTS")}");
            Console.WriteLine();
            Console.WriteLine($"NATIVE_DLL_SEARCH_DIRECTORIES: {AppContext.GetData("NATIVE_DLL_SEARCH_DIRECTORIES")}");
            Console.WriteLine();
            Console.WriteLine($"APP_PATHS: {AppContext.GetData("APP_PATHS")}");
            Console.WriteLine();
            Console.WriteLine($"APP_NI_PATHS: {AppContext.GetData("APP_NI_PATHS")}");
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}