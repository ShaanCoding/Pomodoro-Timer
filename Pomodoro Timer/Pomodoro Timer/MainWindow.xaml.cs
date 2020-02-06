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
        private const int pomodoroDuration = 10; //25 * 60;
        private const int pomodoroBreak = 5; //5 * 60;
        private const int pomodoroLongBreak = 15; //15 * 60;
        private const int pomodoroLongBreakOccurance = 4;
        private static string workingSounds = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\train.mp3";
        private static OggPlayer workingSoundsOGG;
        private static string alarmSounds = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\fan.mp3";
        private static OggPlayer alarmSoundsOGG;



        private const bool popUpNotification = true;

        private const bool allowResizing = false;
        private const bool detachFromSYSTray = false;


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
            }
        }

        private void startPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if(startStopBool == startStopRestartEnum.start)
            {
                if(pomodoroCount < pomodoroLongBreakOccurance / 2)
                {
                    //if even i.e no break
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
            }
            else if (startStopBool == startStopRestartEnum.restart)
            {
                alarmSoundsOGG.Stop("alarmSounds");
                startPauseButton.Content = "Pause";
                startStopBool = startStopRestartEnum.start;
            }
            else if (startStopBool==startStopRestartEnum.stop)
            {
                workingSoundsOGG.Stop("workingSounds");
                alarmSoundsOGG.Stop("alarmSounds");

                Timer.Stop();
                startPauseButton.Content = "Play";
                startStopBool = startStopRestartEnum.start;
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
            }
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {

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
