using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
namespace Winky_Mobile_Test_Prototype_1
{
    public partial class MainPage : Page
    {
        public byte[] value_to_write;

        public async void Motor_Power_Command(short Motor_Selected, short Value)
        {
            var writer = new DataWriter();
            byte direction = 0;

            if      (Value > 0) { direction = 1;}
            else if (Value < 0) { direction = 2;}
            Value = Math.Abs(Value);

            byte[] frame_commade = { (byte)Value, direction };
            value_to_write = frame_commade;
          
            switch (Motor_Selected)
            {
                case 0:
                    writer.WriteBytes(value_to_write);
                    await Write_Buffer_To_Serv2_Char2_Characteristic(writer.DetachBuffer());
                    break;
                case 1:
                    writer.WriteBytes(value_to_write);
                    await Write_Buffer_To_Serv2_Char1_Characteristic(writer.DetachBuffer());
                    break;
                case 2:
                    writer.WriteBytes(value_to_write);
                    await Write_Buffer_To_Serv2_Char1_Characteristic(writer.DetachBuffer());
                    writer.WriteBytes(value_to_write);
                    await Write_Buffer_To_Serv2_Char2_Characteristic(writer.DetachBuffer());
                    break;
                default:
                    ;
                    break;
            }           
        }

        public async Task<bool> Write_Buffer_To_Serv2_Char1_Characteristic(IBuffer buffer)
        {
            try
            {
                // BT_Code: Writes the value from the buffer to the characteristic.
                var result = await Serv2_Char1.WriteValueWithResultAsync(buffer);

                if (result.Status == GattCommunicationStatus.Success)
                {
                    //rootPage.NotifyUser("Successfully wrote value to device", NotifyType.StatusMessage);
                    return true;
                }
                else
                {
                    //rootPage.NotifyUser($"Write failed: {result.Status}", NotifyType.ErrorMessage);
                    return false;
                }
            }
            catch (Exception ex) when (ex.HResult == E_BLUETOOTH_ATT_INVALID_PDU)
            {
                //rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
                return false;
            }
            catch (Exception ex) when (ex.HResult == E_BLUETOOTH_ATT_WRITE_NOT_PERMITTED || ex.HResult == E_ACCESSDENIED)
            {
                // This usually happens when a device reports that it support writing, but it actually doesn't.
                //rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
                return false;
            }
        }
        public async Task<bool> Write_Buffer_To_Serv2_Char2_Characteristic(IBuffer buffer)
        {
            try
            {
                // BT_Code: Writes the value from the buffer to the characteristic.
                var result = await Serv2_Char2.WriteValueWithResultAsync(buffer);

                if (result.Status == GattCommunicationStatus.Success)
                {
                    //rootPage.NotifyUser("Successfully wrote value to device", NotifyType.StatusMessage);
                    return true;
                }
                else
                {
                    //rootPage.NotifyUser($"Write failed: {result.Status}", NotifyType.ErrorMessage);
                    return false;
                }
            }
            catch (Exception ex) when (ex.HResult == E_BLUETOOTH_ATT_INVALID_PDU)
            {
                //rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
                return false;
            }
            catch (Exception ex) when (ex.HResult == E_BLUETOOTH_ATT_WRITE_NOT_PERMITTED || ex.HResult == E_ACCESSDENIED)
            {
                // This usually happens when a device reports that it support writing, but it actually doesn't.
                //rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
                return false;
            }
        }
        public async Task<bool> Write_Buffer_To_Serv2_Char3_Characteristic(IBuffer buffer)
        {
            try
            {
                // BT_Code: Writes the value from the buffer to the characteristic.
                var result = await Serv2_Char3.WriteValueWithResultAsync(buffer);

                if (result.Status == GattCommunicationStatus.Success)
                {
                    //rootPage.NotifyUser("Successfully wrote value to device", NotifyType.StatusMessage);
                    return true;
                }
                else
                {
                    //rootPage.NotifyUser($"Write failed: {result.Status}", NotifyType.ErrorMessage);
                    return false;
                }
            }
            catch (Exception ex) when (ex.HResult == E_BLUETOOTH_ATT_INVALID_PDU)
            {
                //rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
                return false;
            }
            catch (Exception ex) when (ex.HResult == E_BLUETOOTH_ATT_WRITE_NOT_PERMITTED || ex.HResult == E_ACCESSDENIED)
            {
                // This usually happens when a device reports that it support writing, but it actually doesn't.
                //rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
                return false;
            }
        }

    }
}
