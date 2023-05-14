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

namespace PictureHandlerWithAsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource? _cancelToken = null;

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            _cancelToken?.Cancel();
        }

        private async void cmdProcess_Click(object sender, EventArgs e)
        {
            _cancelToken = new CancellationTokenSource();
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
                foreach(string file in files)
                {
                    try
                    {
                        await ProcessFile(file, outputDirectory, _cancelToken.Token);
                    }
                    catch(OperationCanceledException ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            }
            catch(OperationCanceledException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            _cancelToken = null;
            this.Title = "Processing complete";
        }

        private async Task ProcessFile(string currentFile, string outputDirectory, CancellationToken token)
        {
            string filename = System.IO.Path.GetFileName(currentFile);
            using(Bitmap bitmap = new Bitmap(currentFile))
            {
                try
                {
                    await Task.Run(() =>
                    {
                        Dispatcher?.Invoke(() =>
                        {
                            this.Title = $"Processing {filename}";
                        });
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        bitmap.Save(System.IO.Path.Combine(outputDirectory, filename));
                    }, token);
                }
                catch(OperationCanceledException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
