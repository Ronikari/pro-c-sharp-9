using System;
using WpfValidations.Models;
using System.Windows.Input;

namespace WpfValidations.Cmds
{
    public class ChangeColorCommand : CommandBase
    {
        public override bool CanExecute(object parameter) => parameter is Car;

        public override void Execute(object parameter)
        {
            ((Car)parameter).Color = "Pink";
        }
    }
}
