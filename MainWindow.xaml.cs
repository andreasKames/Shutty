using System.Printing;
using System.Text;
using System.Timers;
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

namespace Shutty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int totalMinutes;
        private DispatcherTimer timer;
        private int counterSeconds;
        public MainWindow()
        {
            InitializeComponent();
            counterSeconds = totalMinutes * 60;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_tick;
        }

        private void Timer_tick(object? sender, EventArgs e)
        {
            PrintLabelCountdown(counterSeconds);
            counterSeconds--;
        }

        private void PrintLabelCountdown(int counterSeconds)
        {
            int remainingMinutesAndSeconds, timerHour, timerMinute, timerSecond;
            timerHour = counterSeconds / 3600;
            remainingMinutesAndSeconds = counterSeconds % 3600;
            timerMinute = remainingMinutesAndSeconds / 60;
            timerSecond = remainingMinutesAndSeconds % 60;
            Label_Remaining_Time.Content = String.Format("{0,2:2}:{1,2:2}:{2,2:2}", timerHour.ToString().PadLeft(2,'0'), 
                timerMinute.ToString().PadLeft(2, '0'), timerSecond.ToString().PadLeft(2, '0'));
        }

        private void Button_Herunterfahren(object sender, RoutedEventArgs e)
        {
            HerunterfahrenAbbrechen();
            Herunterfahren();
            
            timer.Stop();
            counterSeconds = totalMinutes * 60;
            //PrintLabelCountdown(counterSeconds);
            timer.Start();           
        }

        private void HerunterfahrenAbbrechen()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C shutdown /a";
            process.StartInfo = startInfo;
            process.Start();
            timer.Stop();
        }

        private void Herunterfahren()
        {
            int shutdownTimeInSeconds = totalMinutes * 60;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "shutdown.exe";
            startInfo.Arguments = "/C /f /s /t " + shutdownTimeInSeconds;
            process.StartInfo = startInfo;
            process.Start();
        }

        private void Button_Herunterfahren_Abbrechen(object sender, RoutedEventArgs e)
        {
            HerunterfahrenAbbrechen();
            counterSeconds = totalMinutes * 60;
            PrintLabelCountdown(counterSeconds);
        }

        private void Button_plus1h(object sender, RoutedEventArgs e)
        {
            modifyTimer(60);

        }        

        private void Button_plus15min(object sender, RoutedEventArgs e)
        {
            modifyTimer(15);
        }

        private void Button_minus15min(object sender, RoutedEventArgs e)
        {
            modifyTimer(-15);
        }

        private void Button_minus1h(object sender, RoutedEventArgs e)
        {
            modifyTimer(-60);
        }
        private void modifyTimer(int time)
        {
            Button_minus15minObj.IsEnabled = true;
            Button_minus1hObj.IsEnabled = true;
            Button_HerunterfahrenObj.IsEnabled = true;

            string timer = "";

            this.totalMinutes += time;
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            if (totalMinutes < 15) { 
                Button_HerunterfahrenObj.IsEnabled = false;
            }

            if (totalMinutes <= 15)
            {
                Button_minus15minObj.IsEnabled = false;
                Button_minus1hObj.IsEnabled = false;                
            }
            if (totalMinutes <= 60)
            {
                Button_minus1hObj.IsEnabled = false; 
            }
            if (hours < 10)
            {
                timer = "0";
            }
            timer += hours + ":";
            
            if (minutes == 0)
            {
                timer += "0";
            }
            timer += minutes;

            Label_adjust_Timer.Content = timer;
        }
    }
}