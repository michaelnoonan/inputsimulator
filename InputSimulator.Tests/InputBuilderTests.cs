using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WindowsInput;

namespace WindowsInput.Tests
{
    [TestFixture]
    public class InputBuilderTests
    {
        [Test]
        public void AddKeyDown()
        {
            var builder = new InputBuilder();
            Assert.That(builder.ToArray(), Is.Empty);
            builder.AddKeyDown(VirtualKeyCode.VK_A);
            Assert.That(builder.Count(), Is.EqualTo(1));
            Assert.That(builder[0].Type, Is.EqualTo((uint)InputType.Keyboard));
            Assert.That(builder[0].Data.Keyboard.KeyCode, Is.EqualTo((ushort)VirtualKeyCode.VK_A));
        }
    }
}
