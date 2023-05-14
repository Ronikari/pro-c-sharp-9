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

namespace WpfRoutedEvents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _mouseActivity = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnClickMe_Clicked(object sender, RoutedEventArgs e)
        {
            // Делать что-нибудь, когда на кнопке произведен щелчок
            MessageBox.Show("Нажал(а) на кнопку");

            AddEventInfo(sender, e);
            MessageBox.Show(_mouseActivity, "Your Event Info");

            // Очистить строку для следующего цикла
            _mouseActivity = "";
        }

        private void AddEventInfo(object sender, RoutedEventArgs e)
        {
            _mouseActivity += string.Format("{0} sent a {1} event named {2}.\n", sender,
                e.RoutedEvent.RoutingStrategy, e.RoutedEvent.Name);
        }

        private void outerEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Нажал(а) на писечку ^_^");
            AddEventInfo(sender, e);
        }

        private void innerEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Нажал(а) на клиторочек ^_^");
            AddEventInfo(sender, e);
        }
    }
}
