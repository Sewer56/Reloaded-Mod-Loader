using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.Input
{
    public static class SampleConfig
    {
        public static string Sample =
@"# [Reloaded] Input Stack Configuration
# DInput: Button IDs are mapped onto their respective DirectInput key codes.
# XInput: Button IDs are a custom enumerable, which may change in the future.

[Main Settings]
# Defines the port that the controller is assigned to.
# By default the loader reads all devices assigned to a port.
Controller_Port=0

[Button Mappings]
Button_A=255
Button_B=255
Button_X=255
Button_Y=255
Button_LB=255
Button_RB=255
Button_LS=255
Button_RS=255
Button_Back=255
Button_Guide=255
Button_Start=255

[Axis]
# Stores the name of the programmatical property where the axis information is obtained from.
# This allows for the axis to be assigned to any other axis type on an input device, such as 
# velocity, torque and other factors of a multi-axis controller.
Left_Stick_X=Null
Left_Stick_Y=Null
Right_Stick_X=Null
Right_Stick_Y=Null
Left_Trigger=Null
Right_Trigger=Null

[Axis Type]
# Specifies the default axis types for each axis.
# This is just a leftover from more legacy code and should not be modified by the user.
# I guess you could disable an entire axis with this... if you really w.w.w..w...ant to.
# o.o.o.okay... actually... it's used to remap XInput axis, ahahahah.
Left_Stick_X=Left_Stick_X
Left_Stick_Y=Left_Stick_Y
Right_Stick_X=Right_Stick_X
Right_Stick_Y=Right_Stick_Y
Left_Trigger=Left_Trigger
Right_Trigger=Right_Trigger

[Axis Inverse]
# If set to true, the value read from the axis will be reversed.
# e.g. You can reverse stick direction.
Left_Stick_X=false
Left_Stick_Y=false
Right_Stick_X=false
Right_Stick_Y=false
Left_Trigger=false
Right_Trigger=false

[Axis Deadzone]
# Deadzone for the axis, between 0 and 100% (float)
# If you don't know what a deadzone is, you should google it.
# In short, the deadzone is an area from the center of the joystick that wouldn't recognize input if the stick is moved within it.
Left_Stick_X=5.00
Left_Stick_Y=5.00
Right_Stick_X=5.00
Right_Stick_Y=5.00
Left_Trigger=3.00
Right_Trigger=3.00

[Radius Scale]
# This is a value which multiplies all axis' analog inputs.
# Setting this above 1 allows for the extension of the radius of an analog stick or trigger.
# This is useful if you want to restrict trigger movement or want to reach all value maximums on each axis.
Left_Stick_X=1.00
Left_Stick_Y=1.00
Right_Stick_X=1.00
Right_Stick_Y=1.00
Left_Trigger=1.00
Right_Trigger=1.00

[Emulation Mapping]
# This section allows binding of the regular buttons to axis and the DPAD.
# e.g. Pressing left arrow on a keyboard maps to an analog stick movement of left.
DPAD_UP=255
DPAD_RIGHT=255
DPAD_DOWN=255
DPAD_LEFT=255

Left_Trigger=255
Right_Trigger=255

Left_Stick_Up=255
Left_Stick_Down=255
Left_Stick_Left=255
Left_Stick_Right=255

Right_Stick_Up=255
Right_Stick_Down=255
Right_Stick_Left=255
Right_Stick_Right=255
";

    }
}
