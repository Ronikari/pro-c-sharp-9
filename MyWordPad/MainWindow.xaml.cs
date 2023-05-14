using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace MyWordPad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetF1CommandBinding();
        }

        private void SetF1CommandBinding()
        {
            CommandBinding helpBinding = new CommandBinding(ApplicationCommands.Help);
            helpBinding.CanExecute += CanHelpExecute;
            helpBinding.Executed += HelpExecuted;
            CommandBindings.Add(helpBinding);
        }

        protected void FileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        protected void ToolsSpellingHints_Click(object sender, RoutedEventArgs e)
        {
            string spellingHints = string.Empty;
            // Попробовать получить ошибку правописания в текущем положении курсора ввода
            SpellingError error = txtData.GetSpellingError(txtData.CaretIndex);
            if (error != null)
            {
                // Построить строку с предполагаемыми вариантами правописания
                foreach (string s in error.Suggestions)
                {
                    spellingHints += $"{s}\n";
                }

                // Отобразить предполагаемые варианты и раскрыть элемент Expander
                lblSpellingHints.Content = spellingHints;
                expanderSpelling.IsExpanded = true;
            }
        }
        protected void MouseEnterExitArea(object sender, RoutedEventArgs e) 
        {
            statBarText.Text = "Закрыть приложение";
        }
        protected void MouseEnterToolsHintsArea(object sender, RoutedEventArgs e)
        {
            statBarText.Text = "Показать варианты правописания";
        }
        protected void MouseLeaveArea(object sender, RoutedEventArgs e)
        {
            statBarText.Text = "Готово";
        }
        private void CanHelpExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Если нужно предотвратить выполнение команды, то можно установить CanExecute в false
            e.CanExecute = true;
        }
        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Посмотрите, здесь нет ничего сложного. Просто напишите что-нибудь!", "Помощь");
        }
        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Создать диалоговое окно открытия файла и показать в нем только текстовые файлы
            var openDlg = new OpenFileDialog { Filter = "Text Files |*.txt" };

            // Был ли совершен щелчок на кнопке ОК?
            if (openDlg.ShowDialog() == true)
            {
                // Загрузить содержимое выбранного файла
                string dataFromFile = File.ReadAllText(openDlg.FileName);

                // Отобразить строку в TextBox
                txtData.Text = dataFromFile;
            }
        }
        private void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var saveDlg = new SaveFileDialog { Filter = "Text Files |*.txt" };

            // Был ли совершен щелчок на кнопке ОК?
            if (saveDlg.ShowDialog() == true)
            {
                // Сохранить данные из TextBox в указанном файле
                File.WriteAllText(saveDlg.FileName, txtData.Text);
            }
        }
    }
}
