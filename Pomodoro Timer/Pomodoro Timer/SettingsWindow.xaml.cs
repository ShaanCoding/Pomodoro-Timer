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

            //select one of those thingys
            var checkedValueAlarmSounds = alarmSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.Name.Equals(Properties.Settings.Default.alarmSounds));
            if(checkedValueAlarmSounds != null)
            {
                checkedValueAlarmSounds.IsChecked = true;
            }

            var checkedValueWorkingSounds = workingSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.Name.Equals(Properties.Settings.Default.workingSounds));
            if (checkedValueWorkingSounds != null)
            {
                checkedValueWorkingSounds.IsChecked = true;
            }
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.pomodoroDuration = pomodoroDurationCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroBreak = pomodoroBreakCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroLongBreak = pomodoroLongBreakCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroLongBreakOccurance = pomodoroLongBreakOccuranceCombobox.SelectedIndex + 1;

            var checkedValueAlarmSounds = alarmSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            Properties.Settings.Default.alarmSounds = checkedValueAlarmSounds.Name;

            var checkedValueWorkingSounds = workingSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            Properties.Settings.Default.workingSounds = checkedValueWorkingSounds.Name;

            Properties.Settings.Default.Save();

            MainWindow.pomodoroDuration = Properties.Settings.Default.pomodoroDuration * 60;
            MainWindow.pomodoroBreak = Properties.Settings.Default.pomodoroBreak * 60;
            MainWindow.pomodoroLongBreak = Properties.Settings.Default.pomodoroLongBreak * 60;
            MainWindow.pomodoroLongBreakOccurance = Properties.Settings.Default.pomodoroLongBreakOccurance;
            MainWindow.workingSounds = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\bgm_" + Properties.Settings.Default.workingSounds + ".mp3";
            MainWindow.alarmSounds = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\alm_" + Properties.Settings.Default.alarmSounds + ".mp3";
            MainWindow.workingSoundsOGG = new MP3Player(MainWindow.workingSounds, "workingSounds");
            MainWindow.alarmSoundsOGG = new MP3Player(MainWindow.alarmSounds, "alarmSounds");
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
