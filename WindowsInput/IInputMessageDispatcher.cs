using System;
using WindowsInput.Native;

namespace WindowsInput
{
    /// <summary>
    /// The contract for a service that dispatches <see cref="INPUT"/> messages to the appropriate destination.
    /// </summary>
    internal interface IInputMessageDispatcher
    {
        /// <summary>
        /// Dispatches the specified list of <see cref="INPUT"/> messages in their specified order.
        /// </summary>
        /// <param name="inputs">The list of <see cref="INPUT"/> messages to be dispatched.</param>
        /// <returns>The number of <see cref="INPUT"/> messages that were successfully dispatched.</returns>
        long DispatchInput(INPUT[] inputs);
    }
}
