using System;

namespace WalkAway.BT
{
    /// <summary>
    /// The secure device information.
    /// </summary>
    public class SecureDeviceInfo
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the device address.
        /// </summary>
        /// <value>
        /// The device address.
        /// </value>
        public string DeviceAddress { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SecureDeviceInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        /// <exception cref="ArgumentException">
        /// Value cannot be null or whitespace, name.
        /// or
        /// Value cannot be null or whitespace, address.
        /// </exception>
        public SecureDeviceInfo(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(address));

            this.Name = name;
            this.DeviceAddress = address;
        }


        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            // ReSharper disable once BaseObjectEqualsIsObjectEquals
            return base.Equals(obj);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        protected bool Equals(SecureDeviceInfo other)
        {
            return string.Equals(this.DeviceAddress, other.DeviceAddress);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.DeviceAddress?.GetHashCode() ?? 0;
        }
    }
}