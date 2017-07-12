using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicHeroes.Structs
{
    /// <summary>
    /// This class' purpose is to hold structs for more complex data types and ranges of values, such that they may be grouped and easily stored. The definitions of each variable can be found in SonicHeroes.Variables
    /// </summary>
    class SonicHeroes_Structs
    {
        /// <summary>
        /// A struct which can be used to hold information for character teleporting functions/savestating.
        /// </summary>
        public struct Characters_Addresses_Information
        {
            public float XVelocity;
            public float YVelocity;
            public float ZVelocity;
            public float XPosition;
            public float YPosition;
            public float ZPosition;
            public ushort XRotation;
            public ushort YRotation;
            public ushort ZRotation;
            public float XCharacterThickness;
            public float YCharacterThickness;
            public float TeamMatesFollowingSomething;
            public float TeamMatesFollowingSomething2;
            public float TeamMatesFollowingSomething3;
            /// <summary>
            /// Application specific - used for functionality of a teleportation system.
            /// </summary>
            public string CharacterWarpName;
        }

        /// <summary>
        /// Struct which may be used for storing a start position address for each stage.
        /// </summary>
        public struct Stage_Position_StartPositionStruct_1P2P
        {
            float XPosition;
            float YPosition;
            float ZPosition;
            ushort Direction;
            byte StartingMode;
            byte HoldTime;
        }

        /// <summary>
        /// Struct which may be used for storing the end position addresses for each stage.
        /// </summary>
        public struct Stage_Position_EndPositionStruct
        {
            float XPosition;
            float YPosition;
            float ZPosition;
            ushort CameraPitch;
            byte Unknown;
        }

        /// <summary>
        /// This contains all of the values used to calculate the camera to be displayed at the current time. Struct which may be used for camera savestating, saving and restoring camera properties or storing camera data from a specified time.
        /// </summary>
        public struct Game_Camera_Mathematics_Struct
        {
            byte IsCameraInAutomaticSegment1; // Can help unlock camera in locked segments
            byte IsCameraInAutomaticSegment2; // Can help unlock camera in locked segments
            float CameraXPosition_AngleCalculations; // Used for Angle Calculations
            float CameraYPosition_AngleCalculations; // Used for Angle Calculations
            float CameraZPosition_AngleCalculations; // Used for Angle Calculations
            int Unknown_AlmostAlwaysMax;
            float TriggerRotationCameraOffset; // When pressing L/R Triggers
            int Unknown_GenerallyNull;
            float AmIInFreeOrAutocam; // Not the setting in settings menu, this will let you control camera anywhere.
            ushort AmountOfFramesWithOwnCameraInAutocamSegments; // Where a camera is defined, this tracks the amount of time since you start to move camera yourself until it returns to game's autocam
        }

        /// <summary>
        /// This contains all of the values for the camera displayed at the current time. Struct which may be used for camera savestating, saving and restoring camera properties or storing camera data from a specified time.
        /// </summary>
        public struct Game_Camera_CurrentCamera_Struct
        {
            float CameraX_OpposingCharMovement; // Freezing makes camera oppose character 2D position on screen
            float CameraY_OpposintCharMovement; // Freezing makes camera oppose character 2D position on screen
            float CameraZ_OpposingCharMovement; // Freezing makes camera oppose character 2D position on screen
            float Camera_Unknown_X;
            float Camera_Unknown_Y;
            float Camera_Unknown_Z;
            float Camera_X; // Physical Camera Location | Functions when Cam Disabled
            float Camera_Y; // Physical Camera Location | Functions when Cam Disabled
            float Camera_Z; // Physical Camera Location | Functions when Cam Disabled
            ushort Camera_Angle_Vertical_BAMS; // Physical Camera Angle | Functions when Cam Disabled
            ushort Camera_Angle_Unknown_BAMS; // Physical Camera Angle | Functions when Cam Disabled
            ushort Camera_Angle_Horizontal_BAMS; // Physical Camera Angle | Functions when Cam Disabled
            float Camera_Unknown2_X;
            float Camera_Unknown2_Y;
            float Camera_Unknown2_Z;
            float Camera_ObjectPointedTowards_X; // Works when camera enabled
            float Camera_ObjectPointedTowards_Y; // Works when camera enabled
            float Camera_ObjectPointedTowards_Z; // Works when camera enabled
            byte Camera_Enabled; // Completely disables/enables camera
        }

        /// <summary>
        /// This struct contains the time trial limits for each level, and team as well as the number of minutes.
        /// </summary>
        public struct Stage_Missions_TimeTrialLimits_Struct
        {
            byte LevelID;
            byte TeamID;
            SByte NumberOfMinutes;
        }

        /// <summary>
        /// This struct contains the team dark robot challenge and team rose ring count for each stage.
        /// </summary>
        public struct Stage_Missions_TeamDark_TeamRose_Struct
        {
            //TODO: Test, see below
            byte NumberOfEnemiesToKill; // Could be an int, did not test.
            byte NumberOfEnemiesToKillVisual;
            byte RoseRingCountExtraMission; // Could be an int, did not test.
        }

        /// <summary>
        /// This struct can be used to store the values for Team Chaotix normal and Team Chaotix extra missions.
        /// </summary>
        public struct Stage_Missions_TeamChaotix_Properties_Offsets_Struct
        {
            byte AmountRequired;
            byte AmountRequiredExtra;
        }

        /// <summary>
        /// This struct will allow you to store the high scores required for each team and rank for each stage.
        /// </summary>
        public class Stage_HighScores_Addresses_Struct 
        {
            int StageID;
            Stage_HighScores_Addresses_TeamEntry[] HighScores = new Stage_HighScores_Addresses_TeamEntry[4]; // Each array entry is for a team.
        }

        /// <summary>
        /// This struct stores the individual rank requirements for each team. It is used by Stage_HighScores_Addresses_TeamEntry
        /// </summary>
        public struct Stage_HighScores_Addresses_TeamEntry
        {
            ushort DRank;
            ushort CRank;
            ushort BRank;
            ushort ARank;
        }

        /// <summary>
        /// This struct stores the unlock requirements for the two player events/modes in 2P Play.
        /// </summary>
        public struct Game_UnlockRequirements_TwoPlayer
        {
            byte ActionRace;
            byte Battle;
            byte SpecialStage;
            byte RingRace;
            byte BobsledRace;
            byte QuickRace;
            byte ExpertRace;
        }

    }
}
