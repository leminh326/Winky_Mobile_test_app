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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Winky_Mobile_Test_Prototype_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Control_Panel_Page : Page
    {
        private MainPage rootPage = MainPage.Current;
        public Control_Panel_Page()
        {
            this.InitializeComponent();
            rootPage.Enable_All();
        }

        private void Left_Motor_Slider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            short Left_Motor_Value = (short)Left_Motor_Slider.Value;
            Left_Motor_Tbox.Text = Left_Motor_Slider.Value.ToString();

            if ((Left_Motor_Value <= 100) && (Left_Motor_Value >= -100))
            {
                rootPage.Motor_Power_Command(0, Left_Motor_Value);
            }
        }

        private void Right_Motor_Slider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            short Right_Motor_Value = (short)Right_Motor_Slider.Value;
            Right_Motor_Tbox.Text = Right_Motor_Slider.Value.ToString();

            if ((Right_Motor_Value <= 100) && (Right_Motor_Value >= -100))
            {
                rootPage.Motor_Power_Command(1, Right_Motor_Value);
            }
        }

        private void Both_Motors_Slider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            short Both_Motors_Value = (short)Both_Motors_Slider.Value;

            Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
            Left_Motor_Tbox.Text  = Both_Motors_Slider.Value.ToString();
            Right_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
            Left_Motor_Slider.Value  = Both_Motors_Slider.Value;
            Right_Motor_Slider.Value = Both_Motors_Slider.Value;

            if ((Both_Motors_Value <= 100) && (Both_Motors_Value >= -100))
            {
                rootPage.Motor_Power_Command(2, Both_Motors_Value);
            }
        }

        private void Left_Motor_Tbox_LostFocus(object sender, RoutedEventArgs e)
        {
            short Left_Motor_Value = Convert.ToInt16(Left_Motor_Tbox.Text);
            if ((Left_Motor_Value <= 100) && (Left_Motor_Value >= -100))
            {
                Left_Motor_Slider.Value = Convert.ToInt16(Left_Motor_Tbox.Text);
                rootPage.Motor_Power_Command(0, Left_Motor_Value);
            }
        }

        private void Right_Motor_Tbox_LostFocus(object sender, RoutedEventArgs e)
        {
            short Right_Motor_Value = Convert.ToInt16(Right_Motor_Tbox.Text);
            if ((Right_Motor_Value <= 100) && (Right_Motor_Value >= -100))
            {
                Right_Motor_Slider.Value = Convert.ToInt16(Right_Motor_Tbox.Text);
                rootPage.Motor_Power_Command(1, Right_Motor_Value);
            }
        }

        private void Both_Motors_Tbox_LostFocus(object sender, RoutedEventArgs e)
        {
            short Both_Motors_Value = Convert.ToInt16(Both_Motors_Tbox.Text);
            if ((Both_Motors_Value <= 100) && (Both_Motors_Value >= -100))
            {
                Both_Motors_Slider.Value = Convert.ToInt16(Both_Motors_Tbox.Text);
                Left_Motor_Slider.Value  = Convert.ToInt16(Both_Motors_Tbox.Text);
                Right_Motor_Slider.Value = Convert.ToInt16(Both_Motors_Tbox.Text);
                Right_Motor_Tbox.Text = Both_Motors_Tbox.Text;
                Left_Motor_Tbox.Text  = Both_Motors_Tbox.Text;

                rootPage.Motor_Power_Command(2, Both_Motors_Value);
            }
        }

        private void Plus_Left_Motor_Btn_Click(object sender, RoutedEventArgs e)
        {            
            if (Left_Motor_Slider.Value <= 90)
            {
                Left_Motor_Slider.Value = Left_Motor_Slider.Value + 10;
                short Left_Motor_Value = (short)Left_Motor_Slider.Value;
                Left_Motor_Tbox.Text = Left_Motor_Slider.Value.ToString();
                rootPage.Motor_Power_Command(0, Left_Motor_Value);
            }           
        }

        private void Plus_Right_Motor_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Right_Motor_Slider.Value <= 90)
            {
                Right_Motor_Slider.Value = Right_Motor_Slider.Value + 10;
                short Right_Motor_Value = (short)Right_Motor_Slider.Value;
                Right_Motor_Tbox.Text = Right_Motor_Slider.Value.ToString();
                rootPage.Motor_Power_Command(1, Right_Motor_Value);
            }
        }

        private void Plus_Both_Motors_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Both_Motors_Slider.Value <= 90 )
            {
                Both_Motors_Slider.Value = Both_Motors_Slider.Value + 10;
                Left_Motor_Slider.Value  = Both_Motors_Slider.Value;
                Right_Motor_Slider.Value = Both_Motors_Slider.Value;
                Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                Left_Motor_Tbox.Text  = Both_Motors_Slider.Value.ToString();
                Right_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();

                short Both_Motors_Value = (short)Both_Motors_Slider.Value;
                rootPage.Motor_Power_Command(2, Both_Motors_Value);
            }
        }

        private void Minus_Left_Motor_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Left_Motor_Slider.Value >= -90)
            {
                Left_Motor_Slider.Value = Left_Motor_Slider.Value - 10;
                short Left_Motor_Value = (short)Left_Motor_Slider.Value;
                Left_Motor_Tbox.Text = Left_Motor_Slider.Value.ToString();
                rootPage.Motor_Power_Command(0, Left_Motor_Value);
            }
        }

        private void Minus_Right_Motor_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Right_Motor_Slider.Value >= -90)
            {
                Right_Motor_Slider.Value = Right_Motor_Slider.Value - 10;
                short Right_Motor_Value = (short)Right_Motor_Slider.Value;
                Right_Motor_Tbox.Text = Right_Motor_Slider.Value.ToString();
                rootPage.Motor_Power_Command(1, Right_Motor_Value);
            }
        }

        private void Minus_Both_Motors_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Both_Motors_Slider.Value >= -90)
            {
                Both_Motors_Slider.Value = Both_Motors_Slider.Value - 10;
                Left_Motor_Slider.Value  = Both_Motors_Slider.Value;
                Right_Motor_Slider.Value = Both_Motors_Slider.Value;
                Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                Left_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
                Right_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();

                short Both_Motors_Value = (short)Both_Motors_Slider.Value;
                rootPage.Motor_Power_Command(2, Both_Motors_Value);
            }
        }
    }
}
