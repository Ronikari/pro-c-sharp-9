using System;
using System.IO;
namespace DirectoryApp
{
    internal class Program
    {
        static void ShowWindowsDirectoryInfo(DirectoryInfo dir)
        {
            Console.WriteLine("***** Directory Info *****");
            Console.WriteLine("FullName: {0}", dir.FullName);
            Console.WriteLine("Name: {0}", dir.Name);
            Console.WriteLine("Parent: {0}", dir.Parent);
            Console.WriteLine("Creation: {0}", dir.CreationTime);
            Console.WriteLine("Attributes: {0}", dir.Attributes);
            Console.WriteLine("Root: {0}", dir.Root);
            Console.WriteLine("********************************************************\n");
        }

        static void DisplayImageFiles(DirectoryInfo dir)
        {
            // Получить все файлы с расширением *.jpg
            FileInfo[] imageFiles = dir.GetFiles("*.jpg", SearchOption.AllDirectories);

            // Сколько файлов найдено?
            Console.WriteLine("Found {0} *.jpg files\n", imageFiles.Length);

            // Вывести информацию о каждом файле
            foreach(FileInfo f in imageFiles)
            {
                Console.WriteLine("********************************************************");
                Console.WriteLine("File name: {0}", f.Name);
                Console.WriteLine("File size: {0} bytes",f.Length);
                Console.WriteLine("Creation: {0}", f.CreationTime);
                Console.WriteLine("Attributes: {0}", f.Attributes);
                Console.WriteLine("********************************************************\n");
            }
        }

        static void ModifyAppDirectory()
        {
            DirectoryInfo dir = new DirectoryInfo(".");

            // Создать \MyFolder в каталоге запуска приложения
            dir.CreateSubdirectory("MyFolder");

            // Создать \MyFolder2\Data в каталоге запуска приложения
            DirectoryInfo myDataFolder = dir.CreateSubdirectory($@"MyFolder2{Path.DirectorySeparatorChar}Data");

            // Вывести конечный путь
            Console.WriteLine("New Folder is: {0}", myDataFolder);
        }

        static void FunWithDirectoryType()
        {
            // Вывести список всех логических устройств на текущем компьютере
            string[] drives = Directory.GetLogicalDrives();
            Console.WriteLine("Here are your drives:");
            foreach (string s in drives)
            {
                Console.WriteLine("--> {0}", s);
            }

            // Удалить ранее созданные подкаталоги
            Console.WriteLine("Press Enter to delete directories");
            Console.ReadLine();
            try
            {
                Directory.Delete("MyFolder");
                // Второй параметр указывает, нужно ли удалять внутренние подкаталоги
                Directory.Delete("MyFolder2", true);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with Directory (Info) *****\n");
            DirectoryInfo dir = new DirectoryInfo($@"D:\Бэкап\Фотки");
            //ShowWindowsDirectoryInfo(dir);
            //DisplayImageFiles(dir);
            //ModifyAppDirectory();
            FunWithDirectoryType();
            Console.ReadLine();
        }
    }
}