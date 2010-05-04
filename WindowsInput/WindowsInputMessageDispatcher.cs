using System;
using System.Linq;
using System.Runtime.InteropServices;
using WindowsInput.Native;

namespace WindowsInput
{
    /// <summary>
    /// Implements the <see cref="IInputMessageDispatcher"/> by calling <see cref="NativeMethods.SendInput"/>.
    /// </summary>
    internal class WindowsInputMessageDispatcher : IInputMessageDispatcher
    {
        /// <summary>
        /// Dispatches the specified list of <see cref="INPUT"/> messages in their specified order by issuing a single called to <see cref="NativeMethods.SendInput"/>.
        /// </summary>
        /// <param name="inputs">The list of <see cref="INPUT"/> messages to be dispatched.</param>
        /// <returns>
        /// The number of <see cref="INPUT"/> messages that were successfully dispatched.
        /// </returns>
        public UInt32 DispatchInput(INPUT[] inputs)
        {
            return NativeMethods.SendInput((UInt32)inputs.Count(), inputs.ToArray(), Marshal.SizeOf(typeof (INPUT)));
        }
    }
}