using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RenderingWithVisuals
{
    public class CustomVisualFrameworkElement : FrameworkElement
    {
        // Коллекция всех визуальных объектов
        VisualCollection theVisuals;

        public CustomVisualFrameworkElement()
        {
            // Заполнить коллекцию VisualCollection несколькими объектами DrawingVisual.
            // Аргумент конструктора представляет владельца визуальных объектов
            theVisuals = new VisualCollection(this) { AddRect(), AddCircle() };

            this.MouseDown += CustomVisualFrameworkElement_MouseDown;
        }

        private Visual AddCircle()
        {
            DrawingVisual drawingVisual = new DrawingVisual();

            // Получить объект DrawingContext для создания нового содержимого
            using DrawingContext drawingContext = drawingVisual.RenderOpen();

            // Создать круг и нарисовать его в DrawingContext
            drawingContext.DrawEllipse(Brushes.DarkBlue, null, new Point(70, 90), 40, 50);
            return drawingVisual;
        }

        private Visual AddRect()
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            using DrawingContext drawingContext = drawingVisual.RenderOpen();
            Rect rect = new Rect(new Point(160, 100), new Size(320, 80));
            drawingContext.DrawRectangle(Brushes.Tomato, null, rect);
            return drawingVisual;
        }

        protected override int VisualChildrenCount => theVisuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            // Значение должно быть больше нуля, поэтому разумно это проверить
            if (index < 0 || index >= theVisuals.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            return theVisuals[index];
        }

        void CustomVisualFrameworkElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Выяснить, где пользователь выполнить щелчок
            Point pt = e.GetPosition((UIElement)sender);

            // Вызвать вспомогательную функцию через делегат, чтобы посмотреть, был ли совершен щелчок на визуальном объекте
            VisualTreeHelper.HitTest(this, null, new HitTestResultCallback(myCallback), new PointHitTestParameters(pt));
        }

        public HitTestResultBehavior myCallback(HitTestResult result)
        {
            // Если щелчок был совершен на визуальном объекте, то переключиться между скошенной и нормальной визуализацией
            if (result.VisualHit.GetType() == typeof(DrawingVisual))
            {
                if (((DrawingVisual)result.VisualHit).Transform == null)
                {
                    ((DrawingVisual)result.VisualHit).Transform = new SkewTransform(7, 7);
                }
                else
                {
                    ((DrawingVisual)result.VisualHit).Transform = null;
                }
            }

            // Сообщить методу HitTest() о прекращении углубления в визуальное дерево
            return HitTestResultBehavior.Stop;
        }
    }
}
