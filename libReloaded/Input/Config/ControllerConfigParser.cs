/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/

using System;
using System.IO;
using IniParser;
using IniParser.Model;
using static Reloaded.Input.ControllerCommon;

namespace Reloaded.Input
{
    /// <summary>
    /// Parses configuration files for XInput, DirectInput controllers.
    /// </summary>
    public class ControllerConfigParser
    {
        /// <summary>
        /// Holds an instance of ini-parser used for parsing INI files.
        /// </summary>
        private readonly FileIniDataParser iniParser;

        /// <summary>
        /// Stores the ini data read by the ini-parser.
        /// </summary>
        private IniData iniData;

        /// <summary>
        /// Initiates the Controller Config Parser.
        /// </summary>
        public ControllerConfigParser()
        {
            iniParser = new FileIniDataParser();
            iniParser.Parser.Configuration.CommentString = "#";
        }

        /// <summary>
        /// Parses a Mod Loader Controller Configuration.
        /// </summary>
        /// <param name="configLocation">States the location of the controller configuration.</param>
        /// <param name="controller">A DirectInput or XInput controller instance.</param>
        public IController ParseConfig(string configLocation, IController controller)
        {
            // Instantiate the individual mapping structs.
            AxisMapping axisMapping = new AxisMapping();
            EmulationButtonMapping emulationMapping = new EmulationButtonMapping();
            ButtonMapping buttonMapping = new ButtonMapping();

            // Check if the config exists, if it doesn't, first write an empty config.
            if (! File.Exists(configLocation)) File.WriteAllText(configLocation, SampleConfig.Sample);

            // Read the controller configuration.
            iniData = iniParser.ReadFile(configLocation);

            // Read the controller port ID.
            controller.ControllerID = Convert.ToInt32(iniData["Main Settings"]["Controller_Port"]);

            // Read the controller button mappings.
            #region Button Mappings
            buttonMapping.Button_A = byte.Parse(iniData["Button Mappings"]["Button_A"]);
            buttonMapping.Button_B = byte.Parse(iniData["Button Mappings"]["Button_B"]);
            buttonMapping.Button_X = byte.Parse(iniData["Button Mappings"]["Button_X"]);
            buttonMapping.Button_Y = byte.Parse(iniData["Button Mappings"]["Button_Y"]);

            buttonMapping.Button_LB = byte.Parse(iniData["Button Mappings"]["Button_LB"]);
            buttonMapping.Button_RB = byte.Parse(iniData["Button Mappings"]["Button_RB"]);

            buttonMapping.Button_LS = byte.Parse(iniData["Button Mappings"]["Button_LS"]);
            buttonMapping.Button_RS = byte.Parse(iniData["Button Mappings"]["Button_RS"]);

            buttonMapping.Button_Back = byte.Parse(iniData["Button Mappings"]["Button_Back"]);
            buttonMapping.Button_Guide = byte.Parse(iniData["Button Mappings"]["Button_Guide"]);
            buttonMapping.Button_Start = byte.Parse(iniData["Button Mappings"]["Button_Start"]);
            #endregion

            // Read the controller axis emulation mappings.
            #region Emulation Mappings
            emulationMapping.DPAD_UP = byte.Parse(iniData["Emulation Mapping"]["DPAD_UP"]);
            emulationMapping.DPAD_LEFT = byte.Parse(iniData["Emulation Mapping"]["DPAD_LEFT"]);
            emulationMapping.DPAD_RIGHT = byte.Parse(iniData["Emulation Mapping"]["DPAD_RIGHT"]);
            emulationMapping.DPAD_DOWN = byte.Parse(iniData["Emulation Mapping"]["DPAD_DOWN"]);

            emulationMapping.Right_Trigger = byte.Parse(iniData["Emulation Mapping"]["Right_Trigger"]);
            emulationMapping.Left_Trigger = byte.Parse(iniData["Emulation Mapping"]["Left_Trigger"]);

            emulationMapping.Left_Stick_Up = byte.Parse(iniData["Emulation Mapping"]["Left_Stick_Up"]);
            emulationMapping.Left_Stick_Left = byte.Parse(iniData["Emulation Mapping"]["Left_Stick_Left"]);
            emulationMapping.Left_Stick_Down = byte.Parse(iniData["Emulation Mapping"]["Left_Stick_Down"]);
            emulationMapping.Left_Stick_Right = byte.Parse(iniData["Emulation Mapping"]["Left_Stick_Right"]);

            emulationMapping.Right_Stick_Up = byte.Parse(iniData["Emulation Mapping"]["Right_Stick_Up"]);
            emulationMapping.Right_Stick_Left = byte.Parse(iniData["Emulation Mapping"]["Right_Stick_Left"]);
            emulationMapping.Right_Stick_Down = byte.Parse(iniData["Emulation Mapping"]["Right_Stick_Down"]);
            emulationMapping.Right_Stick_Right = byte.Parse(iniData["Emulation Mapping"]["Right_Stick_Right"]);
            #endregion

            // Read the controller axis mappings.
            #region Axis Mappings
            axisMapping.leftStickX.propertyName = iniData["Axis"]["Left_Stick_X"];
            axisMapping.leftStickX.axis = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), iniData["Axis Type"]["Left_Stick_X"]);
            axisMapping.leftStickX.deadZone = float.Parse(iniData["Axis Deadzone"]["Left_Stick_X"]);
            axisMapping.leftStickX.isReversed = bool.Parse(iniData["Axis Inverse"]["Left_Stick_X"]);
            axisMapping.leftStickX.radiusScale = float.Parse(iniData["Radius Scale"]["Left_Stick_X"]);

