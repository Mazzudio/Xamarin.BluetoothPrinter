using System.Collections.Generic;
using BluetoothPrinter.Models;

namespace BluetoothPrinter
{
    public interface IBluetoothPrinterService
    {
        DeviceInfo GetCurrentDevice();

        List<DeviceInfo> GetAvailableDevices();

        bool SetCurrentDevice(string printerName);

        void PrintText(string content);

        void PrintQR(string content);
    }
}
