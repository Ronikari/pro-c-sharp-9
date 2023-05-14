using System;
using System.IO;
namespace BinaryWriterReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with Binary Writers / Readers *****\n");

            // Открыть средство двоичной записи в файл
            FileInfo f = new FileInfo("BinFile.dat");
            using (BinaryWriter bw = new BinaryWriter(f.OpenWrite()))
            {
                // Вывести на консоль тип BaseStream (System.IO.FileStream)
                Console.WriteLine("Base stream is: {0}", bw.BaseStream);

                // Создать некоторые данные для сохранения в файле
                double aDouble = 1234.67;
                int anInt = 34567;
                string aString = "A, B, C";

                // Записать данные
                bw.Write(aDouble);
                bw.Write(anInt);
                bw.Write(aString);
                Console.WriteLine("Done!");
                Console.ReadLine();
            }

            // Читать двоичные данные из потока
            using (BinaryReader br = new BinaryReader(f.OpenRead()))
            {
                Console.WriteLine(br.ReadDouble());
                Console.WriteLine(br.ReadInt32());
                Console.WriteLine(br.ReadString());
            }
            Console.ReadLine();
        }
    }
}