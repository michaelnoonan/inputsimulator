using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsInput
{
    /// <summary>
    /// Provides a useful wrapper around the User32 SendInput and related native Windows functions.
    /// </summary>
    public class InputSimulator : IInputSimulator
    {
        /// <summary>
        /// The instance of the <see cref="IInputMessageDispatcher"/> to use for dispatching <see cref="INPUT"/> messages.
        /// </summary>
        private readonly IInputMessageDispatcher _messageDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputSimulator"/> class using the specified <see cref="IInputMessageDispatcher"/> for dispatching <see cref="INPUT"/> messages.
        /// </summary>
        /// <param name="messageDispatcher">The <see cref="IInputMessageDispatcher"/> to use for dispatching <see cref="INPUT"/> messages.</param>
        /// <exception cref="InvalidOperationException">If null is passed as the <paramref name="messageDispatcher"/>.</exception>
        public InputSimulator(IInputMessageDispatcher messageDispatcher)
        {
            if (messageDispatcher == null)
                throw new InvalidOperationException(
                    string.Format("The {0} cannot operate with a null {1}. Please provide a valid {1} instance to use for dispatching {2} messages.",
                    typeof(InputSimulator).Name, typeof(IInputMessageDispatcher).Name, typeof(INPUT).Name));
            
            _messageDispatcher = messageDispatcher;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputSimulator"/> class using an instance of a <see cref="WindowsInputMessageDispatcher"/> for dispatching <see cref="INPUT"/> messages.
        /// </summary>
        public InputSimulator()
        {
            _messageDispatcher = new WindowsInputMessageDispatcher();
        }

        /// <summary>
        /// Calls the Win32 SendInput method to simulate a Key DOWN.
        /// </summary>
        /// <param name="keyCode">The VirtualKeyCode to press</param>
        public void SimulateKeyDown(VirtualKeyCode keyCode)
        {
            var inputList = new InputBuilder().AddKeyDown(keyCode).ToArray();
            
            SendSimulatedInput(inputList);
        }

        /// <summary>
        /// Calls the Win32 SendInput method to simulate a Key UP.
        /// </summary>
        /// <param name="keyCode">The VirtualKeyCode to lift up</param>
        public void SimulateKeyUp(VirtualKeyCode keyCode)
        {
            var inputList = new InputBuilder().AddKeyUp(keyCode).ToArray();
            SendSimulatedInput(inputList);
        }

        private int SendSimulatedInput(INPUT[] inputList)
        {
            if (inputList == null || inputList.Length == 0) return -1;
            return (int)_messageDispatcher.DispatchInput(inputList);
        }

        /// <summary>
        /// Calls the Win32 SendInput method with a KeyDown and KeyUp message in the same input sequence in order to simulate a Key PRESS.
        /// </summary>
        /// <param name="keyCode">The VirtualKeyCode to press</param>
        public void SimulateKeyPress(VirtualKeyCode keyCode)
        {
            var inputList =
                new InputBuilder()
                    .AddKeyDown(keyCode)
                    .AddKeyUp(keyCode)
                    .ToArray();

            SendSimulatedInput(inputList);
        }

        /// <summary>
        /// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted text entry via the keyboard.
        /// </summary>
        /// <param name="text">The text to be simulated.</param>
        public void SimulateTextEntry(string text)
        {
            if (text.Length > UInt32.MaxValue / 2) throw new ArgumentException(string.Format("The text parameter is too long. It must be less than {0} characters.", UInt32.MaxValue / 2), "text");
            var inputList = new InputBuilder().AddCharacters(text).ToArray();
            SendSimulatedInput(inputList);

            //var chars = Encoding.ASCII.GetBytes(text);
            //var len = chars.Length;
            //var inputList = new INPUT[len * 2];
            //for (var x = 0; x < len; x++)
            //{
            //    UInt16 scanCode = chars[x];

            //    var down = new INPUT();
            //    down.Type = (UInt32)InputType.Keyboard;
            //    down.Data.Keyboard = new KEYBDINPUT();
            //    down.Data.Keyboard.KeyCode = 0;
            //    down.Data.Keyboard.Scan = scanCode;
            //    down.Data.Keyboard.Flags = (UInt32)KeyboardFlag.UNICODE;
            //    down.Data.Keyboard.Time = 0;
            //    down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            //    var up = new INPUT();
            //    up.Type = (UInt32)InputType.Keyboard;
            //    up.Data.Keyboard = new KEYBDINPUT();
            //    up.Data.Keyboard.KeyCode = 0;
            //    up.Data.Keyboard.Scan = scanCode;
            //    up.Data.Keyboard.Flags = (UInt32)(KeyboardFlag.KEYUP | KeyboardFlag.UNICODE);
            //    up.Data.Keyboard.Time = 0;
            //    up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            //    // Handle extended keys:
            //    // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
            //    // we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
            //    if ((scanCode & 0xFF00) == 0xE000)
            //    {
            //        down.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.EXTENDEDKEY;
            //        up.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.EXTENDEDKEY;
            //    }

            //    inputList[2*x] = down;
            //    inputList[2*x + 1] = up;

            //}

            //SendSimulatedInput(inputList);
        }

        /// <summary>
        /// Performs a simple modified keystroke like CTRL-C where CTRL is the modifierKey and C is the key.
        /// The flow is Modifier KEYDOWN, Key PRESS, Modifier KEYUP.
        /// </summary>
        /// <param name="modifierKeyCode">The modifier key</param>
        /// <param name="keyCode">The key to simulate</param>
        public void SimulateModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            var inputList =
                new InputBuilder()
                    .AddKeyDown(modifierKeyCode)
                    .AddKeyPress(keyCode)
                    .AddKeyUp(modifierKeyCode)
                    .ToArray();

            SendSimulatedInput(inputList);
        }

        /// <summary>
        /// Performs a modified keystroke where there are multiple modifiers and one key like CTRL-ALT-C where CTRL and ALT are the modifierKeys and C is the key.
        /// The flow is Modifiers KEYDOWN in order, Key PRESS, Modifiers KEYUP in reverse order.
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param>
        /// <param name="keyCode">The key to simulate</param>
        public void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            var builder = new InputBuilder();
            if (modifierKeyCodes != null) modifierKeyCodes.ToList().ForEach(x => builder.AddKeyDown(x));
            builder.AddKeyPress(keyCode);
            if (modifierKeyCodes != null) modifierKeyCodes.Reverse().ToList().ForEach(x => builder.AddKeyUp(x));

            SendSimulatedInput(builder.ToArray());
        }

        /// <summary>
        /// Performs a modified keystroke where there is one modifier and multiple keys like CTRL-K-C where CTRL is the modifierKey and K and C are the keys.
        /// The flow is Modifier KEYDOWN, Keys PRESS in order, Modifier KEYUP.
        /// </summary>
        /// <param name="modifierKey">The modifier key</param>
        /// <param name="keyCodes">The list of keys to simulate</param>
        public void SimulateModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
        {
            var builder = new InputBuilder();
            builder.AddKeyDown(modifierKey);
            if (keyCodes != null) keyCodes.ToList().ForEach(x => builder.AddKeyPress(x));
            builder.AddKeyUp(modifierKey);

            SendSimulatedInput(builder.ToArray());
        }

        /// <summary>
        /// Performs a modified keystroke where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL and ALT are the modifierKeys and K and C are the keys.
        /// The flow is Modifiers KEYDOWN in order, Keys PRESS in order, Modifiers KEYUP in reverse order.
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param>
        /// <param name="keyCodes">The list of keys to simulate</param>
        public void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
        {
            var builder = new InputBuilder();
            if (modifierKeyCodes != null) modifierKeyCodes.ToList().ForEach(x => builder.AddKeyUp(x));
            if (keyCodes != null) keyCodes.ToList().ForEach(x => builder.AddKeyPress(x));
            if (modifierKeyCodes != null) modifierKeyCodes.Reverse().ToList().ForEach(x => builder.AddKeyUp(x));

            SendSimulatedInput(builder.ToArray());
        }
    }
}