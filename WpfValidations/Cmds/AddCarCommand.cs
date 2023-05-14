using System;
using System.Collections.ObjectModel;
using System.Linq;
using WpfValidations.Models;

namespace WpfValidations.Cmds
{
    public class AddCarCommand : CommandBase
    {
        public override bool CanExecute(object parameter) => parameter is ObservableCollection<Car>;
        public override void Execute(object parameter)
        {
            if (parameter is not ObservableCollection<Car> cars) return;
            var maxCount = cars.Max(x => x.Id);
            cars.Add(new Car
            {
                Id = ++maxCount,
                Color = "Yellow",
                Make = "VW",
                PetName = "Birdie"
            });
        }
    }
}
