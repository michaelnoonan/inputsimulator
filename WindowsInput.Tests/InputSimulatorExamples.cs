using NUnit.Framework;
using System.Threading;
using WindowsInput.Native;

namespace WindowsInput.Tests
{
    [TestFixture]
    public class InputSimulatorExamples
    {
        [Test]
        public void OpenWindowsExplorer()
        {
            var sim = new KeyboardSimulator();
            sim.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_E);
        }

        [Test]
        public void SayHello()
        {
            var sim = new KeyboardSimulator();
            sim.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R);
            Thread.Sleep(1000);
            sim.TextEntry("notepad");
            Thread.Sleep(1000);
            sim.KeyPress(VirtualKeyCode.RETURN);
            Thread.Sleep(1000);
            sim.TextEntry("These are your orders if you choose to accept them...");
            sim.TextEntry("This message will self destruct in 5 seconds.");
            Thread.Sleep(5000);
            sim.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE);
            sim.KeyPress(VirtualKeyCode.DOWN);
            sim.KeyPress(VirtualKeyCode.RETURN);
            var i = 50;
            while (i-- > 0) sim.KeyPress(VirtualKeyCode.DOWN);
            sim.KeyPress(VirtualKeyCode.RETURN);
            Thread.Sleep(1000);
            sim.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4);
            sim.KeyPress(VirtualKeyCode.VK_N);
        }

        [Test]
        public void AnotherTest()
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(VirtualKeyCode.SPACE);

            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R);
            Thread.Sleep(1000);
            sim.Keyboard.TextEntry("mspaint");
            Thread.Sleep(1000);
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            Thread.Sleep(1000);


            // Paint
            sim.Mouse.LeftButtonDown();
            sim.Mouse.MoveMouseToPositionOnVirtualDesktop(65535/2, 65535/2);
            sim.Mouse.LeftButtonUp();

        }

        [Test]
        public void TestMouseMoveTo()
        {
            var sim = new InputSimulator();
            sim.Mouse.MoveMouseTo(0,0);
            Thread.Sleep(1000);
            sim.Mouse.MoveMouseTo(65535, 65535);
            Thread.Sleep(1000);
            sim.Mouse.MoveMouseTo(65535/2, 65535/2);
        }
    }
}