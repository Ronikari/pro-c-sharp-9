using System;
using System.Linq;
using System.Reflection;
using CommonSnappableTypes;
namespace MyExtendableApp
{
    internal class Program
    {
        /// <summary>
        /// Метод выполняет следующие действия:
        /// - динамически загружает в память выбранную сборку;
        /// - определяет, содержит ли сборка типы, реализующие интерфейс IAppFunctionality;
        /// - создает экземпляр типа, используя позднее связывание.
        /// При обнаружении типа, реализующего интерфейс, вызывается метод DoIt() и передается методу
        /// DisplayCompanyData() для вывода дополнительной информации о нем посредством рефлексии
        /// </summary>
        /// <param name="assemblyName"></param>
        static void LoadExternalModule(string assemblyName)
        {
            Assembly? theSnapInAsm = null;
            try
            {
                // Динамически загрузить выбранную сборку
                theSnapInAsm = Assembly.LoadFrom(assemblyName);
            }
            catch(Exception ex)
            {
                // Ошибка при загрузке оснастки
                Console.WriteLine($"An error occurred loading the snapin: {ex.Message}");
            }

            // Получить все совместимые с IAppFunctionality классы в сборке
            List<Type>? theClassTypes = theSnapInAsm.GetTypes()
                .Where(t => t.IsClass && (t.GetInterface("IAppFunctionality") != null))
                .ToList();

            if(!theClassTypes.Any())
            {
                Console.WriteLine("Nothing implements IAppFunctionality!");
            }

            // Создать объект и вызвать метод DoIt()
            foreach(Type t in theClassTypes)
            {
                // Использовать позднее связывание для создания экземпляра типа
                IAppFunctionality itfApp = (IAppFunctionality)theSnapInAsm.CreateInstance(t.FullName, true);
                itfApp?.DoIt();

                // Отобразить информацию о компании
                DisplayCompanyData(t);
            }
        }

        /// <summary>
        /// Метод необходим для отображения метаданных, предоставляемых атрибутом [CompanyInfo]
        /// </summary>
        /// <param name="t"></param>
        static void DisplayCompanyData(Type t)
        {
            // Получить данные [CompanyInfo]
            IEnumerable<object>? compInfo = t.GetCustomAttributes(false).Where(ci => (ci is CompanyInfoAttribute));

            // Отобразить данные
            foreach(CompanyInfoAttribute c in compInfo)
            {
                Console.WriteLine($"More info about {c.CompanyName} can be found at {c.CompanyUrl}.");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("***** Welcome to MyTypeViewer *****");
            string typeName = "";
            do
            {
                Console.WriteLine("\nEnter a snapin to load");
                Console.Write("or enter Q to quit: ");

                // Получить имя типа
                typeName = Console.ReadLine();

                // Желает ли пользователь завершить работу?
                if (typeName.Equals("Q", StringComparison.OrdinalIgnoreCase)) break;

                // Попытаться отобразить тип
                try
                {
                    LoadExternalModule(typeName);
                }
                catch
                {
                    // Найти оснастку не удалось
                    Console.WriteLine("Sorry, can't find snapin");
                }
            }
            while (true);
        }
    }
}