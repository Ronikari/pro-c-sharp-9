using System;
using System.IO;
namespace SimpleFileIO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Simple IO with the File Type *****\n");
            DirectoryInfo dir = new DirectoryInfo(".");
            var fileName = $@"{dir}\Test.dat";

            // Создать новый файл на диске
            FileInfo f = new FileInfo(fileName);
            FileStream fs = f.Create();

            // Использовать объект FileStream...
            // Закрыть файловый поток
            fs.Close();

            // Поместить файловый поток внутрь оператора using
            FileInfo f1 = new FileInfo(fileName);
            using(FileStream fs1 = f1.Create())
            {
                // Использовать объект FileStream...
            }
            f1.Delete();

            // Создать новый файл посредством FileInfo.Open()
            FileInfo f2 = new FileInfo(fileName);
            using (FileStream fs2 = f2.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                // Использовать объект FileStream...
            }
            f2.Delete();

            // Получить объект FileStream с правами только для чтения
            FileInfo f3 = new FileInfo(fileName);
            // Перед использованием OpenRead() файл должен существовать
            f3.Create().Close();
            using(FileStream readOnlyStream = f3.OpenRead())
            {
                // Использовать объект FileStream...
            }
            f3.Delete();

            // Теперь получить объект FileStream с правами только для записи
            FileInfo f4 = new FileInfo(fileName);
            using(FileStream writeOnlyStream = f4.OpenWrite())
            {
                // Использовать объект FileStream...
            }
            f4.Delete();

            // Получить объект StreamReader
            FileInfo f5 = new FileInfo(fileName);
            // Перед использованием OpenText() файл должен существовать
            f5.Create().Close();
            using(StreamReader sReader = f5.OpenText())
            {
                // Использовать объект StreamReader()...
            }
            f5.Delete();

            FileInfo f6 = new FileInfo(fileName);
            using(StreamWriter sWriter = f6.CreateText())
            {
                // Использовать объект StreamWriter()...
            }
            f6.Delete();

            FileInfo f7 = new FileInfo(fileName);
            using(StreamWriter sWriterAppend = f7.AppendText())
            {
                // Использовать объект StreamWriter()...
            }
            f7.Delete();

            // Использование File вместо FileInfo
            using(FileStream f8 = File.Create(fileName))
            {
                // Использовать объект FileStream...
            }
            File.Delete(fileName);

            // Создать новый файл через File.Open()
            using(FileStream f9 = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                // Использовать объект FileStream...
            }

            // Получить объект FileStream с правами только для чтения
            using(FileStream readOnlyStream = File.OpenRead(fileName)) { }
            File.Delete(fileName);

            // Теперь получить объект FileStream с правами только для записи
            using(FileStream writeOnlyStream = File.OpenWrite(fileName)) { }
            File.Delete(fileName);

            // Получить объект StreamReader
            using(StreamReader sReader = File.OpenText(fileName)) { }
            File.Delete(fileName);

            // Получить несколько объектов StreamWriter
            using(StreamWriter sWriter = File.CreateText(fileName)) { }
            File.Delete(fileName);
            using(StreamWriter sWriterAppend = File.AppendText(fileName)) { }
            File.Delete(fileName);
        }
    }
}