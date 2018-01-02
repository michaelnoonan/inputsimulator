# Windows Input Simulator Plus
This library is a fork of Michael Noonan's *Windows Input Simulator* (a C# wrapper around the `SendInput` functionality of Windows) and it can be used as a replacement of the original library without any source code changes. 

This fork supports scan codes, making it compatible with many applications that the original library does not support. 

## NuGet
Install-Package InputSimulatorPlus

# Examples

## Example: Single key press
```csharp
public void PressTheSpacebar()
{
    InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
}
```

## Example: Key-down and Key-up
```csharp
public void ShoutHello()
{
    // Simulate each key stroke    
    InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
    InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_H);
    InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_E);
    InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L);
    InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_L);
    InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O);
    InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_1);
    InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);

    // Alternatively you can simulate text entry to acheive the same end result
    InputSimulator.SimulateTextEntry("HELLO!");
}
```

## Example: Modified keystrokes such as CTRL-C
```csharp
public void SimulateSomeModifiedKeystrokes()
{
    // CTRL-C (effectively a copy command in many situations)
    InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);

    // You can simulate chords with multiple modifiers
    // For example CTRL-K-C whic is simulated as
    // CTRL-down, K, C, CTRL-up
    InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, new [] {VirtualKeyCode.VK_K, VirtualKeyCode.VK_C});

    // You can simulate complex chords with multiple modifiers and key presses
    // For example CTRL-ALT-SHIFT-ESC-K which is simulated as
    // CTRL-down, ALT-down, SHIFT-down, press ESC, press K, SHIFT-up, ALT-up, CTRL-up
    InputSimulator.SimulateModifiedKeyStroke(
        new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.MENU, VirtualKeyCode.SHIFT },
        new[] { VirtualKeyCode.ESCAPE, VirtualKeyCode.VK_K });
}
```

## Example: Simulate text entry
```csharp
public void SayHello()
{
    InputSimulator.SimulateTextEntry("Say hello!");
}
```

## Example: Determine the state of different types of keys
```csharp
public void GetKeyStatus()
{
    // Determines if the shift key is currently down
    var isShiftKeyDown = InputSimulator.IsKeyDown(VirtualKeyCode.SHIFT);

    // Determines if the caps lock key is currently in effect (toggled on)
    var isCapsLockOn = InputSimulator.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL);
}
```

