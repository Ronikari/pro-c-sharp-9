using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
namespace DefaultAppDomainApp
{
    internal class Program
    {
        static void DisplayDADStats()
        {
            // Получить доступ к домену приложения для текущего потока
            AppDomain defaultAD = AppDomain.CurrentDomain;

            // Вывести разнообразные статистические данные об этом домене
            Console.WriteLine("Name of this domain: {0}", defaultAD.FriendlyName); // дружественное имя этого домена
            Console.WriteLine("ID of this domain in this process: {0}", defaultAD.Id); // идентификатор процесса
            Console.WriteLine("Is this the default domain?: {0}", defaultAD.IsDefaultAppDomain()); // является ли этот домен стандартным
            Console.WriteLine("Base directory of this domain: {0}", defaultAD.BaseDirectory); // базовый каталог этого домена
            Console.WriteLine("Setup Information for this domain:"); // информация о настройке этого домена: 
            Console.WriteLine("\tApplication Base: {0}", defaultAD.SetupInformation.ApplicationBase); // базовый каталог приложения
            Console.WriteLine("\tTarget Framework: {0}", defaultAD.SetupInformation.TargetFrameworkName); // целевая платформа
        }

        static void ListAllAssembliesInAppDomain()
        {
            // Получить доступ к домену приложения для текущего потока
            AppDomain defaultAD = AppDomain.CurrentDomain;

            // Извлечь все сборки, загруженные в стандартный домен приложения
            var loadedAssemblies = defaultAD.GetAssemblies().OrderBy(x => x.GetName().Name);
            Console.WriteLine("\n***** Here are the assemblies loaded in {0} *****", defaultAD.FriendlyName);
            foreach(Assembly a in loadedAssemblies)
            {
                // Вывести имя и версию
                Console.WriteLine($"-> Name, Version: {a.GetName().Name}:{a.GetName().Version}");
            }
        }

        static void LoadAdditionalAssembliesDifferentContexts()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassLibrary1.dll");
            AssemblyLoadContext lc1 = new AssemblyLoadContext("NewContext1", false);
            var cl1 = lc1.LoadFromAssemblyPath(path);
            var c1 = cl1.CreateInstance("ClassLibrary1.Car");

            AssemblyLoadContext lc2 = new AssemblyLoadContext("NewContext2", false);
            var cl2 = lc2.LoadFromAssemblyPath(path);
            var c2 = cl2.CreateInstance("ClassLibrary1.Car");

            Console.WriteLine("*** Loading Additional Assemblies in Different Contexts ***");
            Console.WriteLine($"Assembly1.Equals(Assembly2) {cl1.Equals(cl2)}");
            Console.WriteLine($"Assembly1 == Assembly2 {cl1 == cl2}");
            Console.WriteLine($"Class1.Equals(Class2) {c1.Equals(c2)}");
            Console.WriteLine($"Class1 == Class2 {c1 == c2}");
        }

        static void LoadAdditionalAssembliesSameContext()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassLibrary1.dll");
            AssemblyLoadContext lc1 = new AssemblyLoadContext(null, false);
            var cl1 = lc1.LoadFromAssemblyPath(path);
            var c1 = cl1.CreateInstance("ClassLibrary1.Car");
            var cl2 = lc1.LoadFromAssemblyPath(path);
            var c2 = cl2.CreateInstance("ClassLibrary1.Car");

            Console.WriteLine("*** Loading Additional Assemblies in Same Context ***");
            Console.WriteLine($"Assembly1.Equals(Assembly2) {cl1.Equals(cl2)}");
            Console.WriteLine($"Assembly1 == Assembly2 {cl1 == cl2}");
            Console.WriteLine($"Class1.Equals(Class2) {c1.Equals(c2)}");
            Console.WriteLine($"Class1 == Class2 {c1 == c2}");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with the default AppDomain *****\n");
            DisplayDADStats();
            ListAllAssembliesInAppDomain();
            LoadAdditionalAssembliesDifferentContexts();
            LoadAdditionalAssembliesSameContext();
            Console.ReadLine();
        }
    }
}