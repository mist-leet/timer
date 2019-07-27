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

namespace timer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool is_time_goes = false;
        public MainWindow()
        {
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
                txts[i].TextChanged += UiTxtChange;
                txts[i].MaxLength = 2;
                txts[i].MaxLines = 1;
            }
            btn_ui_play.Click += UiBtnStartClick;

            main_timer_s.TextChanged += UiTxtChange;
            main_timer_m.TextChanged += UiTxtChange;
            main_timer_h.TextChanged += UiTxtChange;

            main_timer_dots1.IsReadOnly = true;
            main_timer_dots2.IsReadOnly = true;

            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(TimerTick);
            Timer.Interval= new TimeSpan(0, 0, 1);
            Timer.Start();
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
                is_time_goes = true;
            }
        }

        void UiTxtChange(object sender, RoutedEventArgs args)
        {
            if (((TextBox)sender).Text.Length == 1)
                ((TextBox)sender).Text = "0" + ((TextBox)sender).Text;
            else if (((TextBox)sender).Text.Length == 0)
                ((TextBox)sender).Text = "00";
        }

        string[] DateToStringFormat(DateTime t)
        {
            string[] time =
            {
                t.ToLongTimeString().Substring(0,1), 
                t.ToLongTimeString().Substring(2,2),
                t.ToLongTimeString().Substring(5,2)
                // 01:34:57
                // 0:23:56
            };
            return time;
        }

        DateTime CurrentTimerTime()
        {
            return new DateTime(1, 1, 1, Convert.ToInt32(main_timer_h.Text), Convert.ToInt32(main_timer_m.Text), Convert.ToInt32(main_timer_s.Text));
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (is_time_goes)
            {
                DateTime current_time = CurrentTimerTime();
                current_time = current_time.AddSeconds(-1);

                string[] new_time = DateToStringFormat(current_time);
                main_timer_h.Text = new_time[0];
                main_timer_m.Text = new_time[1];
                main_timer_s.Text = new_time[2];
            }
        }
    }
}
