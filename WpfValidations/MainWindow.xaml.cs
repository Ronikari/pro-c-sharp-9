using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfValidations.Models;
using WpfValidations.Cmds;

namespace WpfValidations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IList<Car> _cars = new ObservableCollection<Car>();
        private ICommand _addCarCommand = null;
        private ICommand _changeColorCommand = null;
        private RelayCommand<Car> _deleteCarCommand = null;

        public ICommand AddCarCmd => _addCarCommand ??= new AddCarCommand();
        public ICommand ChangeColorCmd => _changeColorCommand ??= new ChangeColorCommand();
        public RelayCommand<Car> DeleteCarCmd => _deleteCarCommand ??= new RelayCommand<Car>(DeleteCar, CanDeleteCar);

        public MainWindow()
        {
            InitializeComponent();
            _cars.Add(new Car { Id = 1, Color = "Blue", Make = "Chevy", PetName = "Kit", IsChanged = false });
            _cars.Add(new Car { Id = 2, Color = "Red", Make = "Ford", PetName = "Red Rider", IsChanged = false });
            cboCars.ItemsSource = _cars;
        }

        private bool CanDeleteCar(Car car) => car != null;
        private void DeleteCar(Car car) => _cars.Remove(car);
    }
}
