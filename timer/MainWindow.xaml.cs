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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using System.Media;

namespace timer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool is_time_goes = false;
        bool is_time_watch_goes = false;
        DateTime current_watch_time;
        SoundPlayer sp = new SoundPlayer("aliens_game_over2.wav");
        int sp_tick = 0;
        public MainWindow()
        {
            sp.Load();
            
            InitializeComponent();

            Button[] btns =
            {
                btn_ui_1, btn_ui_2, btn_ui_3, btn_ui_4
            };
            TextBox[] txts =
            {
                main_timer_s,
                main_timer_m,
                main_timer_h
            };

            for (int i = 0; i < 4; i++)
                btns[i].Click += UiBtnClick;

            for (int i = 0; i < 3; i++)
            {
                txts[i].MaxLength = 2;
                txts[i].MaxLines = 1;
            }
            btn_ui_play.Click += UiBtnStartClick;

            main_timer_dots1.IsReadOnly = true;
            main_timer_dots2.IsReadOnly = true;

            btn_tab_1.MouseLeftButtonUp += UiBtnTabClick;
            btn_tab_2.MouseLeftButtonUp += UiBtnTabClick;

            // Watch 

            btn_watch_play.Click += UiBtnWatchPlayClick;
            btn_watch_pause.Click += UiBtnWatchPauseClick;
            btn_watch_clear.Click += UiBtnWatchClearClick;
            current_watch_time = new DateTime(1, 1, 1, 0, 0, 0);

            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(TimerTick);
            Timer.Interval= new TimeSpan(0, 0, 0, 1);
            Timer.Start();
        }

        // Timer
        void UiBtnTabClick(object sender, RoutedEventArgs args)
        {
            if (Functions.SelectedItem == StopWatch)
            {
                Functions.SelectedItem = Timer;
                btn_tab_1.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            else
            {
                Functions.SelectedItem = StopWatch;
                btn_tab_2.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            }
        }

        void UiBtnClick(object sender, RoutedEventArgs args)
        {
            string name = ((Button)sender).Name;
            name = name.Substring(7, 1);

            switch (name)
            {
                case "1":
                    {
                        main_timer_h.Text = "00";
                        main_timer_m.Text = "05";
                        main_timer_s.Text = "00";
                        break;
                    }
                case "2":
                    {
                        main_timer_h.Text = "00";
                        main_timer_m.Text = "15";
                        main_timer_s.Text = "00";
                        break;
                    }
                case "3":
                    {
                        main_timer_h.Text = "00";
                        main_timer_m.Text = "30";
                        main_timer_s.Text = "00";
                        break;
                    }
                case "4":
                    {
                        main_timer_h.Text = "01";
                        main_timer_m.Text = "00";
                        main_timer_s.Text = "00";
                        break;
                    }
                default:
                    break;
            }
            is_time_goes = false;
        }

        void UiBtnStartClick(object sender, RoutedEventArgs args)
        {
            if (is_time_goes)
            {
                is_time_goes = false;
            }
            else
            {
                sp_tick = 0;
                TextBox[] txts =
                {
                main_timer_s,
                main_timer_m,
                main_timer_h
                };

                for(int i = 0; i < 3; i++)
                {
                    if (txts[i].Text.Length == 0)
                        txts[i].Text = "00";
                    else if (txts[i].Text.Length == 1)
                        txts[i].Text = "0" + txts[i].Text;
                    if (Convert.ToInt32(txts[i].Text) > 59)
                        txts[i].Text = "59";
                }
                if (Convert.ToInt32(txts[2].Text) > 23)
                    txts[2].Text = "23";
                is_time_goes = true;
            }
        }

        string[] DateToStringFormat(DateTime t)
        {
            string[] time;
            if (t.ToLongTimeString().Substring(1, 1) == ":")
            {
                // 0:23:56
                time = new string[]
                {
                    "0" + t.ToLongTimeString().Substring(0,1),
                    t.ToLongTimeString().Substring(2,2),
                    t.ToLongTimeString().Substring(5,2)
                };
            }
            else
            {
                // 01:34:57
                time = new string[]
                {
                    t.ToLongTimeString().Substring(0,2),
                    t.ToLongTimeString().Substring(3,2),
                    t.ToLongTimeString().Substring(6,2)
                };
            }
            return time;
        }

        DateTime CurrentTimerTime()
        {
            return new DateTime(1, 1, 1, Convert.ToInt32(main_timer_h.Text), Convert.ToInt32(main_timer_m.Text), Convert.ToInt32(main_timer_s.Text));
        }

        bool IsStop()
        {
            if (main_timer_s.Text == "00" &&
                main_timer_m.Text == "00" &&
                main_timer_h.Text == "00")
                return true;
            return false;
        }

        // Watch

        void UiBtnWatchPlayClick(object sender, RoutedEventArgs args)
        {
            if (!is_time_watch_goes)
                is_time_watch_goes = true;
        }

        void UiBtnWatchPauseClick(object sender, RoutedEventArgs args)
        {
            if (is_time_watch_goes)
                is_time_watch_goes = false;
        }

        void UiBtnWatchClearClick(object sender, RoutedEventArgs args)
        {
            txt_watch_timer.Text = "00:00:00";
            is_time_watch_goes = false;
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (is_time_goes && !IsStop())
            {
                DateTime current_time = CurrentTimerTime();
                current_time = current_time.AddSeconds(-1);

                string[] new_time = DateToStringFormat(current_time);
                main_timer_h.Text = new_time[0];
                main_timer_m.Text = new_time[1];
                main_timer_s.Text = new_time[2];
            }
            if (IsStop() && is_time_goes && sp_tick < 3)
            {
                sp_tick++;
                sp.Play();
            }
            if (is_time_watch_goes)
            {
                current_watch_time = current_watch_time.AddSeconds(1);

                // 01:34:67
                // 0:23:56
                if (current_watch_time.ToLongTimeString().Length == 7)
                    txt_watch_timer.Text = "0" + current_watch_time.ToLongTimeString();
                else
                    txt_watch_timer.Text = current_watch_time.ToLongTimeString();
            }

        }
    }
}
