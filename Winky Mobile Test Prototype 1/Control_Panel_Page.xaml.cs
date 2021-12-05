using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        bool auto_mode_on = false;

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
            Power_Tbox.Text       = Both_Motors_Tbox.Text;
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
                Power_Tbox.Text = Both_Motors_Tbox.Text;
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
                Power_Tbox.Text = Both_Motors_Tbox.Text;
                Left_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
                Right_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();

                short Both_Motors_Value = (short)Both_Motors_Slider.Value;
                rootPage.Motor_Power_Command(2, Both_Motors_Value);
            }
        }

        private void Stop_Btn_Click(object sender, RoutedEventArgs e)
        {
            rootPage.Motor_Power_Command(2, 0);

            Both_Motors_Slider.Value    = 0;
            Left_Motor_Slider.Value     = 0;
            Right_Motor_Slider.Value    = 0;
            Both_Motors_Tbox.Text       = Both_Motors_Slider.Value.ToString();
            Left_Motor_Tbox.Text        = Both_Motors_Slider.Value.ToString();
            Right_Motor_Tbox.Text       = Both_Motors_Slider.Value.ToString();
            Power_Slider.Value          = 0;
            Balance_Slider.Value        = 0;
            Power_Tbox.Text = Both_Motors_Tbox.Text;
            Balance_Tbox.Text = "0";
        }

        private void Full_Power_Btn_Click(object sender, RoutedEventArgs e)
        {
            rootPage.Motor_Power_Command(2, 100);

            Both_Motors_Slider.Value    = 100;
            Left_Motor_Slider.Value     = 100;
            Right_Motor_Slider.Value    = 100;
            Both_Motors_Tbox.Text       = Both_Motors_Slider.Value.ToString();
            Left_Motor_Tbox.Text        = Both_Motors_Slider.Value.ToString();
            Right_Motor_Tbox.Text       = Both_Motors_Slider.Value.ToString();
            Power_Slider.Value          = 100;
            Balance_Slider.Value        = 0;
            Power_Tbox.Text = Both_Motors_Tbox.Text;
        }

        private void Power_Slider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            double Right_Power_Value = Power_Slider.Value * ( 1 - (-Balance_Slider.Value / 100) );
            double Left_Power_Value  = Power_Slider.Value * ( 1 - ( Balance_Slider.Value / 100) );

            Both_Motors_Slider.Value = Power_Slider.Value;
            Left_Motor_Slider.Value  = Left_Power_Value;
            Right_Motor_Slider.Value = Right_Power_Value;
            Both_Motors_Tbox.Text    = Both_Motors_Slider.Value.ToString();
            Left_Motor_Tbox.Text     = Left_Motor_Slider.Value.ToString();
            Right_Motor_Tbox.Text    = Right_Motor_Slider.Value.ToString();
            Power_Tbox.Text = Both_Motors_Tbox.Text;

            if ((Left_Motor_Slider.Value <= 100) && (Left_Motor_Slider.Value >= -100))
            {
                rootPage.Motor_Power_Command(0, (short)Left_Motor_Slider.Value);
            }
            if ((Right_Motor_Slider.Value <= 100) && (Right_Motor_Slider.Value >= -100))
            {
                rootPage.Motor_Power_Command(1, (short)Right_Motor_Slider.Value);
            }
        }

        private void Balance_Slider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            double Right_Power_Value = Power_Slider.Value * (1 - (-Balance_Slider.Value / 100));
            double Left_Power_Value  = Power_Slider.Value * (1 - (Balance_Slider.Value / 100));

            Both_Motors_Slider.Value = Power_Slider.Value;
            Left_Motor_Slider.Value  = Left_Power_Value;
            Right_Motor_Slider.Value = Right_Power_Value;
            Both_Motors_Tbox.Text    = Both_Motors_Slider.Value.ToString();
            Left_Motor_Tbox.Text     = Left_Motor_Slider.Value.ToString();
            Right_Motor_Tbox.Text    = Right_Motor_Slider.Value.ToString();
            Power_Tbox.Text = Both_Motors_Tbox.Text;
            Balance_Tbox.Text = (Right_Motor_Slider.Value - Left_Motor_Slider.Value).ToString();

            if ((Left_Motor_Slider.Value <= 100) && (Left_Motor_Slider.Value >= -100))
            {
                rootPage.Motor_Power_Command(0, (short)Left_Motor_Slider.Value);
            }
            if ((Right_Motor_Slider.Value <= 100) && (Right_Motor_Slider.Value >= -100))
            {
                rootPage.Motor_Power_Command(1, (short)Right_Motor_Slider.Value);
            }
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Up:
                    Power_Slider.Value = Power_Slider.Value + 10;
                    double Right_Power_Value = Power_Slider.Value * (1 - (-Balance_Slider.Value / 100));
                    double Left_Power_Value = Power_Slider.Value * (1 - (Balance_Slider.Value / 100));

                    Both_Motors_Slider.Value = Power_Slider.Value;
                    Left_Motor_Slider.Value = Left_Power_Value;
                    Right_Motor_Slider.Value = Right_Power_Value;
                    Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Left_Motor_Tbox.Text = Left_Motor_Slider.Value.ToString();
                    Right_Motor_Tbox.Text = Right_Motor_Slider.Value.ToString();
                    Power_Tbox.Text = Both_Motors_Tbox.Text;

                    if ((Left_Motor_Slider.Value <= 100) && (Left_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(0, (short)Left_Motor_Slider.Value);
                    }
                    if ((Right_Motor_Slider.Value <= 100) && (Right_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(1, (short)Right_Motor_Slider.Value);
                    }
                    break;
                case Windows.System.VirtualKey.Down:
                    Power_Slider.Value = Power_Slider.Value - 10;
                    double Right_Power_Value4 = Power_Slider.Value * (1 - (-Balance_Slider.Value / 100));
                    double Left_Power_Value4 = Power_Slider.Value * (1 - (Balance_Slider.Value / 100));

                    Both_Motors_Slider.Value = Power_Slider.Value;
                    Left_Motor_Slider.Value = Left_Power_Value4;
                    Right_Motor_Slider.Value = Right_Power_Value4;
                    Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Left_Motor_Tbox.Text = Left_Motor_Slider.Value.ToString();
                    Right_Motor_Tbox.Text = Right_Motor_Slider.Value.ToString();
                    Power_Tbox.Text = Both_Motors_Tbox.Text;

                    if ((Left_Motor_Slider.Value <= 100) && (Left_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(0, (short)Left_Motor_Slider.Value);
                    }
                    if ((Right_Motor_Slider.Value <= 100) && (Right_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(1, (short)Right_Motor_Slider.Value);
                    }
                    break;
                case Windows.System.VirtualKey.Left:
                    Balance_Slider.Value = Balance_Slider.Value + 10;
                    double Right_Power_Value3 = Power_Slider.Value * (1 - (-Balance_Slider.Value / 100));
                    double Left_Power_Value3 = Power_Slider.Value * (1 - (Balance_Slider.Value / 100));

                    Both_Motors_Slider.Value = Power_Slider.Value;
                    Left_Motor_Slider.Value = Left_Power_Value3;
                    Right_Motor_Slider.Value = Right_Power_Value3;
                    Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Left_Motor_Tbox.Text = Left_Motor_Slider.Value.ToString();
                    Right_Motor_Tbox.Text = Right_Motor_Slider.Value.ToString();
                    Power_Tbox.Text = Both_Motors_Tbox.Text;

                    if ((Left_Motor_Slider.Value <= 100) && (Left_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(0, (short)Left_Motor_Slider.Value);
                    }
                    if ((Right_Motor_Slider.Value <= 100) && (Right_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(1, (short)Right_Motor_Slider.Value);
                    }
                    break;
                case Windows.System.VirtualKey.Right:
                    Balance_Slider.Value = Balance_Slider.Value - 10;
                    double Right_Power_Value2 = Power_Slider.Value * (1 - (-Balance_Slider.Value / 100));
                    double Left_Power_Value2 = Power_Slider.Value * (1 - (Balance_Slider.Value / 100));

                    Both_Motors_Slider.Value = Power_Slider.Value;
                    Left_Motor_Slider.Value = Left_Power_Value2;
                    Right_Motor_Slider.Value = Right_Power_Value2;
                    Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Left_Motor_Tbox.Text = Left_Motor_Slider.Value.ToString();
                    Right_Motor_Tbox.Text = Right_Motor_Slider.Value.ToString();
                    Power_Tbox.Text = Both_Motors_Tbox.Text;

                    if ((Left_Motor_Slider.Value <= 100) && (Left_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(0, (short)Left_Motor_Slider.Value);
                    }
                    if ((Right_Motor_Slider.Value <= 100) && (Right_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(1, (short)Right_Motor_Slider.Value);
                    }
                    break;
                case Windows.System.VirtualKey.B:
                    rootPage.Motor_Power_Command(2, 0);

                    Both_Motors_Slider.Value = 0;
                    Left_Motor_Slider.Value = 0;
                    Right_Motor_Slider.Value = 0;
                    Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Left_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Right_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Power_Slider.Value = 0;
                    Balance_Slider.Value = 0;
                    Power_Tbox.Text = Both_Motors_Tbox.Text;
                    break;
                case Windows.System.VirtualKey.C:
                    Balance_Slider.Value = 0;
                    double Right_Power_Value7 = Power_Slider.Value * (1 - (-Balance_Slider.Value / 100));
                    double Left_Power_Value7 = Power_Slider.Value * (1 - (Balance_Slider.Value / 100));

                    Both_Motors_Slider.Value = Power_Slider.Value;
                    Left_Motor_Slider.Value = Left_Power_Value7;
                    Right_Motor_Slider.Value = Right_Power_Value7;
                    Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Left_Motor_Tbox.Text = Left_Motor_Slider.Value.ToString();
                    Right_Motor_Tbox.Text = Right_Motor_Slider.Value.ToString();
                    Power_Tbox.Text = Both_Motors_Tbox.Text;

                    if ((Left_Motor_Slider.Value <= 100) && (Left_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(0, (short)Left_Motor_Slider.Value);
                    }
                    if ((Right_Motor_Slider.Value <= 100) && (Right_Motor_Slider.Value >= -100))
                    {
                        rootPage.Motor_Power_Command(1, (short)Right_Motor_Slider.Value);
                    }
                    break;
                case Windows.System.VirtualKey.V:
                    rootPage.Motor_Power_Command(2, 100);

                    Both_Motors_Slider.Value = 100;
                    Left_Motor_Slider.Value = 100;
                    Right_Motor_Slider.Value = 100;
                    Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Left_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Right_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Power_Slider.Value = 100;
                    Balance_Slider.Value = 100;
                    Power_Tbox.Text = Both_Motors_Tbox.Text;
                    break;
                case Windows.System.VirtualKey.N:
                    rootPage.Motor_Power_Command(2, -100);

                    Both_Motors_Slider.Value = -100;
                    Left_Motor_Slider.Value = -100;
                    Right_Motor_Slider.Value = -100;
                    Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Left_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Right_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
                    Power_Slider.Value = -100;
                    Balance_Slider.Value = -100;
                    Power_Tbox.Text = Both_Motors_Tbox.Text;
                    break;
            }
        }

        private void Full_Power_Negative_Btn_Click(object sender, RoutedEventArgs e)
        {
            rootPage.Motor_Power_Command(2, -100);

            Both_Motors_Slider.Value = -100;
            Left_Motor_Slider.Value = -100;
            Right_Motor_Slider.Value = -100;
            Both_Motors_Tbox.Text = Both_Motors_Slider.Value.ToString();
            Left_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
            Right_Motor_Tbox.Text = Both_Motors_Slider.Value.ToString();
            Power_Slider.Value = -100;
            Balance_Slider.Value = 0;
            Power_Tbox.Text = Both_Motors_Tbox.Text;
        }

        private void Offset_Tbox_LostFocus(object sender, RoutedEventArgs e)
        {
            rootPage.motor_power_offset = Convert.ToInt16(Offset_Tbox.Text);
        }
    }
}
