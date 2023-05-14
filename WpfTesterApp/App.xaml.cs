﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfTesterApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Properties["GodMode"] = false;
            // Проверить входные аргументы командной строки на предмет наличия флага /GODMODE
            foreach(string arg in e.Args)
            {
                if(arg.Equals("/godmode", StringComparison.OrdinalIgnoreCase))
                {
                    Application.Current.Properties["GodMode"] = true;
                    break;
                }
            }
        }
        private void App_OnExit(object sender, ExitEventArgs e)
        {

        }
    }
}
