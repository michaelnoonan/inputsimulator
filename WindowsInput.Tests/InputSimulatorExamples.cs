using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WindowsInput.Native;

namespace WindowsInput.Tests
{
    [TestFixture]
    public class InputSimulatorExamples
    {
        [Test]
        [Explicit]
        public void OpenWindowsExplorer()
        {
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_E);
        }

        [Test]
        [Explicit]
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
        [Explicit]
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
        [Explicit]
        public void TestMouseMoveTo()
        {
            var bounds = Screen.PrimaryScreen.WorkingArea;

            var sim = new InputSimulator();
            sim.Mouse
               .MoveMouseTo(0, 0)
               .Sleep(1000)
               .MoveMouseTo(bounds.Width, bounds.Height)
               .Sleep(1000)
               .MoveMouseTo(bounds.Width / 2, bounds.Height / 2);
        }

        [Test]
        [Explicit]
        public void TestDragDrop()
        {
            Process notepad = Process.Start(Path.Combine(Environment.GetEnvironmentVariable("WINDIR"), "notepad.exe"));
            notepad.WaitForInputIdle();

            var sim = new InputSimulator();

            var bounds = WaitForMainWindowPosition(notepad, sim);
            Assert.IsFalse(bounds.IsEmpty, "Window is not showing up");

            // get grab position in the title bar.
            int x = bounds.Left + (bounds.Width / 2);
            int y = bounds.Top + 10;

            sim.Mouse
               .MoveMouseTo(x, y)
               .Sleep(100)
               .LeftButtonDown();

            for (int i = 0; i < 100; i += 1)
            {
                sim.Mouse.MoveMouseTo(x + i, y).Sleep(1);
            }

            sim.Mouse.LeftButtonUp();

            notepad.Kill();
        }

        private Rectangle WaitForMainWindowPosition(Process process, InputSimulator sim, int timeoutMilliseconds = 5000)
        {
            int delay = 10; 
            for (int timeout = 0; timeout < timeoutMilliseconds; timeout += delay)
            {
                NativeMethods.WINDOWPLACEMENT placement = new NativeMethods.WINDOWPLACEMENT();
                NativeMethods.GetWindowPlacement(process.MainWindowHandle, ref placement);
                var width = (placement.rcNormalPosition.right - placement.rcNormalPosition.left);
                if (width == 0)
                {
                    sim.Mouse.Sleep(delay);
                }
                else
                {
                    // find a position in the center of the title bar.
                    var height = (placement.rcNormalPosition.right - placement.rcNormalPosition.left);
                    var x = placement.rcNormalPosition.left;
                    var y = placement.rcNormalPosition.top;
                    return new Rectangle((int)x, (int)y, (int)width, (int)height);
                }
            }
            return Rectangle.Empty;
        }
    }
}