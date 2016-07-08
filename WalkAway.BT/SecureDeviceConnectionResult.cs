using System;

namespace WalkAway.BT
{
    /// <summary>
    /// The secure device connection result.
    /// </summary>
    public class SecureDeviceConnectionResult
    {
        /// <summary>
        /// Gets the device address.
        /// </summary>
        /// <value>
        /// The device address.
        /// </value>
        public string DeviceAddress { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in range.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in range; otherwise, <c>false</c>.
        /// </value>
        public bool IsInRange { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SecureDeviceConnectionResult"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <exception cref="ArgumentException">Value cannot be null or whitespace, address.</exception>
        public SecureDeviceConnectionResult(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(address));

            this.DeviceAddress = address;
        }
    }
}