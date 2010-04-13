using NUnit.Framework;
using WindowsInput;
using System.Threading;

namespace WindowsInput.Tests
{
    [TestFixture]
    public class InputSimulatorExamples
    {
        [Test]
        public void OpenWindowsExplorer()
        {
            var sim = new InputSimulator();
            sim.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_E);
        }

        [Test]
        public void SayHello()
        {
            var sim = new InputSimulator();
            sim.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R);
            sim.SimulateTextEntry("notepad");
            Thread.Sleep(1000);
            sim.SimulateKeyPress(VirtualKeyCode.RETURN);
            Thread.Sleep(1000);
            sim.SimulateTextEntry("These are your orders if you choose to accept them...");
            sim.SimulateTextEntry("This message will self destruct in 5 seconds.");
            Thread.Sleep(5000);
            sim.SimulateModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE);
            sim.SimulateKeyPress(VirtualKeyCode.DOWN);
            sim.SimulateKeyPress(VirtualKeyCode.RETURN);
            var i = 50;
            while (i-- > 0) sim.SimulateKeyPress(VirtualKeyCode.DOWN);
            sim.SimulateKeyPress(VirtualKeyCode.RETURN);
            Thread.Sleep(1000);
            sim.SimulateModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4);
            sim.SimulateKeyPress(VirtualKeyCode.VK_N);
        }
    }
}