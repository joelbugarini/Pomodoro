using System;
using System.Windows;
using System.Windows.Threading;


namespace Pomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PomodoroWindow : Window
    {

        public PomodoroWindow()
        {
            InitializeComponent();
        }

        DispatcherTimer timer, seconds;
        //Rounds
        int[] rest = { 5, 5, 5, 15 };
        int WorkTime = 25;
        int round = 0;

        bool inWorkRound = true;

        TimeSpan time;


        private void StartRound() 
        {
            timer.Start();
            seconds.Start();
        }
        private void StartNewRound()
        {
            if (inWorkRound)
            {
                CreateWorkTimer();
                CreateSmallHandTimer();                
            }
            else
            {
                CreateRestTimer();
                CreateSmallHandTimer();

            }
            timer.Start();
            seconds.Start();

            ButtonNext.IsEnabled = true;
            ButtonPrevious.IsEnabled = true;
        }


        private DispatcherTimer CreateRestTimer()
        {
            timer = new DispatcherTimer();//
            timer.Interval = TimeSpan.FromMinutes(rest[round]);
            timer.Tick += OnWorkTimeEnd;

            time = new TimeSpan(0, rest[round],0);
           
            return timer;
        }

        private void NextRound()
        {
            if (inWorkRound)
            {
                if (round == rest.Length - 1) round= 0; else round++;
            }
        }

        private void PrevRound()
        {
            if (inWorkRound)
            {
                if (round == 0) round = rest.Length - 1; else round--;
            }
        }

        private DispatcherTimer CreateSmallHandTimer()
        {
            if (seconds == null)
            {
                seconds = new DispatcherTimer();
                seconds.Interval = TimeSpan.FromSeconds(1);
                seconds.Tick += OnTick;
            }
            return seconds;
        }

        private DispatcherTimer CreateWorkTimer()
        {
            timer = new DispatcherTimer();//
            timer.Interval = TimeSpan.FromMinutes(WorkTime);
            timer.Tick += OnWorkTimeEnd;

            time = new TimeSpan(0, WorkTime, 0);
            return timer;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            ButtonPlay.Visibility = Visibility.Hidden;
            ButtonPause.Visibility = Visibility.Visible;
            ButtonPrevious.IsEnabled = true;
            ButtonNext.IsEnabled = true;
            if (timer == null) StartNewRound(); else StartRound();
        }

        private void OnTick(object source, EventArgs e)
        {
            time = time.Subtract(new TimeSpan(0, 0, 1));
            TimerMinutes.Content = time.Minutes;
            TimerSeconds.Content = time.ToString("ss");
            this.Title = time.Minutes + ":" + time.Seconds;
        }

        private void OnWorkTimeEnd(object source, EventArgs e)
        {
            inWorkRound = false;

            time = TimeSpan.FromMinutes(rest[round]);
            NextRound();
            
            TimerMinutes.Content = time.Minutes;
            TimerSeconds.Content = time.ToString("ss");

            var helper = new FlashWindowHelper(Application.Current);
            // Flashes the window and taskbar 5 times and stays solid 
            // colored until user focuses the main window
            helper.FlashApplicationWindow();
            System.Media.SystemSounds.Exclamation.Play();

            ((DispatcherTimer)source).Stop();

        }

        private void OnRestTimeEnd(object source, EventArgs e)
        {
            inWorkRound = true;

            time = TimeSpan.FromMinutes(rest[round]);
            NextRound();
            
            TimerMinutes.Content = time.Minutes;
            TimerSeconds.Content = time.ToString("ss");

            var helper = new FlashWindowHelper(Application.Current);
            // Flashes the window and taskbar 5 times and stays solid 
            // colored until user focuses the main window
            helper.FlashApplicationWindow();
            System.Media.SystemSounds.Exclamation.Play();

            ((DispatcherTimer)source).Stop();


        }

        //UI Methods

        private void Dragable_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            NextRound();
            inWorkRound = !inWorkRound;
            timer.Stop();
            seconds.Stop();
            StartNewRound();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            PrevRound();
            inWorkRound = !inWorkRound;
            timer.Stop();
            seconds.Stop();
            StartNewRound();
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            seconds.Stop();
            ButtonPause.Visibility = Visibility.Hidden;
            ButtonPlay.Visibility = Visibility.Visible;
            ButtonNext.IsEnabled = false;
            ButtonPrevious.IsEnabled = false;
        }
    }
}
