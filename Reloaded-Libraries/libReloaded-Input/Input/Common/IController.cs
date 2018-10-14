using Reloaded.Input.Config;
using Reloaded.Input.Config.Substructures;
using Reloaded.Input.Modules;

namespace Reloaded.Input.Common
{
    /// <summary>
    /// Defines an interface for DirectInput + XInput Controller implementations which defines the function names
    /// and signatures to be shared between both the DirectInput and XInput controller implementations.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Defines a general class which contains the individual input mappings for the remapper.
        /// Separated into a separate class to allow for easier serialization and deserialization.
        /// </summary>
        InputMappings InputMappings { get; set; }

        /// <summary>
        /// Provides an implementation to be used for remapping of controller inputs.
        /// </summary>
        Remapper Remapper { get; set; }

        /// <summary>
        /// Retrieves whether a specific button is pressed or not. 
        /// Accepts an enum of Controller_Buttons_Generic as parameter and returns the button ID mapped to
        /// the requested Controller_Buttons_Generic member of the "emulated" 360 pad.
        /// Note: The current controller state must first be manually updated.
        /// </summary>
        /// <returns>True if said button is pressed, else false.</returns>
        bool GetButtonState(ControllerCommon.ControllerButtonsGeneric button);

        /// <summary>
        /// Retrieves the specific intensity in terms of how far/deep an axis is pressed in.
        /// The return value should be a floating point number between -100 and 100 float.
        /// Note: The current controller state must first be manually updated prior.
        /// </summary>
        /// <returns>The value of the axis between -100 and 100.</returns>
        /// <remarks>
        /// This does not take into account the destination axis and reads the value
        /// of the equivalent source axis. If the user has Left Stick mapped to e.g. Right Stick
        /// and you request the right stick axis, the value will return 0 (assuming right stick is centered).
        /// </remarks>
        float GetAxisState(ControllerCommon.ControllerAxis axis);

        /// <summary>
        /// Retrieves all of the individual button states as an array of boolean values.
        /// True if a button is pressed, false if a button is not pressed.
        /// Note: The current controller state must first be manually updated prior.
        /// </summary>
        /// <returns>Array of currently pressed/held in buttons.</returns>
        bool[] GetButtons();

        /// <summary>
        /// Updates the current state of the controller in question, retrieving the current button presses
        /// and axis measurements.
        /// </summary>
        void UpdateControllerState();

        /// <summary>
        /// Retrieves true if the input device is connected, else false.
        /// </summary>
        /// <returns>Retrieves true if the input device is connected, else false.</returns>
        bool IsConnected();

        /// <summary>
        /// Waits for the user to move an axis and retrieves the last pressed axis. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="mappingEntry">Specififies the mapping entry containing the axis to be remapped.</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if the axis has been successfully remapped, else false.</returns>
        bool RemapAxis(int timeoutSeconds, out float currentTimeout, AxisMappingEntry mappingEntry, ref bool cancellationToken);

        /// <summary>
        /// Waits for the user to press a button and retrieves the last pressed button. 
        /// Accepts any button as input, changes value of passed in button.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if the button has been successfully remapped, else false.</returns>
        bool RemapButtons(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap, ref bool cancellationToken);

        /// <summary>
        /// Retrieves the state of the whole controller in question.
        /// Using a combination of GetAxisState and GetButton state
        /// </summary>
        ControllerInputs.ControllerInputs GetControllerState();
    }
}
