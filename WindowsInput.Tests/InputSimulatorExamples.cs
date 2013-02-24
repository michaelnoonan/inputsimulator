using NUnit.Framework;
using WindowsInput.Native;

namespace WindowsInput.Tests
{
    [TestFixture]
    public class InputSimulatorExamples
    {
        [Test]
        public void OpenWindowsExplorer()
        {
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_E);
        }

        [Test]
        public void SayHello()
        {
            var sim = new InputSimulator();
            sim.Keyboard
               .ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R)
               .Sleep(1000)
               .TextEntry("notepad")
               .Sleep(1000)
               .KeyPress(VirtualKeyCode.RETURN)
               .Sleep(1000)
               .TextEntry("These are your orders if you choose to accept them...")
               .TextEntry("This message will self destruct in 5 seconds.")
               .Sleep(5000)
               .ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE)
               .KeyPress(VirtualKeyCode.DOWN)
               .KeyPress(VirtualKeyCode.RETURN);
            
            var i = 10;
            while (i-- > 0)
            {
                sim.Keyboard.KeyPress(VirtualKeyCode.DOWN).Sleep(100);
            }

            sim.Keyboard
               .KeyPress(VirtualKeyCode.RETURN)
               .Sleep(1000)
               .ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4)
               .KeyPress(VirtualKeyCode.VK_N);
        }

        [Test]
        public void AnotherTest()
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(VirtualKeyCode.SPACE);

            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R)
               .Sleep(1000)
               .TextEntry("mspaint")
               .Sleep(1000)
               .KeyPress(VirtualKeyCode.RETURN)
               .Sleep(1000)
               .Mouse
               .LeftButtonDown()
               .MoveMouseToPositionOnVirtualDesktop(65535/2, 65535/2)
               .LeftButtonUp();

        }

        [Test]
        public void TestMouseMoveTo()
        {
            var sim = new InputSimulator();
            sim.Mouse
               .MoveMouseTo(0, 0)
               .Sleep(1000)
               .MoveMouseTo(65535, 65535)
               .Sleep(1000)
               .MoveMouseTo(65535/2, 65535/2);
        }
    }
}