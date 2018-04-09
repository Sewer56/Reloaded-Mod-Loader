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
using System.Globalization;
using System.IO;
using IniParser;
using IniParser.Model;
using Reloaded.Input.Common;

namespace Reloaded.Input.Config
{
    /// <summary>
    /// Parses configuration files for XInput, DirectInput controllers.
    /// </summary>
    public class ControllerConfigParser
    {
        /// <summary>
        /// Holds an instance of ini-parser used for parsing INI files.
        /// </summary>
        private readonly FileIniDataParser _iniParser;

        /// <summary>
        /// Stores the ini data read by the ini-parser.
        /// </summary>
        private IniData _iniData;

        /// <summary>
        /// Initiates the Controller Config Parser.
        /// </summary>
        public ControllerConfigParser()
        {
            _iniParser = new FileIniDataParser();
            _iniParser.Parser.Configuration.CommentString = "#";
        }

        /// <summary>
        /// Parses a Mod Loader Controller Configuration.
        /// </summary>
        /// <param name="configLocation">States the location of the controller configuration.</param>
        /// <param name="controller">A DirectInput or XInput controller instance.</param>
        public ControllerCommon.IController ParseConfig(string configLocation, ControllerCommon.IController controller)
        {
            // Instantiate the individual mapping structs.
            ControllerCommon.AxisMapping axisMapping = new ControllerCommon.AxisMapping();
            ControllerCommon.EmulationButtonMapping emulationMapping = new ControllerCommon.EmulationButtonMapping();
            ControllerCommon.ButtonMapping buttonMapping = new ControllerCommon.ButtonMapping();

            // Check if the config exists, if it doesn't, first write an empty config.
            if (!File.Exists(configLocation))
            {
                // Check if directory exists first.
                // Unpack sample themes
                if (!Directory.Exists(Path.GetDirectoryName(configLocation)))
                { Directory.CreateDirectory(Path.GetDirectoryName(configLocation)); }

                // Write Sample Config
                File.WriteAllText(configLocation, SampleConfig.Sample);
            }
                

            // Read the controller configuration.
            _iniData = _iniParser.ReadFile(configLocation);

            // Read the controller port ID.
            controller.ControllerId = Convert.ToInt32(_iniData["Main Settings"]["Controller_Port"]);

            // Read the controller button mappings.
            #region Button Mappings
            buttonMapping.ButtonA = byte.Parse(_iniData["Button Mappings"]["Button_A"]);
            buttonMapping.ButtonB = byte.Parse(_iniData["Button Mappings"]["Button_B"]);
            buttonMapping.ButtonX = byte.Parse(_iniData["Button Mappings"]["Button_X"]);
            buttonMapping.ButtonY = byte.Parse(_iniData["Button Mappings"]["Button_Y"]);

            buttonMapping.ButtonLb = byte.Parse(_iniData["Button Mappings"]["Button_LB"]);
            buttonMapping.ButtonRb = byte.Parse(_iniData["Button Mappings"]["Button_RB"]);

            buttonMapping.ButtonLs = byte.Parse(_iniData["Button Mappings"]["Button_LS"]);
            buttonMapping.ButtonRs = byte.Parse(_iniData["Button Mappings"]["Button_RS"]);

            buttonMapping.ButtonBack = byte.Parse(_iniData["Button Mappings"]["Button_Back"]);
            buttonMapping.ButtonGuide = byte.Parse(_iniData["Button Mappings"]["Button_Guide"]);
            buttonMapping.ButtonStart = byte.Parse(_iniData["Button Mappings"]["Button_Start"]);
            #endregion

            // Read the controller axis emulation mappings.
            #region Emulation Mappings
            emulationMapping.DpadUp = byte.Parse(_iniData["Emulation Mapping"]["DPAD_UP"]);
            emulationMapping.DpadLeft = byte.Parse(_iniData["Emulation Mapping"]["DPAD_LEFT"]);
            emulationMapping.DpadRight = byte.Parse(_iniData["Emulation Mapping"]["DPAD_RIGHT"]);
            emulationMapping.DpadDown = byte.Parse(_iniData["Emulation Mapping"]["DPAD_DOWN"]);

            emulationMapping.RightTrigger = byte.Parse(_iniData["Emulation Mapping"]["Right_Trigger"]);
            emulationMapping.LeftTrigger = byte.Parse(_iniData["Emulation Mapping"]["Left_Trigger"]);

            emulationMapping.LeftStickUp = byte.Parse(_iniData["Emulation Mapping"]["Left_Stick_Up"]);
            emulationMapping.LeftStickLeft = byte.Parse(_iniData["Emulation Mapping"]["Left_Stick_Left"]);
            emulationMapping.LeftStickDown = byte.Parse(_iniData["Emulation Mapping"]["Left_Stick_Down"]);
            emulationMapping.LeftStickRight = byte.Parse(_iniData["Emulation Mapping"]["Left_Stick_Right"]);

            emulationMapping.RightStickUp = byte.Parse(_iniData["Emulation Mapping"]["Right_Stick_Up"]);
            emulationMapping.RightStickLeft = byte.Parse(_iniData["Emulation Mapping"]["Right_Stick_Left"]);
            emulationMapping.RightStickDown = byte.Parse(_iniData["Emulation Mapping"]["Right_Stick_Down"]);
            emulationMapping.RightStickRight = byte.Parse(_iniData["Emulation Mapping"]["Right_Stick_Right"]);
            #endregion

            // Read the controller axis mappings.
            #region Axis Mappings
            axisMapping.LeftStickX.SourceAxis = _iniData["Axis"]["Left_Stick_X"];
            axisMapping.LeftStickX.DestinationAxis = ParseAxisName(_iniData["Axis Type"]["Left_Stick_X"]);
            axisMapping.LeftStickX.DeadZone = float.Parse(_iniData["Axis Deadzone"]["Left_Stick_X"]);
            axisMapping.LeftStickX.IsReversed = bool.Parse(_iniData["Axis Inverse"]["Left_Stick_X"]);
            axisMapping.LeftStickX.RadiusScale = float.Parse(_iniData["Radius Scale"]["Left_Stick_X"]);

            axisMapping.RightStickX.SourceAxis = _iniData["Axis"]["Right_Stick_X"];
            axisMapping.RightStickX.DestinationAxis = ParseAxisName(_iniData["Axis Type"]["Right_Stick_X"]);
            axisMapping.RightStickX.DeadZone = float.Parse(_iniData["Axis Deadzone"]["Right_Stick_X"]);
            axisMapping.RightStickX.IsReversed = bool.Parse(_iniData["Axis Inverse"]["Right_Stick_X"]);
            axisMapping.RightStickX.RadiusScale = float.Parse(_iniData["Radius Scale"]["Right_Stick_X"]);

            axisMapping.LeftStickY.SourceAxis = _iniData["Axis"]["Left_Stick_Y"];
            axisMapping.LeftStickY.DestinationAxis = ParseAxisName(_iniData["Axis Type"]["Left_Stick_Y"]);
            axisMapping.LeftStickY.DeadZone = float.Parse(_iniData["Axis Deadzone"]["Left_Stick_Y"]);
            axisMapping.LeftStickY.IsReversed = bool.Parse(_iniData["Axis Inverse"]["Left_Stick_Y"]);
            axisMapping.LeftStickY.RadiusScale = float.Parse(_iniData["Radius Scale"]["Left_Stick_Y"]);

            axisMapping.RightStickY.SourceAxis = _iniData["Axis"]["Right_Stick_Y"];
            axisMapping.RightStickY.DestinationAxis = ParseAxisName(_iniData["Axis Type"]["Right_Stick_Y"]);
            axisMapping.RightStickY.DeadZone = float.Parse(_iniData["Axis Deadzone"]["Right_Stick_Y"]);
            axisMapping.RightStickY.IsReversed = bool.Parse(_iniData["Axis Inverse"]["Right_Stick_Y"]);
            axisMapping.RightStickY.RadiusScale = float.Parse(_iniData["Radius Scale"]["Right_Stick_Y"]);

            axisMapping.LeftTrigger.SourceAxis = _iniData["Axis"]["Left_Trigger"];
            axisMapping.LeftTrigger.DestinationAxis = ParseAxisName(_iniData["Axis Type"]["Left_Trigger"]);
            axisMapping.LeftTrigger.DeadZone = float.Parse(_iniData["Axis Deadzone"]["Left_Trigger"]);
            axisMapping.LeftTrigger.IsReversed = bool.Parse(_iniData["Axis Inverse"]["Left_Trigger"]);
            axisMapping.LeftTrigger.RadiusScale = float.Parse(_iniData["Radius Scale"]["Left_Trigger"]);

            axisMapping.RightTrigger.SourceAxis = _iniData["Axis"]["Right_Trigger"];
            axisMapping.RightTrigger.DestinationAxis = ParseAxisName(_iniData["Axis Type"]["Right_Trigger"]);
            axisMapping.RightTrigger.DeadZone = float.Parse(_iniData["Axis Deadzone"]["Right_Trigger"]);
            axisMapping.RightTrigger.IsReversed = bool.Parse(_iniData["Axis Inverse"]["Right_Trigger"]);
            axisMapping.RightTrigger.RadiusScale = float.Parse(_iniData["Radius Scale"]["Right_Trigger"]);
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
        public void WriteConfig(string configLocation, ControllerCommon.IController controller)
        {
            // Controller port ID.
            _iniData["Main Settings"]["Controller_Port"] = Convert.ToString(controller.ControllerId);

            // Controller button mappings.
            #region Button Mappings
            _iniData["Button Mappings"]["Button_A"] = Convert.ToString((int)controller.ButtonMapping.ButtonA);
            _iniData["Button Mappings"]["Button_B"] = Convert.ToString((int)controller.ButtonMapping.ButtonB);
            _iniData["Button Mappings"]["Button_X"] = Convert.ToString((int)controller.ButtonMapping.ButtonX);
            _iniData["Button Mappings"]["Button_Y"] = Convert.ToString((int)controller.ButtonMapping.ButtonY);

            _iniData["Button Mappings"]["Button_LB"] = Convert.ToString((int)controller.ButtonMapping.ButtonLb);
            _iniData["Button Mappings"]["Button_RB"] = Convert.ToString((int)controller.ButtonMapping.ButtonRb);

            _iniData["Button Mappings"]["Button_LS"] = Convert.ToString((int)controller.ButtonMapping.ButtonLs);
            _iniData["Button Mappings"]["Button_RS"] = Convert.ToString((int)controller.ButtonMapping.ButtonRs);

            _iniData["Button Mappings"]["Button_Back"] = Convert.ToString((int)controller.ButtonMapping.ButtonBack);
            _iniData["Button Mappings"]["Button_Guide"] = Convert.ToString((int)controller.ButtonMapping.ButtonGuide);
            _iniData["Button Mappings"]["Button_Start"] = Convert.ToString((int)controller.ButtonMapping.ButtonStart);
            #endregion

            // Controller axis emulation mappings.
            #region Emulation Mappings
            _iniData["Emulation Mapping"]["DPAD_UP"] = Convert.ToString((int)controller.EmulationMapping.DpadUp);
            _iniData["Emulation Mapping"]["DPAD_LEFT"] = Convert.ToString((int)controller.EmulationMapping.DpadLeft);
            _iniData["Emulation Mapping"]["DPAD_RIGHT"] = Convert.ToString((int)controller.EmulationMapping.DpadRight);
            _iniData["Emulation Mapping"]["DPAD_DOWN"] = Convert.ToString((int)controller.EmulationMapping.DpadDown);

            _iniData["Emulation Mapping"]["Right_Trigger"] = Convert.ToString((int)controller.EmulationMapping.RightTrigger);
            _iniData["Emulation Mapping"]["Left_Trigger"] = Convert.ToString((int)controller.EmulationMapping.LeftTrigger);

            _iniData["Emulation Mapping"]["Left_Stick_Up"] = Convert.ToString((int)controller.EmulationMapping.LeftStickUp);
            _iniData["Emulation Mapping"]["Left_Stick_Left"] = Convert.ToString((int)controller.EmulationMapping.LeftStickLeft);
            _iniData["Emulation Mapping"]["Left_Stick_Down"] = Convert.ToString((int)controller.EmulationMapping.LeftStickDown);
            _iniData["Emulation Mapping"]["Left_Stick_Right"] = Convert.ToString((int)controller.EmulationMapping.LeftStickRight);

            _iniData["Emulation Mapping"]["Right_Stick_Up"] = Convert.ToString((int)controller.EmulationMapping.RightStickUp);
            _iniData["Emulation Mapping"]["Right_Stick_Left"] = Convert.ToString((int)controller.EmulationMapping.RightStickLeft);
            _iniData["Emulation Mapping"]["Right_Stick_Down"] = Convert.ToString((int)controller.EmulationMapping.RightStickDown);
            _iniData["Emulation Mapping"]["Right_Stick_Right"] = Convert.ToString((int)controller.EmulationMapping.RightStickRight);
            #endregion

            // Controller axis mappings.
            #region Axis Mappings
            _iniData["Axis"]["Left_Stick_X"] = Convert.ToString(controller.AxisMapping.LeftStickX.SourceAxis);
            _iniData["Axis Type"]["Left_Stick_X"] = GetAxisName(controller.AxisMapping.LeftStickX.DestinationAxis);
            _iniData["Axis Deadzone"]["Left_Stick_X"] = Convert.ToString((int)controller.AxisMapping.LeftStickX.DeadZone);
            _iniData["Axis Inverse"]["Left_Stick_X"] = Convert.ToString(controller.AxisMapping.LeftStickX.IsReversed);
            _iniData["Radius Scale"]["Left_Stick_X"] = Convert.ToString(controller.AxisMapping.LeftStickX.RadiusScale, CultureInfo.InvariantCulture);

            _iniData["Axis"]["Right_Stick_X"] = Convert.ToString(controller.AxisMapping.RightStickX.SourceAxis);
            _iniData["Axis Type"]["Right_Stick_X"] = GetAxisName(controller.AxisMapping.RightStickX.DestinationAxis);
            _iniData["Axis Deadzone"]["Right_Stick_X"] = Convert.ToString((int)controller.AxisMapping.RightStickX.DeadZone);
            _iniData["Axis Inverse"]["Right_Stick_X"] = Convert.ToString(controller.AxisMapping.RightStickX.IsReversed);
            _iniData["Radius Scale"]["Right_Stick_X"] = Convert.ToString(controller.AxisMapping.RightStickX.RadiusScale, CultureInfo.InvariantCulture);

            _iniData["Axis"]["Left_Stick_Y"] = Convert.ToString(controller.AxisMapping.LeftStickY.SourceAxis);
            _iniData["Axis Type"]["Left_Stick_Y"] = GetAxisName(controller.AxisMapping.LeftStickY.DestinationAxis);
            _iniData["Axis Deadzone"]["Left_Stick_Y"] = Convert.ToString((int)controller.AxisMapping.LeftStickY.DeadZone);
            _iniData["Axis Inverse"]["Left_Stick_Y"] = Convert.ToString(controller.AxisMapping.LeftStickY.IsReversed);
            _iniData["Radius Scale"]["Left_Stick_Y"] = Convert.ToString(controller.AxisMapping.LeftStickY.RadiusScale, CultureInfo.InvariantCulture);

            _iniData["Axis"]["Right_Stick_Y"] = Convert.ToString(controller.AxisMapping.RightStickY.SourceAxis);
            _iniData["Axis Type"]["Right_Stick_Y"] = GetAxisName(controller.AxisMapping.RightStickY.DestinationAxis);
            _iniData["Axis Deadzone"]["Right_Stick_Y"] = Convert.ToString((int)controller.AxisMapping.RightStickY.DeadZone);
            _iniData["Axis Inverse"]["Right_Stick_Y"] = Convert.ToString(controller.AxisMapping.RightStickY.IsReversed);
            _iniData["Radius Scale"]["Right_Stick_Y"] = Convert.ToString(controller.AxisMapping.RightStickY.RadiusScale, CultureInfo.InvariantCulture);

            _iniData["Axis"]["Left_Trigger"] = Convert.ToString(controller.AxisMapping.LeftTrigger.SourceAxis);
            _iniData["Axis Type"]["Left_Trigger"] = GetAxisName(controller.AxisMapping.LeftTrigger.DestinationAxis);
            _iniData["Axis Deadzone"]["Left_Trigger"] = Convert.ToString((int)controller.AxisMapping.LeftTrigger.DeadZone);
            _iniData["Axis Inverse"]["Left_Trigger"] = Convert.ToString(controller.AxisMapping.LeftTrigger.IsReversed);
            _iniData["Radius Scale"]["Left_Trigger"] = Convert.ToString(controller.AxisMapping.LeftTrigger.RadiusScale, CultureInfo.InvariantCulture);

            _iniData["Axis"]["Right_Trigger"] = Convert.ToString(controller.AxisMapping.RightTrigger.SourceAxis);
            _iniData["Axis Type"]["Right_Trigger"] = GetAxisName(controller.AxisMapping.RightTrigger.DestinationAxis);
            _iniData["Axis Deadzone"]["Right_Trigger"] = Convert.ToString((int)controller.AxisMapping.RightTrigger.DeadZone);
            _iniData["Axis Inverse"]["Right_Trigger"] = Convert.ToString(controller.AxisMapping.RightTrigger.IsReversed);
            _iniData["Radius Scale"]["Right_Trigger"] = Convert.ToString(controller.AxisMapping.RightTrigger.RadiusScale, CultureInfo.InvariantCulture);
            #endregion

            // Write config.
            _iniParser.WriteFile(configLocation, _iniData);
        }

        /// <summary>
        /// Parses an axis name as seen in the controller configuration files to axis
        /// in libReloaded code. This ensures that remapping code does not potentiall break 
        /// after refactoring axis names in source code.
        /// </summary>
        /// <param name="configAxisName">The axis name as seen in the configuration file.</param>
        private ControllerCommon.ControllerAxis ParseAxisName(string configAxisName)
        {
            switch (configAxisName)
            {
                case "Left_Stick_X":    return ControllerCommon.ControllerAxis.LeftStickX;
                case "Left_Stick_Y":    return ControllerCommon.ControllerAxis.LeftStickY;
                case "Right_Stick_X":   return ControllerCommon.ControllerAxis.RightStickX;
                case "Right_Stick_Y":   return ControllerCommon.ControllerAxis.RightStickY;
                case "Left_Trigger":    return ControllerCommon.ControllerAxis.LeftTrigger;
                case "Right_Trigger":   return ControllerCommon.ControllerAxis.RightTrigger;
                default:                return ControllerCommon.ControllerAxis.Null;
            }
        }

        /// <summary>
        /// Parses a passed in Controller Axis enumberable and spits out an axis name to use within
        /// the controller configuration files.
        /// </summary>
        /// <param name="controllerAxis">The controller axis to convert to string for config file storage..</param>
        private string GetAxisName(ControllerCommon.ControllerAxis controllerAxis)
        {
            switch (controllerAxis)
            {
                case ControllerCommon.ControllerAxis.LeftStickX:    return "Left_Stick_X";
                case ControllerCommon.ControllerAxis.LeftStickY:    return "Left_Stick_Y";
                case ControllerCommon.ControllerAxis.RightStickX:   return "Right_Stick_X";
                case ControllerCommon.ControllerAxis.RightStickY:   return "Right_Stick_Y";
                case ControllerCommon.ControllerAxis.LeftTrigger:   return "Left_Trigger";
                case ControllerCommon.ControllerAxis.RightTrigger:  return "Right_Trigger";
                default:                                            return "Null";
            }
        }
    }
}
