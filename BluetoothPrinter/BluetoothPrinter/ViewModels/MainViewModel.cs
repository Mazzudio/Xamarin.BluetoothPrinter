using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms; 

namespace BluetoothPrinter.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    { 
        public string CurrentPrinter { get; set; } 

        public string InputText { get; set; } = "This is sample text for testing."; 

        public ICommand SelectPrinterCommand { get; private set; }
        public ICommand PrintTextCommand { get; private set; }
        public ICommand PrintQRCommand { get; private set; }

        public MainViewModel()
        {
            SelectPrinterCommand = new Command(async () => await ShowAvailableDevices());
            PrintTextCommand = new Command(async () => await PrintText());
            PrintQRCommand = new Command(async () => await PrintQR());
            Xamarin.Forms.DependencyService.Register<IBluetoothPrinterService>();
            var devices = DependencyService.Get<IBluetoothPrinterService>().GetAvailableDevices();
            
            if(devices != null && devices.Count > 0)
            {
                SelectDevice(devices[0].Title);
            }
        }

        async Task ShowAvailableDevices()
        {
            var devices = DependencyService.Get<IBluetoothPrinterService>().GetAvailableDevices();
            if(devices != null && devices.Count > 0)
            {
                var choices = devices.Select(d => d.Title).ToArray();
                string action = await Application.Current.MainPage.DisplayActionSheet("Select printer device.", "Cancel", null, choices);
                if (choices.Contains(action))
                {
                    SelectDevice(action);
                }
            }
        }

        void SelectDevice(string printerName)
        {
            if (DependencyService.Get<IBluetoothPrinterService>().SetCurrentDevice(printerName))
            {
                var current = DependencyService.Get<IBluetoothPrinterService>().GetCurrentDevice();
                if (current != null)
                {
                    CurrentPrinter = current.Title;
                    OnPropertyChanged("CurrentPrinter");
                }
            }
        }

        async Task PrintText()
        {
            var text = InputText;
            //Xamarin.Forms.DependencyService.Register<IBluetoothPrinterService>();
            DependencyService.Get<IBluetoothPrinterService>().PrintText(text);
        }

        async Task PrintQR()
        {
            var text = InputText; 
            //Xamarin.Forms.DependencyService.Register<IBluetoothPrinterService>();
            DependencyService.Get<IBluetoothPrinterService>().PrintQR(text);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
