using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using InTheHand.Net.Sockets;

namespace WalkAway.BT
{
    /// <summary>
    /// The secure device service.
    /// </summary>
    public class SecureDeviceService
    {
        /// <summary>
        /// Discovers all devices that are in range and authenticated.
        /// </summary>
        /// <returns>Returns collection of devices addresses that can be used to scan for in future.</returns>
        public IEnumerable<SecureDeviceInfo> DiscoverAllDevices()
        {
            using (var client = new BluetoothClient())
            {
                var availableDevices = client.DiscoverDevices(int.MaxValue, true, false, false, false);
                return availableDevices.Select(device =>
                    new SecureDeviceInfo(device.DeviceName, device.DeviceAddress.ToString()));
            }
        }

        /// <summary>
        /// Scans for the secure devices in range.
        /// </summary>
        /// <param name="maxDevices">The maximum number of devices to scan for.</param>
        /// <param name="interestingDevicesAddresses">The interesting devices addresses.</param>
        /// <returns>
        /// Returns the collection of secure device connection results with in-range states. 
        /// If scanning fails for some reason, all interesting devices will be marked as out of range.
        /// </returns>
        /// <exception cref="ArgumentNullException">maxDevices must be greater than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">interestingDevicesAddresses</exception>
        /// <remarks>
        /// Lower maximum number of devices to scan for will take less to scan.
        /// To scan for all devices in range, set value to <see cref="int.MaxValue"/>.
        /// </remarks>
        public IEnumerable<SecureDeviceConnectionResult> TryScan(
            int maxDevices,
            IEnumerable<string> interestingDevicesAddresses)
        {
            if (interestingDevicesAddresses == null)
                throw new ArgumentNullException(nameof(interestingDevicesAddresses));
            if (maxDevices <= 0) throw new ArgumentOutOfRangeException(nameof(maxDevices));

            // Enumerate to list
            var devicesAddresses = interestingDevicesAddresses as string[] ?? interestingDevicesAddresses.ToArray();

            // Try to scan for the devices
            try
            {
                return Scan(maxDevices, devicesAddresses);
            }
            catch (Exception)
            {
                // TODO Log

                // Returns collection of provided devices addresses with all devices marked out of range
                return devicesAddresses.Select(deviceAddress => new SecureDeviceConnectionResult(deviceAddress));
            }
        }

        /// <summary>
        /// Scans the specified maximum devices.
        /// </summary>
        /// <param name="maxDevices">The maximum devices.</param>
        /// <param name="interestingDevicesAddresses">The interesting devices addresses.</param>
        /// <returns>Returns the collection of secure device connection results with in-range states.</returns>
        /// <exception cref="ArgumentNullException">maxDevices must be greater than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">interestingDevicesAddresses</exception>
        /// <remarks>
        /// Lower maximum number of devices to scan for will take less to scan.
        /// To scan for all devices in range, set value to <see cref="int.MaxValue"/>.
        /// </remarks>
        private IEnumerable<SecureDeviceConnectionResult> Scan(int maxDevices, IEnumerable<string> interestingDevicesAddresses)
        {
            if (interestingDevicesAddresses == null)
                throw new ArgumentNullException(nameof(interestingDevicesAddresses));
            if (maxDevices <= 0) throw new ArgumentOutOfRangeException(nameof(maxDevices));

            // Map interesting devices addresses to secure device connection container
            var scannedDevices = interestingDevicesAddresses
                .ToDictionary(
                    deviceAddress => deviceAddress,
                    deviceAddress => new SecureDeviceConnectionResult(deviceAddress));

            // Process all available devices
            using (var client = new BluetoothClient())
            {
                var availableDevices = client.DiscoverDevices(maxDevices, true, false, false, false);
                foreach (var bluetoothDeviceInfo in availableDevices)
                {
                    var deviceAddressString = bluetoothDeviceInfo.DeviceAddress.ToString();

                    // Check if device is interesting to us
                    if (!scannedDevices.ContainsKey(deviceAddressString))
                        continue;

                    // If already connected, then device is in range
                    if (bluetoothDeviceInfo.Connected)
                    {
                        scannedDevices[deviceAddressString].IsInRange = true;
                        continue;
                    }

                    // Try to connect if not connected already
                    using (var deviceClient = new BluetoothClient())
                    {
                        try
                        {
                            // Connect to first available installed service
                            deviceClient.Connect(
                                bluetoothDeviceInfo.DeviceAddress,
                                bluetoothDeviceInfo.InstalledServices.First());

                            // Mark device as in range
                            scannedDevices[deviceAddressString].IsInRange = true;
                        }
                        catch (SocketException)
                        {
                            // TODO Log
                        }
                        catch (Exception)
                        {
                            // TODO Log
                        }
                    }
                }
            }

            return scannedDevices.Values.ToList();
        }
    }
}
