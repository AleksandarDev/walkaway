using System.Linq;
using System.Windows;
using WalkAway.BT;
using WalkAway.Native;

namespace WalkAway
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var service = new SecureDeviceService();

            // Retrieve all possibly interesting devices
            var availableDevices = service.DiscoverAllDevices().ToList();

            // Check if interesting devices are in range
            var result = service.TryScan(availableDevices.Count, availableDevices.Select(device => device.DeviceAddress));

            // Lock workstation if not all interesting devices are in range
            if (!result.All(device => device.IsInRange))
                User32Native.LockWorkStation();
        }
    }
}
