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
using System.Windows.Shapes;

namespace Pomodoro_Timer
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            for (int i = 1; i <= 480; i++)
            {
                pomodoroDurationCombobox.Items.Add(i.ToString());
                pomodoroBreakCombobox.Items.Add(i.ToString());
                pomodoroLongBreakCombobox.Items.Add(i.ToString());
            }

            for(int i = 1; i <= 100; i++)
            {
                pomodoroLongBreakOccuranceCombobox.Items.Add(i.ToString());
            }
            //add population of songs

            pomodoroDurationCombobox.SelectedIndex = Properties.Settings.Default.pomodoroDuration - 1;
            pomodoroBreakCombobox.SelectedIndex = Properties.Settings.Default.pomodoroBreak - 1;
            pomodoroLongBreakCombobox.SelectedIndex = Properties.Settings.Default.pomodoroLongBreak - 1;
            pomodoroLongBreakOccuranceCombobox.SelectedIndex = Properties.Settings.Default.pomodoroLongBreakOccurance - 1;
            //add song
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.pomodoroDuration = pomodoroDurationCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroBreak = pomodoroBreakCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroLongBreak = pomodoroLongBreakCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroLongBreakOccurance = pomodoroLongBreakOccuranceCombobox.SelectedIndex + 1;
            //add songs
            Properties.Settings.Default.Save();

            MainWindow.pomodoroDuration = pomodoroDurationCombobox.SelectedIndex + 1; ;
            MainWindow.pomodoroBreak = pomodoroBreakCombobox.SelectedIndex + 1; ;
            MainWindow.pomodoroLongBreak = pomodoroLongBreakCombobox.SelectedIndex + 1; ;
            MainWindow.pomodoroLongBreakOccurance = pomodoroLongBreakOccuranceCombobox.SelectedIndex + 1;

            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
