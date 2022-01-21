using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Threading;

namespace Pomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PomodoroWindow : Window
    {
        public PomodoroArray PomodoroArray { get; set; }
        public TimeSpan Streak { get; set; }
        DispatcherTimer Clock;
        public bool Test { get; set; }
        public PomodoroWindow()
        {
            InitializeComponent();
            PomodoroArray = new PomodoroArray(4, 25, 5, 15);
            Test = false;
            Streak = ToTime(25);
            CreateWorkTimer();            
        }

        private void CreateWorkTimer()
        {
            Clock = new DispatcherTimer();
            Clock.Interval = TimeSpan.FromSeconds(1);

            Clock.Tick += OnTick;            
        }
        
        private void OnTick(object source, EventArgs e)
        {
            Streak = Streak - TimeSpan.FromSeconds(1);
            if (Streak.CompareTo(TimeSpan.FromSeconds(0))==0)
            {
                if (PomodoroArray.Peek() == PomodoroArray.StreakLength) 
                {
                    Beep(Properties.Resources.right);
                }
                else 
                {
                    Beep(Properties.Resources.left);
                }
                
                PomodoroArray.Next();
                Streak = ToTime(PomodoroArray.GetCurrentStreakLength());
                TimerSeconds.Content = "00";
                TimerMinutes.Content = PomodoroArray.GetCurrentStreakLength();
            }
            else {
                TimerSeconds.Content = Streak.ToString("ss");
                TimerMinutes.Content = Streak.Minutes;
                Title = Streak.Minutes + ":" + Streak.ToString("ss");
            }                     
        }


        //Buttons
        #region
        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            ButtonPlay.Visibility = Visibility.Hidden;
            ButtonPause.Visibility = Visibility.Visible;
            ButtonPrevious.IsEnabled = true;
            ButtonNext.IsEnabled = true;

            Clock.Start();
        }


        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            PomodoroArray.Next();
            Streak = ToTime(PomodoroArray.GetCurrentStreakLength());
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            PomodoroArray.Previous();
            Streak = ToTime(PomodoroArray.GetCurrentStreakLength());
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            ButtonPause.Visibility = Visibility.Hidden;
            ButtonPlay.Visibility = Visibility.Visible;
            ButtonNext.IsEnabled = false;
            ButtonPrevious.IsEnabled = false;

            Title = "Pomodoro - Paused";

            Clock.Stop();
        }

        #endregion

        public void Beep(UnmanagedMemoryStream sound_resource)
        {
            var helper = new FlashWindowHelper(Application.Current);
            helper.FlashApplicationWindow();
            SoundPlayer snd = new SoundPlayer(sound_resource);
            snd.Play();
        }

        public TimeSpan ToTime(int number)
        {
           return (Test) ? TimeSpan.FromSeconds(number) : TimeSpan.FromMinutes(number);
        }
    }
}
