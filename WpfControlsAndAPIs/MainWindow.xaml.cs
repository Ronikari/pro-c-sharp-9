using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoLot.Dal.EfStructures;
using AutoLot.Dal.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WpfControlsAndAPIs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IConfiguration _configuration;
        private ApplicationDbContext _context;

        public MainWindow()
        {
            this.InitializeComponent();

            // Установить режим Ink в качестве стандартного
            this.MyInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            this.inkRadio.IsChecked = true;
            this.comboColors.SelectedIndex = 0;

            SetBindings();
            GetConfigurationAndDbContext();
            ConfigureGrid();
        }

        private void SetBindings()
        {
            // Создать объект Binding
            Binding b = new Binding
            {
                // Зарегистрировать преобразователь, источник и путь
                Converter = new MyDoubleConverter(),
                Source = this.mySB,
                Path = new PropertyPath("Value")
            };

            // Вызвать метод SetBinding() объекта Label
            this.labelSBThumb.SetBinding(Label.ContentProperty, b);
        }

        private void RadioButtonClicked(object sender, RoutedEventArgs e)
        {
            // В зависимости от того, какая кнопка отправила событие, поместить InkCanvas
            // в нужный режим оперирования
            this.MyInkCanvas.EditingMode = (sender as RadioButton)?.Content.ToString() switch
            {
                // Эти строки должны совпадать со значениями свойства Content каждого элемента RadioButton
                "Ink Mode!" => InkCanvasEditingMode.Ink,
                "Erase Mode!" => InkCanvasEditingMode.EraseByStroke,
                "Select Mode!" => InkCanvasEditingMode.Select,
                _ => this.MyInkCanvas.EditingMode
            };
        }

        private void ColorChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получить свойство Tag выбранного элемента StackPanel
            string? colorToUse = (this.comboColors.SelectedItem as StackPanel)?.Tag.ToString();

            // Изменить цвет, используемый для визуализации штрихов
            this.MyInkCanvas.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(colorToUse);
        }

        private void LoadData(object sender, RoutedEventArgs e)
        {
            // Наполнить StrokeCollection из файла
            using (FileStream fs = new FileStream("StrokeData.bin", FileMode.Open, FileAccess.Read))
            {
                StrokeCollection strokes = new StrokeCollection(fs);
                this.MyInkCanvas.Strokes = strokes;
            }
        }

        private void SaveData(object sender, RoutedEventArgs e)
        {
            // Сохранить все данные InkCanvas в локальном файле
            using (FileStream fs = new FileStream("StrokeData.bin", FileMode.Create))
            {
                this.MyInkCanvas.Strokes.Save(fs);
                fs.Close();
                MessageBox.Show("Image Saved", "Saved");
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            // Очистить все штрихи
            this.MyInkCanvas.Strokes.Clear();
        }

        private void GetConfigurationAndDbContext()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = _configuration.GetConnectionString("AutoLot");
            optionsBuilder.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
            _context = new ApplicationDbContext(optionsBuilder.Options);
        }

        private void ConfigureGrid()
        {
            using var repo = new CarRepo(_context);
            gridInventory.ItemsSource = repo
                .GetAllIgnoreQueryFilters()
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    Make = x.MakeName,
                    x.Color,
                    x.PetName
                });
        }
    }
}
