// Обеспечить доступ к перечисленным ниже пространствам имен!
// (System.Threating.Tasks уже должно присутствовать благодаря выбранному шаблону)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataParallelismWithForEach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Новая переменная уровня Window
        private CancellationTokenSource _cancelToken = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            // Используется для сообщения всем рабочим потокам о необходимости останова!
            _cancelToken.Cancel();
        }

        private void cmdProcess_Click(object sender, EventArgs e)
        {
            // Запустить новую "задачу" для обработки файлов
            Task.Factory.StartNew(ProcessFiles);
            this.Title = "Processing Complete";
        }

        private void ProcessFiles()
        {
            // Использовать экземпляр ParallelOptions для хранения CancellationToken
            ParallelOptions parOpts = new ParallelOptions();
            parOpts.CancellationToken = _cancelToken.Token;
            parOpts.MaxDegreeOfParallelism = System.Environment.ProcessorCount;

            // Загрузить все файлы .jpg и создать новый каталог для модифицированных данных.
            // Получить путь к каталогу с исполняемым файлом.
            // В режиме отладки VS 2019 текущим каталогом будет <каталог проекта>\bin\debug\net7.0-windows.
            // В случае VS Code или команды dotnet run текущим каталогом будет <каталог проекта>.
            var basePath = Directory.GetCurrentDirectory();
            var pictureDirectory = System.IO.Path.Combine(basePath, "TestPictures");
            var outputDirectory = System.IO.Path.Combine(basePath, "ModifiedPictures");

            // Удалить любые существующие файлы
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);

            try
            {
                // Обработать данные изображений в блокирующей манере
                Parallel.ForEach(files, currentFile =>
                {
                    string filename = System.IO.Path.GetFileName(currentFile);
                    // Вывести идентификатор потока, обрабатывающего текущее изображение
                    //this.Title = $"Processing {filename} on thread {Thread.CurrentThread.ManagedThreadId}...";
                    // Вызвать Invoke() на объекте Dispatcher, чтобы позволить вторичным потокам получать доступ
                    // к элементам управления в безопасной к потокам манере
                    Dispatcher?.Invoke(() =>
                    {
                        this.Title = $"Processing {filename} on thread {Thread.CurrentThread.ManagedThreadId}...";
                    });
                    using (Bitmap bitmap = new Bitmap(currentFile))
                    {
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));
                    }
                });
                Dispatcher?.Invoke(() => this.Title = "Done!");
            }
            catch(OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => this.Title = ex.Message);
            }

        }
    }
}
