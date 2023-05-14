using System;
using System.IO;
using System.Text;
namespace FileStreamApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with FileStreams *****\n");

            // Получить объект FileStream
            using (FileStream fStream = File.Open("myMessage.dat", FileMode.Create))
            {
                // Закодировать строку в виде массива байтов
                string msg = "Hello!";
                byte[] msgAsByteArray = Encoding.Default.GetBytes(msg);
                // Записать byte[] в файл
                fStream.Write(msgAsByteArray, 0, msgAsByteArray.Length);
                // Сбросить внутреннюю позицию потока
                fStream.Position = 0;
                // Прочитать byte[] из файла и вывести на консоль
                Console.Write("Your message as an array of bytes: ");
                byte[] bytesOnFile = new byte[msgAsByteArray.Length];
                for (int i = 0; i < bytesOnFile.Length; i++)
                {
                    bytesOnFile[i] = (byte)fStream.ReadByte();
                    Console.Write(bytesOnFile[i]);
                }
                // Вывести декодированное сообщение
                Console.Write("\nDecoded Message: ");
                Console.WriteLine(Encoding.Default.GetString(bytesOnFile));
                Console.ReadLine();
            }
            File.Delete("myMessage.dat");
        }
    }
}