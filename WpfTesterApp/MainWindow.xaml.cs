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

namespace WpfTesterApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            this.Closing += MainWindow_Closing;
            this.MouseMove += MainWindow_MouseMove;
            this.KeyDown += MainWindowOn_KeyDown;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Выяснить, на самом ли деле пользователь хочет закрыть окно
            string msg = "Do you want to close without saving?";
            MessageBoxResult result = MessageBox.Show(msg, "My App", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                // Если пользователь не желает закрыть окно, тогда отменить закрытие
                e.Cancel = true;
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            MessageBox.Show("See ya!", "My App");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Указал ли пользователь /godmode?
#pragma warning disable CS8605 // Распаковка-преобразование вероятного значения NULL.
            if ((bool)Application.Current.Properties["GodMode"])
            {
                MessageBox.Show("Cheater!");
            }
#pragma warning restore CS8605 // Распаковка-преобразование вероятного значения NULL.
        }

        private void MyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            // Отобразить в заголовке окна текущую позицию (х,у) курсора мыши
            //this.Title = e.GetPosition(this).ToString();
            this.Title = string.Format($"My App: Mouse Coordinates [{e.GetPosition(this)}]");
        }

        private void MainWindowOn_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Отобразить на кнопке информацию о нажатой клавише
            ClickMe.Content = e.Key.ToString();
        }
    }
}
