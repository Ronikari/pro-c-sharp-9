using System;
using System.IO;
namespace MyDirectoryWatcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** The Amazing File Watcher App *****\n");

            // Установить путь к каталогу, за которым нужно наблюдать
            FileSystemWatcher watcher = new FileSystemWatcher();
            try
            {
                watcher.Path = @".";
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Указать цели наблюдения
            watcher.NotifyFilter = NotifyFilters.LastAccess
                | NotifyFilters.LastWrite
                | NotifyFilters.FileName
                | NotifyFilters.DirectoryName;

            // Следить только за текстовыми файлами
            watcher.Filter = "*.txt";

            // Добавить обработчики событий.
            // Указать, что будет происходить при изменении, создании и удалении файла
            watcher.Created += (s, e) => Console.WriteLine($"File: {e.FullPath} {e.ChangeType}!");
            watcher.Changed += (s, e) => Console.WriteLine($"File: {e.FullPath} {e.ChangeType}!");
            watcher.Deleted += (s, e) => Console.WriteLine($"File: {e.FullPath} {e.ChangeType}!");

            // Указать, что будет происходить при переименовании файла
            watcher.Renamed += (s, e) => Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}!");

            // Начать наблюдение за каталогом
            watcher.EnableRaisingEvents = true;

            // Ожидать от пользователя команды завершения программы
            Console.WriteLine(@"Press 'q' to quit app.");

            // Сгенерировать несколько событий
            using (StreamWriter sw = File.CreateText("Test.txt"))
            {
                sw.Write("This is some text");
            }
            File.Move("Test.txt", "Test2.txt");
            File.Delete("Test2.txt");
            while (Console.Read() != 'q') ;
        }
    }
}