using System.Runtime.InteropServices;

namespace WalkAway.Native
{
    /// <summary>
    /// User32.dll native calls.
    /// </summary>
    public static class User32Native
    {
        /// <summary>
        /// Locks the workstation.
        /// </summary>
        /// <returns>Returns <c>True</c> if workstation was locked successfully; <c>False</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();
    }
}
