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
        LinearGradientBrush workingGradient = new LinearGradientBrush(
            Color.FromArgb(255, 255, 100, 55),
            Color.FromArgb(255, 251, 68, 116),
            new Point(0, 0),
            new Point(0.55, 0.52));

        LinearGradientBrush breakGradient = new LinearGradientBrush(
            Color.FromArgb(255, 23, 232, 217),
            Color.FromArgb(255, 94, 124, 234),
            new Point(0, 0),
            new Point(0.74, 0.73));

        //Saved user settings
        public static int pomodoroDuration = Properties.Settings.Default.pomodoroDuration * 60; //25 * 60; 480 max 
        public static int pomodoroBreak = Properties.Settings.Default.pomodoroBreak * 60; //5 * 60; 480 max
        public static int pomodoroLongBreak = Properties.Settings.Default.pomodoroLongBreak * 60; //15 * 60; 480 max
        public static int pomodoroLongBreakOccurance = Properties.Settings.Default.pomodoroLongBreakOccurance; // 100 max
        public static string workingSounds = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\bgm_" + Properties.Settings.Default.workingSounds + ".mp3";
        public static string alarmSounds = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\alm_" + Properties.Settings.Default.alarmSounds + ".mp3";

        public static MP3Player alarmSoundsOGG;
        public static MP3Player workingSoundsOGG;
        private int pomodoroCount = 0;
        private startStopRestartEnum startStopBool = startStopRestartEnum.start;
        private int time;
        DispatcherTimer Timer;

        private enum startStopRestartEnum
        {
            start,
            stop,
            restart,
            resume,
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Background = workingGradient;
            time = pomodoroDuration;
            countdownTimer.Content = FormatTimer(time);

            workingSoundsOGG = new MP3Player(workingSounds, "workingSounds");
            alarmSoundsOGG = new MP3Player(alarmSounds, "alarmSounds");
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
                if(pomodoroCount < pomodoroLongBreakOccurance * 2 + 1)
                {
                    if(pomodoroCount % 2 == 0)
                    {
                        this.Background = workingGradient;
                        startPauseButton.Visibility = Visibility.Visible;
                        restartButton.Visibility = Visibility.Visible;
                        doneBreakButton.Visibility = Visibility.Collapsed;
                        time = pomodoroDuration;
                    }
                    else
                    {
                        this.Background = breakGradient;
                        startPauseButton.Visibility = Visibility.Collapsed;
                        restartButton.Visibility = Visibility.Collapsed;
                        doneBreakButton.Visibility = Visibility.Visible;
                        time = pomodoroBreak;
                    }
                }
                else
                {
                    this.Background = breakGradient;
                    startPauseButton.Visibility = Visibility.Collapsed;
                    restartButton.Visibility = Visibility.Collapsed;
                    doneBreakButton.Visibility = Visibility.Visible;
                    time = pomodoroLongBreak;
                    pomodoroCount = -1;
                }

                workingSoundsOGG.Play("workingSounds");

                Timer.Start();
                startPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-pause-big.png")));
                startStopBool = startStopRestartEnum.stop;
            }
            else if (startStopBool == startStopRestartEnum.restart)
            {
                alarmSoundsOGG.Stop("alarmSounds");
                startPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-start-big.png")));
                startStopBool = startStopRestartEnum.start;
                restartButton.Visibility = Visibility.Collapsed;
            }
            else if (startStopBool==startStopRestartEnum.stop)
            {
                workingSoundsOGG.Stop("workingSounds");
                alarmSoundsOGG.Stop("alarmSounds");

                Timer.Stop();
                startPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-start-big.png")));
                startStopBool = startStopRestartEnum.resume;
                restartButton.Visibility = Visibility.Collapsed;
            }
            else if(startStopBool == startStopRestartEnum.resume)
            {
                workingSoundsOGG.Play("workingSounds");
                Timer.Start();
                startPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-pause-big.png")));
                startStopBool = startStopRestartEnum.stop;
                restartButton.Visibility = Visibility.Visible;
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
                startPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-start-big.png")));
                restartButton.Visibility = Visibility.Collapsed;
            }
        }

        private void doneBreakButton_Click(object sender, RoutedEventArgs e)
        {
            alarmSoundsOGG.Stop("alarmSounds");
            workingSoundsOGG.Stop("workingSounds");
            Timer.Stop();
            countdownTimer.Content = FormatTimer(pomodoroDuration);

            startStopBool = startStopRestartEnum.start;
            startPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-start-big.png")));
            doneBreakButton.Visibility = Visibility.Collapsed;
            startPauseButton.Visibility = Visibility.Visible;

            if (time > 0)
            {
                pomodoroCount++;
            }
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            Window settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private static string FormatTimer(int time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
