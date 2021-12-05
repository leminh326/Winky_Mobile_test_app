using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Winky_Mobile_Test_Prototype_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Auto_Mode_Page : Page
    {
        private MainPage rootPage = MainPage.Current;
        bool auto_mode_on = false;

        public Auto_Mode_Page()
        {
            this.InitializeComponent();
        }

        private void Auto_Mode_Btn_Click(object sender, RoutedEventArgs e)
        {
            AutoModeTask();
        }
        private async Task AutoModeTask()
        {
            if (Auto_Mode_Btn.IsChecked == true)
            {
                Auto_Mode_Btn.Content = "Stop Auto Mode";
                auto_mode_on = true;
                int direction = 0;

                while (auto_mode_on == true)
                {
                    await Task.Delay(Convert.ToInt16(Auto_Mode_Tbox.Text));
                    if (Auto_Mode_Btn.IsChecked == true)
                    {
                        if (direction == 0)
                        {
                            direction = 1;
                            rootPage.Motor_Power_Command(2, (short)-(Convert.ToInt16(Power_Auto_Mode_Tbox.Text)));
                        }
                        else
                        {
                            direction = 0;
                            rootPage.Motor_Power_Command(2, (short)(Convert.ToInt16(Power_Auto_Mode_Tbox.Text)));
                        }
                    }
                    else { auto_mode_on = false; rootPage.Motor_Power_Command(2, 0); }
                }
            }
            else
            {
                Auto_Mode_Btn.Content = "Start Auto Mode";
                auto_mode_on = false;
                rootPage.Motor_Power_Command(2, 0);
            }
        }
    }
}
