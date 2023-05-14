using System;
using System.Linq;
using System.Reflection;
namespace MyTypeViewer
{
    internal class Program
    {
        // Отобразить имена методов в типе
        static void ListMethods(Type t)
        {
            Console.WriteLine("***** Methods *****");
            MethodInfo[] mi = t.GetMethods();
            foreach(MethodInfo m in mi)
            {
                // Получить информацию о возвращаемом типе
                string? retVal = m.ReturnType.FullName;
                string paramInfo = "(";

                // Получить информацию о параметрах
                foreach(ParameterInfo pi in m.GetParameters())
                {
                    paramInfo += string.Format("{0} {1} ", pi.ParameterType, pi.Name);
                }
                paramInfo += " )";

                // Отобразить базовую сигнатуру метода
                Console.WriteLine("-> {0} {1} {2}", retVal, m.Name, paramInfo);
            }
            Console.WriteLine();
        }

        // Отобразить имена полей в типе
        static void ListFields(Type t)
        {
            Console.WriteLine("***** Fields *****");
            var fieldNames = from f in t.GetFields() select f.Name;
            foreach(var name in fieldNames)
            {
                Console.WriteLine("->{0}", name);
            }
            Console.WriteLine();
        }

        // Отобразить имена свойств в типе
        static void ListProps(Type t)
        {
            Console.WriteLine("***** Properties *****");
            var propNames = from p in t.GetProperties() select p.Name;
            foreach(var name in propNames)
            {
                Console.WriteLine("->{0}", name);
            }
            Console.WriteLine();
        }

        // Отобразить имена интерфейсов, которые реализуют тип
        static void ListInterfaces(Type t)
        {
            Console.WriteLine("***** Interfaces *****");
            var ifaces = from i in t.GetInterfaces() select i;
            foreach(Type i in ifaces)
            {
                Console.WriteLine("->{0}", i.Name);
            }
            Console.WriteLine();
        }

        static void ListVariousStats(Type t)
        {
            Console.WriteLine("***** Various Statistics *****");

            Console.WriteLine("Base class is: {0}", t.BaseType);
            Console.WriteLine("Is type abstract? {0}", t.IsAbstract);
            Console.WriteLine("Is type sealed? {0}", t.IsSealed);
            Console.WriteLine("Is type generic? {0}", t.IsGenericTypeDefinition);
            Console.WriteLine("Is type a class type? {0}", t.IsClass);
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("***** Welcome to MyTypeViewer *****");
            string typeName = "";

            do
            {
                Console.WriteLine("\nEnter a type name to evaluate");
                Console.Write("or enter Q to quit: ");

                // Получить имя типа
                typeName = Console.ReadLine();

                // Пользователь желает завершить программу?
                if (typeName.Equals("Q", StringComparison.OrdinalIgnoreCase)) break;

                // Попробовать отобразить информацию о типе
                try
                {
                    Type t = Type.GetType(typeName);
                    Console.WriteLine("");
                    if(t == null && typeName.Equals("System.Console", StringComparison.OrdinalIgnoreCase))
                    {
                        t = typeof(System.Console);
                    }
                    ListVariousStats(t);
                    ListFields(t);
                    ListProps(t);
                    ListMethods(t);
                    ListInterfaces(t);
                }
                catch
                {
                    Console.WriteLine("Sorry, can't find type");
                }
            }
            while (true);
        }
    }
}