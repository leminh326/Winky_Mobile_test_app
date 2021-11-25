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

using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace Winky_Mobile_Test_Prototype_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Welcome_Page : Page
    {
        private MainPage rootPage = MainPage.Current;
        private ObservableCollection<BluetoothLEDeviceDisplay> KnownDevices = new ObservableCollection<BluetoothLEDeviceDisplay>();
        private List<DeviceInformation> UnknownDevices = new List<DeviceInformation>();
        private DeviceWatcher deviceWatcher;
        private bool subscribedForNotifications = false;
        private GattCharacteristic registeredCharacteristic = null;
        private GattCharacteristic selectedCharacteristic = null;

        #region Error Codes
        readonly int E_BLUETOOTH_ATT_WRITE_NOT_PERMITTED = unchecked((int)0x80650003);
        readonly int E_BLUETOOTH_ATT_INVALID_PDU = unchecked((int)0x80650004);
        readonly int E_ACCESSDENIED = unchecked((int)0x80070005);
        readonly int E_DEVICE_NOT_AVAILABLE = unchecked((int)0x800710df); // HRESULT_FROM_WIN32(ERROR_DEVICE_NOT_AVAILABLE)
        #endregion

        public Welcome_Page()
        {
            this.InitializeComponent();
        }

        private async void Scan_Btn_Click(object sender, RoutedEventArgs e)
        {
            Progress_Bar.Visibility = Visibility.Visible;
            ProgressBar_Text.Visibility = Visibility.Visible;
            ProgressBar_Text.Text = "Scanning";

            if ((deviceWatcher == null) && (Scan_Btn.IsChecked == true))
            {
                StartBleDeviceWatcher();
                Scan_Btn.Content = "Stop scan";
            }
            else if (Scan_Btn.IsChecked != true)
            {
                StopBleDeviceWatcher();
                Scan_Btn.Content = "Scan";
            }

            Progress_Bar.Value = 0;
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(1000);
                Progress_Bar.Value = Progress_Bar.Value + 20;
                ProgressBar_Text.Text = Progress_Bar.Value.ToString() + "%";
            }
            Progress_Bar.Visibility = Visibility.Collapsed;
            ProgressBar_Text.Visibility = Visibility.Collapsed;
            Select_CBox.IsEnabled = true;
            Scan_Btn.IsEnabled = false;
            rootPage.Is_Winky_Connected = true;
            Scan_Btn.Content = "Scan";
        }

        private void Select_CBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Connect_Btn.IsEnabled = true;
        }

        private void StartBleDeviceWatcher()
        {
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable" };
            string aqsAllBluetoothLEDevices = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";
            deviceWatcher = DeviceInformation.CreateWatcher(aqsAllBluetoothLEDevices, requestedProperties, DeviceInformationKind.AssociationEndpoint);

            // Register event handlers before starting the watcher.
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Removed += DeviceWatcher_Removed;
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            deviceWatcher.Stopped += DeviceWatcher_Stopped;

            // Start over with an empty collection.
            KnownDevices.Clear();

            // Start the watcher. Active enumeration is limited to approximately 30 seconds.
            deviceWatcher.Start();
        }
        private void StopBleDeviceWatcher()
        {
            if (deviceWatcher != null)
            {
                // Unregister the event handlers.
                deviceWatcher.Added -= DeviceWatcher_Added;
                deviceWatcher.Updated -= DeviceWatcher_Updated;
                deviceWatcher.Removed -= DeviceWatcher_Removed;
                deviceWatcher.EnumerationCompleted -= DeviceWatcher_EnumerationCompleted;
                deviceWatcher.Stopped -= DeviceWatcher_Stopped;

                // Stop the watcher.
                deviceWatcher.Stop();
                deviceWatcher = null;
            }
        }

        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lock (this)
                {
                    Debug.WriteLine(String.Format("Added {0}{1}", deviceInfo.Id, deviceInfo.Name));

                    // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                    if (sender == deviceWatcher)
                    {
                        // Make sure device isn't already present in the list.
                        if (FindBluetoothLEDeviceDisplay(deviceInfo.Id) == null)
                        {
                            if (deviceInfo.Name != string.Empty)
                            {
                                // If device has a friendly name display it immediately.
                                KnownDevices.Add(new BluetoothLEDeviceDisplay(deviceInfo));
                            }
                            else
                            {
                                // Add it to a list in case the name gets updated later. 
                                UnknownDevices.Add(deviceInfo);
                            }
                        }

                    }
                }
            });
        }
        private async void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lock (this)
                {
                    Debug.WriteLine(String.Format("Updated {0}{1}", deviceInfoUpdate.Id, ""));

                    // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                    if (sender == deviceWatcher)
                    {
                        BluetoothLEDeviceDisplay bleDeviceDisplay = FindBluetoothLEDeviceDisplay(deviceInfoUpdate.Id);
                        if (bleDeviceDisplay != null)
                        {
                            // Device is already being displayed - update UX.
                            bleDeviceDisplay.Update(deviceInfoUpdate);
                            return;
                        }

                        DeviceInformation deviceInfo = FindUnknownDevices(deviceInfoUpdate.Id);
                        if (deviceInfo != null)
                        {
                            deviceInfo.Update(deviceInfoUpdate);
                            // If device has been updated with a friendly name it's no longer unknown.
                            if (deviceInfo.Name != String.Empty)
                            {
                                KnownDevices.Add(new BluetoothLEDeviceDisplay(deviceInfo));
                                UnknownDevices.Remove(deviceInfo);
                            }
                        }
                    }
                }
            });
        }
        private async void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lock (this)
                {
                    Debug.WriteLine(String.Format("Removed {0}{1}", deviceInfoUpdate.Id, ""));

                    // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                    if (sender == deviceWatcher)
                    {
                        // Find the corresponding DeviceInformation in the collection and remove it.
                        BluetoothLEDeviceDisplay bleDeviceDisplay = FindBluetoothLEDeviceDisplay(deviceInfoUpdate.Id);
                        if (bleDeviceDisplay != null)
                        {
                            KnownDevices.Remove(bleDeviceDisplay);
                        }

                        DeviceInformation deviceInfo = FindUnknownDevices(deviceInfoUpdate.Id);
                        if (deviceInfo != null)
                        {
                            UnknownDevices.Remove(deviceInfo);
                        }
                    }
                }
            });
        }
        private async void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object e)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                if (sender == deviceWatcher)
                {
                    Debug.WriteLine($"{KnownDevices.Count} devices found. Enumeration completed."); ;
                }
            });
        }
        private async void DeviceWatcher_Stopped(DeviceWatcher sender, object e)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                if (sender == deviceWatcher)
                {
                    // "No longer watching for devices."
                }
            });
        }
        private BluetoothLEDeviceDisplay FindBluetoothLEDeviceDisplay(string id)
        {
            foreach (BluetoothLEDeviceDisplay bleDeviceDisplay in KnownDevices)
            {
                if (bleDeviceDisplay.Id == id)
                {
                    return bleDeviceDisplay;
                }
            }
            return null;
        }
        private DeviceInformation FindUnknownDevices(string id)
        {
            foreach (DeviceInformation bleDeviceInfo in UnknownDevices)
            {
                if (bleDeviceInfo.Id == id)
                {
                    return bleDeviceInfo;
                }
            }
            return null;
        }

        private async void Connect_Device()
        {
            // Clear Bluetooth Device //
            if (!await ClearBluetoothLEDeviceAsync())
            {
                Debug.WriteLine("Error: Unable to reset state, try again."); 
                return;
            }

            // Try to connect //
            try
            {
                rootPage.bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(rootPage.SelectedBleDeviceId); // Connect

                if (rootPage.bluetoothLeDevice == null)
                {
                    Debug.WriteLine("Failed to connect to device."); 
                }
            }
            catch (Exception ex) when (ex.HResult == E_DEVICE_NOT_AVAILABLE)
            {
                Debug.WriteLine("Bluetooth radio is not on."); 
            }

            // Try to find Service //
            if (rootPage.bluetoothLeDevice != null)
            {
                GattDeviceServicesResult result = await rootPage.bluetoothLeDevice.GetGattServicesAsync(BluetoothCacheMode.Uncached);

                if (result.Status == GattCommunicationStatus.Success)
                {
                    string service_name;
                    var services = result.Services;
                    Debug.WriteLine(String.Format("Found {0} services :", services.Count));

                    // Try to find characteristics
                    foreach (var service in services)
                    {
                        service_name = DisplayHelpers.GetServiceName(service);
                        Debug.WriteLine(service_name);
                        switch (service_name)
                        {                            
                            case "Custom Service: 00000100-cc7a-482a-984a-7f2ed5b3e58f": // Service 1
                                Debug.WriteLine("Service 1");
                                GattCharacteristicsResult result2 = await service.GetCharacteristicsAsync();
                                var characteristics = result2.Characteristics;
                                foreach (GattCharacteristic c in characteristics)
                                {
                                    string Characteristic_Name = DisplayHelpers.GetCharacteristicName(c);
                                    switch (Characteristic_Name)
                                    {                                       
                                        case "Custom Characteristic: 00000101-8e22-4541-9d4c-21edae82ed19":
                                            Debug.WriteLine("Serv 1 - Char 1");
                                            rootPage.Serv1_Char1 = c;
                                            break;
                                        case "Custom Characteristic: 00000102-8e22-4541-9d4c-21edae82ed19":
                                            Debug.WriteLine("Serv 1 - Char 2");
                                            rootPage.Serv1_Char2 = c;
                                            break;
                                        case "Custom Characteristic: 00000103-8e22-4541-9d4c-21edae82ed19":
                                            Debug.WriteLine("Serv 1 - Char 3");
                                            rootPage.Serv1_Char3 = c;
                                            break;
                                        default:
                                            Debug.WriteLine("Error, Char unknown");
                                            break;
                                    }
                                }
                                break;
                            case "Custom Service: 00000200-cc7a-482a-984a-7f2ed5b3e58f": // Service 2
                                Debug.WriteLine("Service 2");
                                GattCharacteristicsResult result3 = await service.GetCharacteristicsAsync();
                                var characteristics3 = result3.Characteristics;
                                foreach (GattCharacteristic c in characteristics3)
                                {
                                    string Characteristic_Name = DisplayHelpers.GetCharacteristicName(c);
                                    switch (Characteristic_Name)
                                    {
                                        case "Custom Characteristic: 00000201-8e22-4541-9d4c-21edae82ed19":
                                            Debug.WriteLine("Serv 2 - Char 1");
                                            rootPage.Serv2_Char1 = c;
                                            break;
                                        case "Custom Characteristic: 00000202-8e22-4541-9d4c-21edae82ed19":
                                            Debug.WriteLine("Serv 2 - Char 2");
                                            rootPage.Serv2_Char2 = c;
                                            break;
                                        case "Custom Characteristic: 00000203-8e22-4541-9d4c-21edae82ed19":
                                            Debug.WriteLine("Serv 2 - Char 3");
                                            rootPage.Serv2_Char3 = c;
                                            break;
                                        default:
                                            Debug.WriteLine("Error, Char unknown");
                                            break;
                                    }
                                }
                                break;
                            case "Custom Service: 00000300-cc7a-482a-984a-7f2ed5b3e58f": // Service 3
                                Debug.WriteLine("Service 3");
                                GattCharacteristicsResult result4 = await service.GetCharacteristicsAsync();
                                var characteristics4 = result4.Characteristics;
                                foreach (GattCharacteristic c in characteristics4)
                                {
                                    string Characteristic_Name = DisplayHelpers.GetCharacteristicName(c);
                                    switch (Characteristic_Name)
                                    {
                                        case "Custom Characteristic: 00000301-8e22-4541-9d4c-21edae82ed19":
                                            Debug.WriteLine("Serv 3 - Char 1");
                                            rootPage.Serv3_Char1 = c;
                                            break;
                                        case "Custom Characteristic: 00000302-8e22-4541-9d4c-21edae82ed19":
                                            Debug.WriteLine("Serv 3 - Char 2");
                                            rootPage.Serv3_Char2 = c;
                                            break;
                                        default:
                                            Debug.WriteLine("Error, Char unknown");
                                            break;
                                    }
                                }
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }

                    }
                }
                else
                {
                    Debug.WriteLine("Device unreachable"); 
                }
            }
        }

        private async Task<bool> ClearBluetoothLEDeviceAsync()
        {
            if (subscribedForNotifications)
            {
                // Need to clear the CCCD from the remote device so we stop receiving notifications
                var result = await registeredCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.None);
                if (result != GattCommunicationStatus.Success)
                {
                    return false;
                }
                else
                {
                    selectedCharacteristic.ValueChanged -= Characteristic_ValueChanged;
                    subscribedForNotifications = false;
                }
            }


            if (rootPage.bluetoothLeDevice != null)
            {
                GattDeviceServicesResult result = await rootPage.bluetoothLeDevice.GetGattServicesAsync(BluetoothCacheMode.Uncached);

                if (result.Status == GattCommunicationStatus.Success)
                {
                    var services = result.Services;
                    Debug.WriteLine(String.Format("Delete {0} services :", services.Count));

                    // Try to find characteristics
                    foreach (var service in services)
                    {
                        service.Dispose();
                    }
                }

                rootPage.bluetoothLeDevice.Dispose(); // disconnect
                rootPage.bluetoothLeDevice = null;
                GC.Collect();
            }

            return true;
        }
        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args) { }

        private async void Connect_Btn_Click(object sender, RoutedEventArgs e)
        {
            StopBleDeviceWatcher();

            // Save the selected device's ID for use in other scenarios.
            var bleDeviceDisplay = Select_CBox.SelectedItem as BluetoothLEDeviceDisplay;
            if (bleDeviceDisplay != null)
            {
                rootPage.SelectedBleDeviceId = bleDeviceDisplay.Id;
                rootPage.SelectedBleDeviceName = bleDeviceDisplay.Name;
                rootPage.Winky_Name_Set = bleDeviceDisplay.Name;
            }
            Connect_Device();

            Progress_Bar.Visibility = Visibility.Visible;
            Progress_Bar.Value = 0;
            ProgressBar_Text.Visibility = Visibility.Visible;
            ProgressBar_Text.Text = "Connecting";
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(1400);
                Progress_Bar.Value = Progress_Bar.Value + 20;
                ProgressBar_Text.Text = Progress_Bar.Value.ToString() + "%";
            }
            Progress_Bar.Visibility = Visibility.Collapsed;
            ProgressBar_Text.Visibility = Visibility.Collapsed;
            Connect_Btn.IsEnabled = false;
            Begin_Test_Btn.IsEnabled = true;
        }

        private void Begin_Test_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Control_Panel_Page));
        }
        /*
private async void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
{           
// BT_Code: An Indicate or Notify reported that the value has changed.
// Display the new value with a timestamp.
var newValue = FormatValueByPresentation(args.CharacteristicValue, presentationFormat);
var message = $"Value at {DateTime.Now:hh:mm:ss.FFF}: {newValue}";
await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
() => CharacteristicLatestValue.Text = message);          
}
*/

    }
}
