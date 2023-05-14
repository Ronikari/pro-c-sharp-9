using System;
using System.Collections.Generic;
using System.Reflection;

// Создать псевдоним для объектной модели Excel
using Excel = Microsoft.Office.Interop.Excel;
using ExportDataToOfficeApp;
namespace ExportDataToOfficeApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Car> carsInStock = new List<Car>
            {
                new Car {Color = "Green", Make = "VW", PetName = "Mary"},
                new Car {Color = "Red", Make = "Saab", PetName = "Mel"},
                new Car {Color = "Black", Make = "Ford", PetName = "Hank"},
                new Car {Color = "Yellow", Make = "BMW", PetName = "Davie"}
            };
            ExportToExcel(carsInStock);

            void ExportToExcel(List<Car> carsInStock)
            {
                // Загрузить Excel и затем создать новую пустую рабочую книгу
                Excel.Application excelApp = new Excel.Application();
                excelApp.Workbooks.Add();

                // Сделать пользовательский интерфейс Excel видимым на рабочем столе
                excelApp.Visible = true;

                // В этом примере используется единственный рабочий лист
                Excel._Worksheet workSheet = (Excel._Worksheet)excelApp.ActiveSheet;

                // Установить заголовки столбцов в ячейках
                workSheet.Cells[1, "A"] = "Make";
                workSheet.Cells[1, "B"] = "Color";
                workSheet.Cells[1, "C"] = "Pet Name";

                // Сопоставить все данные из List<Car> с ячейками электронной таблицы
                int row = 1;
                foreach(Car c in carsInStock)
                {
                    row++;
                    workSheet.Cells[row, "A"] = c.Make;
                    workSheet.Cells[row, "B"] = c.Color;
                    workSheet.Cells[row, "C"] = c.PetName;
                }

                // Придать симпатичный вид табличным данным
                workSheet.Range["A1"].AutoFormat(Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic2);

                // Сохранить файл, завершить работу Excel и отобразить сообщение пользователю
                workSheet.SaveAs($@"{Environment.CurrentDirectory}\Inventory.xlsx");
                excelApp.Quit();
                Console.WriteLine("The Inventory.xslx file has been saved to your app folder");
            }
        }
    }
}