            axisMapping.rightStickX.propertyName = iniData["Axis"]["Right_Stick_X"];
            axisMapping.rightStickX.axis = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), iniData["Axis Type"]["Right_Stick_X"]);
            axisMapping.rightStickX.deadZone = float.Parse(iniData["Axis Deadzone"]["Right_Stick_X"]);
            axisMapping.rightStickX.isReversed = bool.Parse(iniData["Axis Inverse"]["Right_Stick_X"]);
            axisMapping.rightStickX.radiusScale = float.Parse(iniData["Radius Scale"]["Right_Stick_X"]);

            axisMapping.leftStickY.propertyName = iniData["Axis"]["Left_Stick_Y"];
            axisMapping.leftStickY.axis = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), iniData["Axis Type"]["Left_Stick_Y"]);
            axisMapping.leftStickY.deadZone = float.Parse(iniData["Axis Deadzone"]["Left_Stick_Y"]);
            axisMapping.leftStickY.isReversed = bool.Parse(iniData["Axis Inverse"]["Left_Stick_Y"]);
            axisMapping.leftStickY.radiusScale = float.Parse(iniData["Radius Scale"]["Left_Stick_Y"]);

            axisMapping.rightStickY.propertyName = iniData["Axis"]["Right_Stick_Y"];
            axisMapping.rightStickY.axis = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), iniData["Axis Type"]["Right_Stick_Y"]);
            axisMapping.rightStickY.deadZone = float.Parse(iniData["Axis Deadzone"]["Right_Stick_Y"]);
            axisMapping.rightStickY.isReversed = bool.Parse(iniData["Axis Inverse"]["Right_Stick_Y"]);
            axisMapping.rightStickY.radiusScale = float.Parse(iniData["Radius Scale"]["Right_Stick_Y"]);

            axisMapping.leftTrigger.propertyName = iniData["Axis"]["Left_Trigger"];
            axisMapping.leftTrigger.axis = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), iniData["Axis Type"]["Left_Trigger"]);
            axisMapping.leftTrigger.deadZone = float.Parse(iniData["Axis Deadzone"]["Left_Trigger"]);
            axisMapping.leftTrigger.isReversed = bool.Parse(iniData["Axis Inverse"]["Left_Trigger"]);
            axisMapping.leftTrigger.radiusScale = float.Parse(iniData["Radius Scale"]["Left_Trigger"]);

            axisMapping.rightTrigger.propertyName = iniData["Axis"]["Right_Trigger"];
            axisMapping.rightTrigger.axis = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), iniData["Axis Type"]["Right_Trigger"]);
            axisMapping.rightTrigger.deadZone = float.Parse(iniData["Axis Deadzone"]["Right_Trigger"]);
            axisMapping.rightTrigger.isReversed = bool.Parse(iniData["Axis Inverse"]["Right_Trigger"]);
            axisMapping.rightTrigger.radiusScale = float.Parse(iniData["Radius Scale"]["Right_Trigger"]);
            #endregion

            // Assign Mappings back to Controller
            controller.AxisMapping = axisMapping;
            controller.ButtonMapping = buttonMapping;
            controller.EmulationMapping = emulationMapping;

            // Return controller.
            return controller;
        }

        /// <summary>
        /// Writes a mod loader controller configuration.
        /// </summary>
        /// <param name="configLocation">States the location of the controller configuration.</param>
        /// <param name="controller">A DirectInput or XInput controller instance.</param>
        public void WriteConfig(string configLocation, IController controller)
        {
            // Controller port ID.
            iniData["Main Settings"]["Controller_Port"] = Convert.ToString(controller.ControllerID);

            // Controller button mappings.
            #region Button Mappings
            iniData["Button Mappings"]["Button_A"] = Convert.ToString((int)controller.ButtonMapping.Button_A);
            iniData["Button Mappings"]["Button_B"] = Convert.ToString((int)controller.ButtonMapping.Button_B);
            iniData["Button Mappings"]["Button_X"] = Convert.ToString((int)controller.ButtonMapping.Button_X);
            iniData["Button Mappings"]["Button_Y"] = Convert.ToString((int)controller.ButtonMapping.Button_Y);

            iniData["Button Mappings"]["Button_LB"] = Convert.ToString((int)controller.ButtonMapping.Button_LB);
            iniData["Button Mappings"]["Button_RB"] = Convert.ToString((int)controller.ButtonMapping.Button_RB);

            iniData["Button Mappings"]["Button_LS"] = Convert.ToString((int)controller.ButtonMapping.Button_LS);
            iniData["Button Mappings"]["Button_RS"] = Convert.ToString((int)controller.ButtonMapping.Button_RS);

            iniData["Button Mappings"]["Button_Back"] = Convert.ToString((int)controller.ButtonMapping.Button_Back);
            iniData["Button Mappings"]["Button_Guide"] = Convert.ToString((int)controller.ButtonMapping.Button_Guide);
            iniData["Button Mappings"]["Button_Start"] = Convert.ToString((int)controller.ButtonMapping.Button_Start);
            #endregion

            // Controller axis emulation mappings.
            #region Emulation Mappings
            iniData["Emulation Mapping"]["DPAD_UP"] = Convert.ToString((int)controller.EmulationMapping.DPAD_UP);
            iniData["Emulation Mapping"]["DPAD_LEFT"] = Convert.ToString((int)controller.EmulationMapping.DPAD_LEFT);
            iniData["Emulation Mapping"]["DPAD_RIGHT"] = Convert.ToString((int)controller.EmulationMapping.DPAD_RIGHT);
            iniData["Emulation Mapping"]["DPAD_DOWN"] = Convert.ToString((int)controller.EmulationMapping.DPAD_DOWN);

            iniData["Emulation Mapping"]["Right_Trigger"] = Convert.ToString((int)controller.EmulationMapping.Right_Trigger);
            iniData["Emulation Mapping"]["Left_Trigger"] = Convert.ToString((int)controller.EmulationMapping.Left_Trigger);

            iniData["Emulation Mapping"]["Left_Stick_Up"] = Convert.ToString((int)controller.EmulationMapping.Left_Stick_Up);
            iniData["Emulation Mapping"]["Left_Stick_Left"] = Convert.ToString((int)controller.EmulationMapping.Left_Stick_Left);
            iniData["Emulation Mapping"]["Left_Stick_Down"] = Convert.ToString((int)controller.EmulationMapping.Left_Stick_Down);
            iniData["Emulation Mapping"]["Left_Stick_Right"] = Convert.ToString((int)controller.EmulationMapping.Left_Stick_Right);

            iniData["Emulation Mapping"]["Right_Stick_Up"] = Convert.ToString((int)controller.EmulationMapping.Right_Stick_Up);
            iniData["Emulation Mapping"]["Right_Stick_Left"] = Convert.ToString((int)controller.EmulationMapping.Right_Stick_Left);
            iniData["Emulation Mapping"]["Right_Stick_Down"] = Convert.ToString((int)controller.EmulationMapping.Right_Stick_Down);
            iniData["Emulation Mapping"]["Right_Stick_Right"] = Convert.ToString((int)controller.EmulationMapping.Right_Stick_Right);
            #endregion

            // Controller axis mappings.
            #region Axis Mappings
            iniData["Axis"]["Left_Stick_X"] = Convert.ToString(controller.AxisMapping.leftStickX.propertyName);
            iniData["Axis Type"]["Left_Stick_X"] = Enum.GetName(typeof(ControllerAxis),controller.AxisMapping.leftStickX.axis);
            iniData["Axis Deadzone"]["Left_Stick_X"] = Convert.ToString((int)controller.AxisMapping.leftStickX.deadZone);
            iniData["Axis Inverse"]["Left_Stick_X"] = Convert.ToString(controller.AxisMapping.leftStickX.isReversed);
            iniData["Radius Scale"]["Left_Stick_X"] = Convert.ToString(controller.AxisMapping.leftStickX.radiusScale);

            iniData["Axis"]["Right_Stick_X"] = Convert.ToString(controller.AxisMapping.rightStickX.propertyName);
            iniData["Axis Type"]["Right_Stick_X"] = Enum.GetName(typeof(ControllerAxis),controller.AxisMapping.rightStickX.axis);
            iniData["Axis Deadzone"]["Right_Stick_X"] = Convert.ToString((int)controller.AxisMapping.rightStickX.deadZone);
            iniData["Axis Inverse"]["Right_Stick_X"] = Convert.ToString(controller.AxisMapping.rightStickX.isReversed);
            iniData["Radius Scale"]["Right_Stick_X"] = Convert.ToString(controller.AxisMapping.rightStickX.radiusScale);

            iniData["Axis"]["Left_Stick_Y"] = Convert.ToString(controller.AxisMapping.leftStickY.propertyName);
            iniData["Axis Type"]["Left_Stick_Y"] = Enum.GetName(typeof(ControllerAxis),controller.AxisMapping.leftStickY.axis);
            iniData["Axis Deadzone"]["Left_Stick_Y"] = Convert.ToString((int)controller.AxisMapping.leftStickY.deadZone);
            iniData["Axis Inverse"]["Left_Stick_Y"] = Convert.ToString(controller.AxisMapping.leftStickY.isReversed);
            iniData["Radius Scale"]["Left_Stick_Y"] = Convert.ToString(controller.AxisMapping.leftStickY.radiusScale);

            iniData["Axis"]["Right_Stick_Y"] = Convert.ToString(controller.AxisMapping.rightStickY.propertyName);
            iniData["Axis Type"]["Right_Stick_Y"] = Enum.GetName(typeof(ControllerAxis),controller.AxisMapping.rightStickY.axis);
            iniData["Axis Deadzone"]["Right_Stick_Y"] = Convert.ToString((int)controller.AxisMapping.rightStickY.deadZone);
            iniData["Axis Inverse"]["Right_Stick_Y"] = Convert.ToString(controller.AxisMapping.rightStickY.isReversed);
            iniData["Radius Scale"]["Right_Stick_Y"] = Convert.ToString(controller.AxisMapping.rightStickY.radiusScale);

            iniData["Axis"]["Left_Trigger"] = Convert.ToString(controller.AxisMapping.leftTrigger.propertyName);
            iniData["Axis Type"]["Left_Trigger"] = Enum.GetName(typeof(ControllerAxis), controller.AxisMapping.leftTrigger.axis);
            iniData["Axis Deadzone"]["Left_Trigger"] = Convert.ToString((int)controller.AxisMapping.leftTrigger.deadZone);
            iniData["Axis Inverse"]["Left_Trigger"] = Convert.ToString(controller.AxisMapping.leftTrigger.isReversed);
            iniData["Radius Scale"]["Left_Trigger"] = Convert.ToString(controller.AxisMapping.leftTrigger.radiusScale);

            iniData["Axis"]["Right_Trigger"] = Convert.ToString(controller.AxisMapping.rightTrigger.propertyName);
            iniData["Axis Type"]["Right_Trigger"] = Enum.GetName(typeof(ControllerAxis), controller.AxisMapping.rightTrigger.axis);
            iniData["Axis Deadzone"]["Right_Trigger"] = Convert.ToString((int)controller.AxisMapping.rightTrigger.deadZone);
            iniData["Axis Inverse"]["Right_Trigger"] = Convert.ToString(controller.AxisMapping.rightTrigger.isReversed);
            iniData["Radius Scale"]["Right_Trigger"] = Convert.ToString(controller.AxisMapping.rightTrigger.radiusScale);
            #endregion

            // Write config.
            iniParser.WriteFile(configLocation, iniData);
        }
    }
}
