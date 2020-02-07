using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace Pomodoro_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Saved user settings
        public static int pomodoroDuration = Properties.Settings.Default.pomodoroDuration; //25 * 60; 480 max 
        public static int pomodoroBreak = Properties.Settings.Default.pomodoroBreak; //5 * 60; 480 max
        public static int pomodoroLongBreak = Properties.Settings.Default.pomodoroLongBreak; //15 * 60; 480 max
        public static int pomodoroLongBreakOccurance = Properties.Settings.Default.pomodoroLongBreakOccurance; // 100 max
        public static string workingSounds = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\" + Properties.Settings.Default.workingSounds;
        public static string alarmSounds = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\" + Properties.Settings.Default.alarmSounds;

        private static OggPlayer alarmSoundsOGG;
        private static OggPlayer workingSoundsOGG;
        private int pomodoroCount = 0;
        private startStopRestartEnum startStopBool = startStopRestartEnum.start;
        private int time;
        DispatcherTimer Timer;

        private enum startStopRestartEnum
        {
            start,
            stop,
            restart,
        }

        public MainWindow()
        {
            InitializeComponent();

            workingSoundsOGG = new OggPlayer(workingSounds, "workingSounds");
            alarmSoundsOGG = new OggPlayer(alarmSounds, "alarmSounds");
            workingSoundsOGG.Volume("workingSounds", 1000);
            alarmSoundsOGG.Volume("alarmSounds", 1000);

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += TimerTick;
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (time > 0)
            {
                time--;
                countdownTimer.Content = FormatTimer(time);
            }
            else
            {
                workingSoundsOGG.Stop("workingSounds");
                alarmSoundsOGG.Play("alarmSounds");

                pomodoroCount++;
                Timer.Stop();
                startPauseButton.Content = "OK";
                startStopBool = startStopRestartEnum.restart;
                restartButton.Visibility = Visibility.Collapsed;
            }
        }

        private void startPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if(startStopBool == startStopRestartEnum.start)
            {
                if(pomodoroCount < pomodoroLongBreakOccurance / 2)
                {
                    if(pomodoroCount % 2 == 0)
                    {
                        time = pomodoroDuration;
                    }
                    else
                    {
                        time = pomodoroBreak;
                    }
                }
                else
                {
                    time = pomodoroLongBreak;
                    pomodoroCount = 0;
                }

                workingSoundsOGG.Play("workingSounds");

                Timer.Start();
                startPauseButton.Content = "Pause";
                startStopBool = startStopRestartEnum.stop;
                restartButton.Visibility = Visibility.Visible;
            }
            else if (startStopBool == startStopRestartEnum.restart)
            {
                alarmSoundsOGG.Stop("alarmSounds");
                startPauseButton.Content = "Play";
                startStopBool = startStopRestartEnum.start;
                restartButton.Visibility = Visibility.Collapsed;
            }
            else if (startStopBool==startStopRestartEnum.stop)
            {
                workingSoundsOGG.Stop("workingSounds");
                alarmSoundsOGG.Stop("alarmSounds");

                Timer.Stop();
                startPauseButton.Content = "Play";
                startStopBool = startStopRestartEnum.start;
                restartButton.Visibility = Visibility.Collapsed;
            }

        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
            if(startStopBool == startStopRestartEnum.stop)
            {
                alarmSoundsOGG.Stop("alarmSounds");
                workingSoundsOGG.Stop("workingSounds");

                pomodoroCount = 0;
                Timer.Stop();
                countdownTimer.Content = FormatTimer(pomodoroDuration);

                startStopBool = startStopRestartEnum.start;
                startPauseButton.Content = "Play";
                restartButton.Visibility = Visibility.Collapsed;
            }
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            Window settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private static string FormatTimer(int time)
        {
            if (time / 60 < 10)
            {
                if (time % 60 < 10)
                {
                    return string.Format("0{0}:0{1}", time / 60, time % 60);
                }
                else
                {
                    return string.Format("0{0}:{1}", time / 60, time % 60);
                }
            }
            else
            {
                if (time % 60 < 10)
                {
                    return string.Format("{0}:0{1}", time / 60, time % 60);
                }
                else
                {
                    return string.Format("{0}:{1}", time / 60, time % 60);
                }
            }
        }

 
    }
}
