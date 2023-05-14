using System;
using System.IO; // для определения FileNotFoundException
using System.Reflection;
namespace ExternalAssemblyReflector
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** External Assembly Viewer *****");
            string? asmName = "";
            Assembly? asm = null;
            do
            {
                Console.WriteLine("\nEnter an assembly to evaluate");
                Console.Write("or enter Q to quit: ");

                // Получить имя сборки
                asmName = Console.ReadLine();

                // Пользователь решает завершить программу?
                if (asmName.Equals("Q", StringComparison.OrdinalIgnoreCase)) break;

                // Попробовать загрузить сборку
                try
                {
                    asm = Assembly.LoadFrom(asmName);
                    DisplayTypesInAsm(asm);
                }
                catch
                {
                    Console.WriteLine("Sorry, can't find assembly");
                }
            }
            while (true);
        }

        static void DisplayTypesInAsm(Assembly? asm)
        {
            Console.WriteLine("\n***** Types in Assembly *****");
            Console.WriteLine("-> {0}", asm.FullName);
            Type[] types = asm.GetTypes();
            foreach(Type t in types)
            {
                Console.WriteLine("Type: {0}", t);
            }
            Console.WriteLine();
        }
    }
}