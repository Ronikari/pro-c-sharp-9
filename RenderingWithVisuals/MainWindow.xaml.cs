using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace RenderingWithVisuals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            const int TextFontSize = 30;

            // Создать объект System.Windows.Media.FormattedText
            FormattedText text = new FormattedText("Hello Visual Layer!", new System.Globalization.CultureInfo("en-US"),
                FlowDirection.LeftToRight, new Typeface(this.FontFamily, FontStyles.Italic, FontWeights.DemiBold,
                FontStretches.UltraExpanded), TextFontSize, Brushes.Green, null, VisualTreeHelper.GetDpi(this).PixelsPerDip);

            // Создать объект DrawingVisual и получить объект DrawingContext
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                // Вызвать любой из методов DrawingContext для визуализации данных
                drawingContext.DrawRoundedRectangle(Brushes.Yellow,
                    new Pen(Brushes.Black, 5),
                    new Rect(5, 5, 450, 100), 20, 20);
                drawingContext.DrawText(text, new Point(20, 20));
            }

            // Динамически создать битовое изображение, используя данные в объекте DrawingVisual
            RenderTargetBitmap bmp = new RenderTargetBitmap(500, 100, 100, 90, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            // Установить источник для элемента управления Image
            myImage.Source = bmp;
        }
    }
}
