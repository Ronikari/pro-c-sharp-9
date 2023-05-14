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

namespace CustomDependencyProperty
{
    /// <summary>
    /// Логика взаимодействия для ShowNumberControl.xaml
    /// </summary>
    public partial class ShowNumberControl : UserControl
    {
        public ShowNumberControl()
        {
            InitializeComponent();
        }

        // Обычное свойство .NET Core
        private int _currNumber = 0;

        public int CurrentNumber
        {
            get => (int)GetValue(CurrentNumberProperty);
            set => SetValue(CurrentNumberProperty, value);
        }

        public static readonly DependencyProperty CurrentNumberProperty =
            DependencyProperty.Register("CurrentNumber",
                typeof(int),
                typeof(ShowNumberControl),
                new UIPropertyMetadata(100, new PropertyChangedCallback(CurrentNumberChanged)),
                new ValidateValueCallback(ValidateCurrentNumber));

        // Простое бизнес-правило: значение должно находиться в диапазоне между 0 и 500
        public static bool ValidateCurrentNumber(object value) =>
            Convert.ToInt32(value) >= 0 && Convert.ToInt32(value) <= 500;

        private static void CurrentNumberChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            // Привести DependencyObject к ShowNumberControl
            ShowNumberControl c = (ShowNumberControl)depObj;

            // Получить элемент управления Label в ShowNumberControl
            Label theLabel = c.numberDisplay;

            // Установить для Label новое значение
            theLabel.Content = args.NewValue.ToString();
        }

        /*
        public int CurrentNumber
        {
            get => _currNumber;
            set
            {
                _currNumber = value;
                numberDisplay.Content = CurrentNumber.ToString();
            }
        }
        */
    }
}
