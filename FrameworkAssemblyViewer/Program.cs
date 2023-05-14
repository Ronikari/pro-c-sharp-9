using System;
using System.Linq;
using System.Reflection;
namespace FrameworkAssemblyViewer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** The Framework Assembly Reflector App *****\n");

            // Загрузить Microsoft.EntityFrameworkCore.dll
            var displayName = "Microsoft.EntityFrameworkCore, Version=5.0.0.0, Culture=\"\", PublicKeyToken=adb9793829ddae60";
            Assembly asm = Assembly.Load(displayName);
            DisplayInfo(asm);
            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static void DisplayInfo(Assembly a)
        {
            Console.WriteLine("***** Info about assembly *****");
            Console.WriteLine($"Asm name: {a.GetName().Name}");
            Console.WriteLine($"Asm Version: {a.GetName().Version}");
            Console.WriteLine($"Asm Culture: {a.GetName().CultureInfo.DisplayName}");
            Console.WriteLine("\nHere are the public enums:");
            // Использовать запрос LINQ для нахождения открытых перечислений
            Type[] types = a.GetTypes();
            var publicEnums = from pe in types
                              where pe.IsPublic && pe.IsEnum
                              select pe;
            foreach ( var pe in publicEnums )
            {
                Console.WriteLine(pe);
            }
        }
    }
}