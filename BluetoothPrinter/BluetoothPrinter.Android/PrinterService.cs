using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using BluetoothPrinter.Droid;
using BluetoothPrinter.Models;

[assembly: Xamarin.Forms.Dependency(typeof(PrinterService))]
namespace BluetoothPrinter.Droid
{
    public class PrinterService : IBluetoothPrinterService
    {
        private BluetoothDevice _connectedDevice;
        public PrinterService()
        {
        }

        public List<DeviceInfo> GetAvailableDevices()
        {
            if (BluetoothAdapter.DefaultAdapter != null && BluetoothAdapter.DefaultAdapter.IsEnabled)
            {
                List<DeviceInfo> result = new List<DeviceInfo>();
                foreach (var pairedDevice in BluetoothAdapter.DefaultAdapter.BondedDevices)
                {
                    result.Add(new DeviceInfo
                    {
                        Title = pairedDevice.Name,
                        MacAddress =pairedDevice.Address
                    }); 
                }
                return result;
            }
            return null;
        }

        public DeviceInfo GetCurrentDevice()
        {
            if(_connectedDevice != null)
            {
                return new DeviceInfo
                {
                    Title = _connectedDevice.Name,
                    MacAddress = _connectedDevice.Address
                };
            }
            return null;
        }

        public bool SetCurrentDevice(string printerName)
        {
            if (BluetoothAdapter.DefaultAdapter != null && BluetoothAdapter.DefaultAdapter.IsEnabled)
            {
                foreach (var pairedDevice in BluetoothAdapter.DefaultAdapter.BondedDevices)
                {
                    if (pairedDevice.Name == printerName)
                    {
                        _connectedDevice = pairedDevice;
                        return true;
                    }  
                }
            }
            return false;
        }

        public void PrintQR(string content)
        {
            SendCommandToPrinter("qr", content, _connectedDevice);
        }

        public void PrintText(string content)
        {
            SendCommandToPrinter("plain", content, _connectedDevice);
        }

        async void SendCommandToPrinter(string type, string content, BluetoothDevice device)
        {
            if (string.IsNullOrEmpty(content)) return;
            Printer print = new Printer();
            if (device != null)
            {
                await print.Print(type, content, device);
            }
            else
            {
                throw new Exception("No selected device.");
            }
        }
    }
}