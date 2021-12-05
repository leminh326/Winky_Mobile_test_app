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

using Windows.UI.ViewManagement;


namespace Winky_Mobile_Test_Prototype_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Disable_All();
            Scenario_Frame.Navigate(typeof(Welcome_Page));
        }

        private void Info_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Info_Btn.IsChecked == true)
            {
                //Scenario_Frame.Navigate(typeof(Page1_Info));
                Unchecked_All();
                Info_Btn.IsChecked = true;
            }
            else { }
        }

        private void Control_Panel_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Control_Panel_Btn.IsChecked == true)
            {
                Scenario_Frame.Navigate(typeof(Control_Panel_Page));
                Unchecked_All();
                Control_Panel_Btn.IsChecked = true;
            }
            else { }
        }

        private void Admin_Service_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Admin_Service_Btn.IsChecked == true)
            {
                Scenario_Frame.Navigate(typeof(Admin_Page));
                Unchecked_All();
                Admin_Service_Btn.IsChecked = true;
            }
            else { }
        }

        private void Function_2_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Function_2_Btn.IsChecked == true)
            {
                //Scenario_Frame.Navigate(typeof(Page1_Info));
                Unchecked_All();
                Function_2_Btn.IsChecked = true;
            }
            else { }
        }

        private void Function_3_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Function_3_Btn.IsChecked == true)
            {
                //Scenario_Frame.Navigate(typeof(Page1_Info));
                Unchecked_All();
                Function_3_Btn.IsChecked = true;
            }
            else { }
        }

        private void Function_4_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Function_4_Btn.IsChecked == true)
            {
                //Scenario_Frame.Navigate(typeof(Page1_Info));
                Unchecked_All();
                Function_4_Btn.IsChecked = true;
            }
            else { }
        }

        private void Function_5_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Function_5_Btn.IsChecked == true)
            {
                //Scenario_Frame.Navigate(typeof(Page1_Info));
                Unchecked_All();
                Function_5_Btn.IsChecked = true;
            }
            else { }
        }

        private void Function_6_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Function_6_Btn.IsChecked == true)
            {
                //Scenario_Frame.Navigate(typeof(Page1_Info));
                Unchecked_All();
                Function_6_Btn.IsChecked = true;
            }
            else { }
        }

        private void Function_7_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Function_7_Btn.IsChecked == true)
            {
                //Scenario_Frame.Navigate(typeof(Page1_Info));
                Unchecked_All();
                Function_7_Btn.IsChecked = true;
            }
            else { }
        }

        private void Function_8_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Function_8_Btn.IsChecked == true)
            {
                //Scenario_Frame.Navigate(typeof(Page1_Info));
                Unchecked_All();
                Function_8_Btn.IsChecked = true;
            }
            else { }
        }

        public void Unchecked_All()
        {
            Info_Btn.IsChecked = false;
            Admin_Service_Btn.IsChecked = false;
            Control_Panel_Btn.IsChecked = false;
            Auto_Mode_Btn.IsChecked = false;
            Function_2_Btn.IsChecked = false;
            Function_3_Btn.IsChecked = false;
            Function_4_Btn.IsChecked = false;
            Function_5_Btn.IsChecked = false;
            Function_6_Btn.IsChecked = false;
            Function_7_Btn.IsChecked = false;
            Function_8_Btn.IsChecked = false;
        }
        public void Enable_All()
        {
            Info_Btn.IsEnabled = true;
            Admin_Service_Btn.IsEnabled = true;
            Control_Panel_Btn.IsEnabled = true;
            Auto_Mode_Btn.IsEnabled = true;
            Function_2_Btn.IsEnabled = true;
            Function_3_Btn.IsEnabled = true;
            Function_4_Btn.IsEnabled = true;
            Function_5_Btn.IsEnabled = true;
            Function_6_Btn.IsEnabled = true;
            Function_7_Btn.IsEnabled = true;
            Function_8_Btn.IsEnabled = true;
        }
        public void Disable_All()
        {
            Info_Btn.IsEnabled = false;
            Admin_Service_Btn.IsEnabled = false;
            Control_Panel_Btn.IsEnabled = false;
            Auto_Mode_Btn.IsEnabled = false;
            Function_2_Btn.IsEnabled = false;
            Function_3_Btn.IsEnabled = false;
            Function_4_Btn.IsEnabled = false;
            Function_5_Btn.IsEnabled = false;
            Function_6_Btn.IsEnabled = false;
            Function_7_Btn.IsEnabled = false;
            Function_8_Btn.IsEnabled = false;
        }

        private void Auto_Mode_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Auto_Mode_Btn.IsChecked == true)
            {
                Scenario_Frame.Navigate(typeof(Auto_Mode_Page));
                Unchecked_All();
                Auto_Mode_Btn.IsChecked = true;
            }
            else { }
        }
    }
}
