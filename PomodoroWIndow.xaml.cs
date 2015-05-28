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
        int[] worker = { 25, 5, 25, 5, 25, 5, 25, 15 };
        int round = 0;


        TimeSpan time;


        //Buttons
        #region
        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            ButtonPlay.Visibility = Visibility.Hidden;
            ButtonPause.Visibility = Visibility.Visible;
            ButtonPrevious.IsEnabled = true;
            ButtonNext.IsEnabled = true;
            if (timer == null) StartNewRound(); else StartRound();
        }

        private void Dragable_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void ButonWindow_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            NextRound();
            timer.Stop();
            seconds.Stop();
            StartNewRound();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            PrevRound();
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
        #endregion


        private TimeSpan UpdateTime(int Time)
        {
            time = new TimeSpan(0, Time, 0);
            return time;
        }
        
        private void StartRound() 
        {
            timer.Start();
            seconds.Start();
        }

        private void StartNewRound()
        {
            CreateWorkTimer();
            CreateSmallHandTimer();

            timer.Start();
            seconds.Start();

            ButtonNext.IsEnabled = true;
            ButtonPrevious.IsEnabled = true;
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
            timer.Interval = UpdateTime(worker[round]);
            timer.Tick += OnWorkTimeEnd;

            return timer;
        }

        private void NextRound()
        {
            if (round == worker.Length - 1) round = 0; else round++;
        }

        private void PrevRound()
        {
            if (round == 0) round = worker.Length - 1; else round--;
        }

        private void OnTick(object source, EventArgs e)
        {
            time = time.Subtract(new TimeSpan(0, 0, 1));
            TimerMinutes.Content = time.Minutes;
            TimerSeconds.Content = time.ToString("ss");
            this.Title = time.ToString("mm") + ":" + time.ToString("ss");
        }

        private void OnWorkTimeEnd(object source, EventArgs e)
        {
            NextRound();   

            var helper = new FlashWindowHelper(Application.Current);
            helper.FlashApplicationWindow();
            System.Media.SystemSounds.Exclamation.Play();

            ((DispatcherTimer)source).Stop();
            timer = null;
            StartNewRound();
            timer.Start();
        }

       
    }
}
