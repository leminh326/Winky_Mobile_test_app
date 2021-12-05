using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Winky_Mobile_Test_Prototype_1
{
    public partial class MainPage : Page
    {
        public BluetoothLEDevice bluetoothLeDevice;
        public string SelectedBleDeviceId;
        public string SelectedBleDeviceName = "No device selected";
        public string Winky_Name_Set;
        public bool Is_Winky_Connected = false;

        // Test_info
        public String Tester_Name = "Unknown";
        public String Firmware_Ver = "Waiting for Test 1";
        public String App_Ver = "2.0.15";
        public String Date = "Unkown";
        public String Winky_ID = "Waiting for Test 1";
        public String Winky_Name = "Unknown";

        public String Status_Test_1 = "Unknown";
        public String Status_Test_2 = "Unknown";
        public String Status_Test_3 = "Unknown";
        public String Status_Test_4 = "Unknown";
        public String Status_Test_5 = "Unknown";
        public String Status_Test_6 = "Unknown";
        public String Status_Test_7 = "Unknown";
        public String Status_Test_8 = "Unknown";
        public String Status_Test_9 = "Unknown";
        public String Status_Test_10 = "Unknown";
        public String Status_Test_11 = "Unknown";
        public String Status_Test_12 = "Unknown";
        public String Status_Test_13 = "Unknown";
        public String Status_Test_14 = "Unknown";

        // Service 1
        public GattCharacteristic Serv1_Char1;
        public GattCharacteristic Serv1_Char2;
        public GattCharacteristic Serv1_Char3;

        // Service 2
        public GattCharacteristic Serv2_Char1;
        public GattCharacteristic Serv2_Char2;
        public GattCharacteristic Serv2_Char3;

        // Service 3
        public GattCharacteristic Serv3_Char1;
        public GattCharacteristic Serv3_Char2;

        public short motor_power_offset = 0;
        public readonly int E_BLUETOOTH_ATT_WRITE_NOT_PERMITTED = unchecked((int)0x80650003);
        public readonly int E_BLUETOOTH_ATT_INVALID_PDU = unchecked((int)0x80650004);
        public readonly int E_ACCESSDENIED = unchecked((int)0x80070005);
        public readonly int E_DEVICE_NOT_AVAILABLE = unchecked((int)0x800710df); // HRESULT_FROM_WIN32(ERROR_DEVICE_NOT_AVAILABLE)

        public byte FrameId = 0x01;
    }

}
