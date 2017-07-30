using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicHeroes.Variables
{
        // Work In Progress!
        // For data types and structs in which many of the data here can be accessed, read or written, please see SonicHeroes_Structs.
        // Please consider contributing, populating this list is very time consuming.

        /// <summary>
        /// This class defines all of the memory and code variables for Sonic Heroes, allowing for easy access of anything to be changed as long as memory is appropriately written. All of these pointers are NOT relative to the main module. Thus you SHOULD NOT need to apply a 0x400000 base offset. For cases where the addresses lead to pointers and structs and/or have no entry for the actual values, see SonicHeroes_Structs, there will be a struct for the data to be read/written from/into.
        /// </summary>
        public static class SonicHeroesVariables
        {
            /// <summary>
            /// Various addresses which are used by the config tool for the Heroes Mod Loader as well as the stock launcher, since these are ineffective in the game, they are bundled here.
            /// </summary>
            public enum Launcher_Addresses
            {
                /// <summary>
                /// [Absolute Address! For Executable, NOT RAM] Flags, 8 bytes, see Microsoft: Window Styles, https://msdn.microsoft.com/en-us/library/windows/desktop/ms632600(v=vs.85).aspx
                /// </summary>
                Window_Style = 0x46D88,

                /// <summary>
                /// [Absolute Address! For Executable, NOT RAM] [Int] Width for the 1280x1024 stock 32 bit colour setting.
                /// </summary>
                Width_StockLauncher_1280_1024 = 0x3C9290,
                /// <summary>
                /// [Absolute Address! For Executable, NOT RAM] [Int] Height for the 1280x1024 stock 32 bit colour setting.
                /// </summary>
                Height_StockLauncher_1280_1024 = 0x3C9294,

                /// <summary>
                /// [Absolute Address! For Executable, NOT RAM] [Int] Width for the 1280x1024 fullscreen stock 32 bit colour setting.
                /// </summary>
                Width_StockLauncher_FullScreen_1280_1024 = 0x3C92E0,
                /// <summary>
                /// [Absolute Address! For Executable, NOT RAM] [Int] Height for the 1280x1024 fullscreen stock 32 bit colour setting.
                /// </summary>
                Height_StockLauncher_FullScreen_1280_1024 = 0x3C92E4,
            }

            /// <summary>
            /// Window border styles to choose from, you may use this to as reference to get the values to set the border style on the fly.
            /// </summary>
            public enum WINAPI_BorderStyles
            {
                Stock = 13107200, // 0x0000C800
                Borderless = -2147483648, // 0x00000080
                Resizable = 12845056, // 0x0000C400
                Resizable_Borderless = 262144, // 0x00000400
            }

            /// <summary>
            /// [IntPtr] Defines The Pointer from Which the Information about the current character may be obrained. For offsets see, Characters_PointerOffsets
            /// </summary>
            public enum Characters_Addresses
            {
                CurrentPlayerControlledCharacter_Pointer = 0x009CE820, // This is a pointer to the information about the current character the player is controlling.
            }

            /// <summary>
            /// The currently set controller buttons to be used for performing individual actions within Sonic Heroes. This is equal to your configuration in the launcher. See Player_Controller_Struct for named inputs.
            /// </summary>
            public enum Player_Controller_ControllerButtons
            {
                /// <summary>
                /// [ Byte(8) ] The buttons of the first controller assigned to actions.
                /// </summary>
                ControllerButtons_Player1 = 0x8CAEC4,

                /// <summary>
                /// [ Byte(8) ] The buttons of the first controller assigned to actions.
                /// </summary>
                ControllerButtons_Player2 = 0x8CAECC,
            }

            /// <summary>
            /// The offsets from the value obtained from the pointers to the characters in the Characters enumerable with offsets.
            /// </summary>
            public enum Characters_Addresses_Offsets
            {
                /// <summary>
                /// [Float] Defines the player's forward velocity relative to where he is facing.
                /// </summary>
                XVelocity = 0xDC,
                /// <summary>
                /// [Float] Defines the player's vertical velocity relative to where he is facing.
                /// </summary>
                YVelocity = 0xE0,
                /// <summary>
                /// [Float] Defines the player's sideways velocity relative to where he is facing.
                /// </summary>
                ZVelocity = 0xE4,
                /// <summary>
                /// [Float] Defines the player's X position.
                /// </summary>
                XPosition = 0xE8,
                /// <summary>
                /// [Float] Defines the player's Y position.
                /// </summary>
                YPosition = 0xEC,
                /// <summary>
                /// [Float] Defines the player's Z position.
                /// </summary>
                ZPosition = 0xF0,
                /// <summary>
                /// [Float] Defines the player's X rotation.
                /// </summary>
                XRotation = 0xF8,
                /// <summary>
                /// [Float] Defines the player's Y rotation.
                /// </summary>
                YRotation = 0xFC,
                /// <summary>
                /// [Float] Defines the player's Z rotation.
                /// </summary>
                ZRotation = 0x100,
                /// <summary>
                /// [Float] Defines the player's horizontal thickness.
                /// </summary>
                XCharacterThickness = 0x104,
                /// <summary>
                /// [Float] Defines the player's vertical thickness.
                /// </summary>
                YCharacterThickness = 0x108,
                /// <summary>
                /// [Float?] Affects how your teammates will follow the leader character.
                /// </summary>
                TeamMatesFollowingSomething = 0x10C,
                /// <summary>
                /// [Float?] Affects how your teammates will follow the leader character.
                /// </summary>
                TeamMatesFollowingSomething2 = 0x110,
                /// <summary>
                /// [Float?] Affects how your teammates will follow the leader character.
                /// </summary>
                TeamMatesFollowingSomething3 = 0x114
            }

            /// <summary>
            /// List of offsets defining the properties of the current game session.
            /// </summary>
            public enum Game_CurrentState
            {
                /// <summary>
                /// [Byte] Toggles camera movement.
                /// </summary>
                CameraMovementEnabled = 0x00A69880,
                /// <summary>
                /// [Int] Used for storing the real world time (yes for real). This is used for frame pacing.
                /// </summary>
                RealWorldTime = 0x00A77690,
                /// <summary>
                /// [Byte] 0 To Disable Effects, 1 To Enable.
                /// </summary>
                ShaderEffectsEnabled = 0x008E2050, // 0 to disable FX
                /// <summary>
                /// [Byte] 1 if inside any level, 0 or 255 otherwise.
                /// </summary>
                CurrentlyInLevel = 0x008E2028, // 1 if in a level, 255 or 0 if in menus
                /// <summary>
                /// [Byte] 1 if paused, 0 if not paused
                /// </summary>
                IsIngamePauseMenuOpen = 0x008d6708,
                /// <summary>
                /// [Byte] By default multiplies score requirements by 100.
                /// </summary>
                ScoreRequirementMultiplier = 0x435C45, // Multiplies score requirements by 100
                /// <summary>
                /// [Byte] Toggles with whether the game limits its framerate when unfocused.
                /// </summary>
                NonFocusedFPSCapToggle = 0x007C9284, // This decides whether to limit the FPS of the game when the game is not the active window.
                /// <summary>
                /// [Byte] Toggles whether the game will run the games played in two player mode at half the framerate. Enable (Default): 0x40, Disable: 0x90
                /// </summary>
                TwoPlayer_Framerate_Limiter_Toggle = 0x802D07,
            }

            /// <summary>
            /// What you basically see in your HUD ingame.
            /// </summary>
            public enum Player_CurrentState
            {
                /// <summary>
                /// [Short] Maximum 999 in HUD.
                /// </summary>
                AmountOfRings = 0x9DD70C,
            }
           
            /// <summary>
            /// Defines the memory addresses holding the current stage and stage properties in memory.
            /// </summary>
            public enum Stage_CurrentStage
            {
                /// <summary>
                /// [Byte] This defines the level chosen by the player in any menu. It dictates the level plate/screen shown during loading.
                /// </summary>
                PlayerStageChoiceMainMenu = 0x008D6720,
                // This is the level the player selects in Challenge Mode or any other Main Menu Screens. 
                // It also dictates which level plate is shown onscreen.

                /// <summary>
                /// [Byte] This is the stage which will be loaded ingame.
                /// </summary>
                PlayerStageIDToLoad = 0x008D6710, // This is the Stage that will actually be loaded ingame.
            }

            /// <summary>
            /// All of the individual IDs used for each stage. Each is a [Byte]
            /// </summary>
            public enum Stage_StageIDs
            {
                TestLevel = 0x00,
                Unknown = 0x01,
                SeasideHill = 0x2,
                OceanPalace = 0x3,
                GrandMetropolis = 0x4,
                PowerPlant = 0x5,
                CasinoPark = 0x6,
                BingoHighway = 0x7,
                RailCanyon = 0x8,
                BulletStation = 0x9,
                FrogForest = 10,
                LostJungle = 11,
                HangCastle = 12,
                MysticMansion = 13,
                EggFleet = 14,
                FinalFortress = 15,

                //TODO: Verify those below
                EggHawk = 16,
                TeamBattle1 = 17,
                RobotCarnival = 18,
                EggAlbatross = 19,
                TeamBattle2 = 20,
                RobotStorm = 21,
                EggEmperor = 22,
                MetalMadness = 23,
                MetalOverlord = 24,
                SeaGate = 25,
                SeasideCourse = 26,
                CityCourse = 27,
                CasinoCourse = 28,
                BonusStage1 = 29,
                BonusStage2 = 30,
                BonusStage3 = 31,
                BonusStage4 = 32,
                BonusStage5 = 33,
                BonusStage6 = 34,
                BonusStage7 = 35,
                RailCanyonChaotix = 36,
                SeasideHill2PRace = 37,
                GrandMetropolis2PRace = 38,
                BINGOHighway2PRace = 39,
                CityTopBattle = 40,
                CasinoRingBattle = 41,
                TurtleShellBattle = 42,
                EggTreat = 43,
                PinballMatch = 44,
                HotElevator = 45,
                RoadRock = 46,
                MadExpress = 47,
                TerrorHall = 48,
                RailCanyonExpert = 49,
                FrogForestExpert = 50,
                EggFleetExpert = 51,
                EmeraldChallenge1 = 52,
                EmeraldChallenge2 = 53,
                EmeraldChallenge3 = 54,
                EmeraldChallenge4 = 55,
                EmeraldChallenge5 = 56,
                EmeraldChallenge6 = 57,
                EmeraldChallenge7 = 58,
                SpecialStageMultiplayer1 = 59,
                SpecialStageMultiplayer2 = 60,
                SpecialStageMultiplayer3 = 61,
            }

            /// <summary>
            /// For the offsets, please see GameMenus_MenuEntries_Offsets
            /// </summary>
            public enum Game_GameMenuScreens
            {
                // Pointer
                /// <summary>
                /// [Int] This is the pointer from which start the entries for the individual main menu selections and animations for each main menu item. See Game_GameMenuScreens_MenuEntries_Offsets.
                /// </summary>
                MenuEntries_BasePointer = 0x00A777B4,

                // Unsigned Byte
                /// <summary>
                /// [Byte] The index of the currently selected item in the pause menu.
                /// </summary>
                PauseMenu_Selection = 0x008D6930,
                /// <summary>
                /// [Byte] The index of the currently selected item in the options submenu of the pause menu.
                /// </summary>
                PauseMenu_Options_Selection = 0x008D6934,
                /// <summary>
                /// [Byte] The index of the currently selected item in the Controller Configuration submenu of the pause menu.
                /// </summary>
                PauseMenu_Options_GamePadConfiguration_Selection = 0x008D6938,
                /// <summary>
                /// [Byte] The index of the camera mode selection menu in the pause menu.
                /// </summary>
                PauseMenu_Options_CameraMode_Selection = 0x008D693C,
            }

            // Type: Unsigned Byte
            public enum Game_GameMenuScreens_MenuEntries_Offsets // These are offsets for GameMenus enumerable MenuAnimationEntries_BasePointer
            {
                Main_Menu_Animation = 0x124, // Fun fact, freeze this, press an unintended button and next menu will load with theme of menu set!
                Main_Menu_Selection = 0x468,
                
                CG_Menu_Animation = 0x110, // Shared with 1P Play
                CG_Menu_Selection = 0x198,
                OnePlayer_Play_Animation = 0x110,
                OnePlayer_Play_Selection = 0x1AC,

                Extra_Menu_Animation = 0x118, // Shared with Vibration Setting Menu
                Extra_Menu_Selection = 0x2C4,
                Vibration_Setting_Current_Choice = 0x23C,

                Language_Setting_Animation = 0x11C,
                Language_Setting_Selection = 0x2C8,

                File_Select_Selection_And_Animation = 0x130,
                Options_Menu_Selection_And_Animation = 0x3E0,

                Audio_Menu_Selection = 0x194,
                Audio_Menu_Animation = 0x228,

                Audio_Menu_Subcategory_Selection = 0x220,
                Audio_Menu_Subcategory_Animation = 0x22C,

                TwoPlayer_Menu_Selection = 0x198,
                TwoPlayer_Menu_Animation = 0x2B8,

                TwoPlayer_P1_Team_Menu_Selection = 0x2C4,
                TwoPlayer_P2_Team_Menu_Selection = 0x3DC,

                TwoPlayer_P1_Team_Menu_Animation = 0x350,
                TwoPlayer_P2_Team_Menu_Animation = 0x2C8,

                TwoPlayer_Stage_Selection = 0x2B0,
                TwoPlayer_Stage_Animation = 0x2C0,

                ChallengeMode_Selection = 0x194,
                ChallengeMode_Animation = 0x2B4,

                ChallengeMode_Team_Selection = 0x220,
                ChallengeMode_Team_Animation = 0x2B8,

                ChallengeMode_Mission_Selection = 0x2AC,
                ChallengeMode_Mission_Animation = 0x2BC,

                Story_Select_Menu_Selection = 0x194, // Shared with Challenge Mode
                Story_Select_Menu_Animation = 0x19C,

                Story_Select_Submenu_Selection = 0x234,
                Story_Select_Submenu_Animation = 0x1A0,

                File_Select_Start_Select_Cancel_Selection = 0x518,
                File_Select_Start_Select_Cancel_Selection_Animation = 0x51A,

                Exit_Game_Yes_No_Menu_Selection = 0x678,
                Super_Hard_Mode_Menu_Selection = 0x678,
            }

            /// <summary>
            /// Defines the values the game uses to determine the current camera state, from angle to position.
            /// </summary>
            public enum Game_Camera_Mathematics
            {
                /// <summary>
                /// [Byte] Defines whether camera is in an automatic segment. Can help unlock camera in locked segments.
                /// </summary>
                IsCameraInAutomaticSegment1 = 0x00A606B8, // Can help unlock camera in locked segments
                /// <summary>
                /// [Byte] Defines whether camera is in an automatic segment. Can help unlock camera in locked segments.
                /// </summary>
                IsCameraInAutomaticSegment2 = 0x00A606BC, // Can help unlock camera in locked segments
                /// <summary>
                /// [Float] The X Position defining a point the camera takes into account when calculating which angle it should point at.
                /// </summary>
                CameraXPosition_AngleCalculations = 0x00A606C0, // Used for Angle Calculations
                /// <summary>
                /// [Float] The Y Position defining a point the camera takes into account when calculating which angle it should point at.
                /// </summary>
                CameraYPosition_AngleCalculations = 0x00A606C4, // Used for Angle Calculations
                /// <summary>
                /// [Float] The Z Position defining a point the camera takes into account when calculating which angle it should point at.
                /// </summary>
                CameraZPosition_AngleCalculations = 0x00A606C8, // Used for Angle Calculations
                Unknown_AlmostAlwaysMax = 0x00A606D0,
                /// <summary>
                /// [Float] The distance from the centre of oscillation/rotation when the player rotates the camera using the L/R triggers.
                /// </summary>
                TriggerRotationCameraOffset = 0x00A606F0, // When pressing L/R Triggers
                Unknown_GenerallyNull = 0x00A606F4,
                /// <summary>
                /// [Byte] Not the setting in the settings menu. This defines whether the player can freely control the camera.
                /// </summary>
                AmIInFreeOrAutocam = 0x00A606F4, // Not the setting in settings menu, this will let you control camera anywhere.
                /// <summary>
                /// Where a camera is defined, this tracks the amount of time since you start to move camera yourself until it returns to game's autocam.
                /// </summary>
                AmountOfFramesWithOwnCameraInAutocamSegments = 0x00A60700,
            }

            /// <summary>
            /// Defines the current state of the camera.
            /// </summary>
            public enum Game_Camera_CurrentCamera
            {
                /// <summary>
                /// [Float] Value declaring how much the camera shifts during movement, inversely proportional to the character's movement.
                /// </summary>
                CameraX_OpposingCharMovement = 0x00A60BF0,
                /// <summary>
                /// [Float] Value declaring how much the camera shifts during movement, inversely proportional to the character's movement.
                /// </summary>
                CameraY_OpposingCharMovement = 0x00A60BF4,
                /// <summary>
                /// [Float] Value declaring how much the camera shifts during movement, inversely proportional to the character's movement.
                /// </summary>
                CameraZ_OpposingCharMovement = 0x00A60BF8,
                Camera_Unknown_X = 0x00A60BFC,
                Camera_Unknown_Y = 0x00A60C00,
                Camera_Unknown_Z = 0x00A60C04,
                /// <summary>
                /// [Float] Physical location X of the camera.
                /// </summary>
                Camera_X = 0x00A60C30,
                /// <summary>
                /// [Float] Physical location Y of the camera.
                /// </summary>
                Camera_Y = 0x00A60C34,
                /// <summary>
                /// [Float] Physical location Z of the camera.
                /// </summary>
                Camera_Z = 0x00A60C38,
                /// <summary>
                /// [Short] Vertical Angle of the Camera, Defined in Binary Angle Measurement System.
                /// </summary>
                Camera_Angle_Vertical_BAMS = 0x00A60C3C,
                /// <summary>
                /// [Short] Unknown Angle of the Camera, Defined in Binary Angle Measurement System.
                /// </summary>
                Camera_Angle_Unknown_BAMS = 0x00A60C3E,
                /// <summary>
                /// [Short] Horizontal Angle of the Camera, Defined in Binary Angle Measurement System.
                /// </summary>
                Camera_Angle_Horizontal_BAMS = 0x00A60C40,
                Camera_Unknown2_X = 0x00A60C48,
                Camera_Unknown2_Y = 0x00A60C4C,
                Camera_Unknown2_Z = 0x00A60C50,
                /// <summary>
                /// [Float] X Coordinate of the object/point the camera is pointing towards.
                /// </summary>
                Camera_ObjectPointedTowards_X = 0x00A60C54,
                /// <summary>
                /// [Float] Y Coordinate of the object/point the camera is pointing towards.
                /// </summary>
                Camera_ObjectPointedTowards_Y = 0x00A60C58,
                /// <summary>
                /// [Float] Z Coordinate of the object/point the camera is pointing towards.
                /// </summary>
                Camera_ObjectPointedTowards_Z = 0x00A60C5C,
                /// <summary>
                /// [Byte] Disables/Enables the camera.
                /// </summary>
                Camera_Enabled = 0x00A69880,
            }

            /// <summary>
            /// Addresses for the time trial limits | Team Sonic. See Stage_Missions_TimeTrialLimits_Offsets for the offsets.
            /// </summary>
            public enum Stage_Missions_TimeTrialLimits
            {
                SeasideHill = 0x7C6A68,
                OceanPalace = 0x7C6A70,
                GrandMetropolis = 0x7C6A78,
                PowerPlant = 0x7C6A80,
                CasinoPark = 0x7C6A88,
                BingoHighway = 0x7C6A90,
                RailCanyon = 0x7C6A98,
                BulletStation = 0x7C6AA0,
                FrogForest = 0x7C6AA8,
                LostJungle = 0x7C6AB0,
                HangCastle = 0x7C6AB8,
                MysticMansion = 0x7C6AC0,
                EggFleet = 0x7C6AC8,
                FinalFortress = 0x7C6AD0,
                GrandMetropolis_Chaotix = 0x7C6AD8,
                RailCanyon_Chaotix = 0x7C6AE0,
                RailCanyon_ChaotixVersion_Chaotix = 0x7C6AE8,
                FrogForest_Chaotix = 0x7C6AF0,
            }

            /// <summary>
            /// Offsets for the time trial limits for Team Sonic.
            /// </summary>
            public enum Stage_Missions_TimeTrialLimits_Offsets
            {
                /// <summary>
                /// [Byte] See_StageID
                /// </summary>
                LevelID = 0x0,
                /// <summary>
                /// [Byte] In Order: Sonic/Dark/Rose/Chaotix
                /// </summary>
                TeamID = 0x4,
                /// <summary>
                /// [Byte] Maximum 128 Minutes
                /// </summary>
                NumberOfMinutes = 0x5,
            }

            /// <summary>
            /// Custom options for Team Rose & Dark extra missions.
            /// </summary>
            public enum Stage_Missions_TeamDark_TeamRose
            {
                /// <summary>
                /// [Byte] The amount of enemies which Team Dark needs to kill in the extra missions.
                /// </summary>
                NumberOfEnemiesToKill = 0x5A9993,
                /// <summary>
                /// [Byte] The amount of enemies which Team Dark needs to kill in the extra missions on the HUD.
                /// </summary>
                NumberOfEnemiesToKillVisual = 0x5A9984,
                /// <summary>
                /// [Byte] The amount of rings Team Rose needs to collect to complete an extra mission.
                /// </summary>
                RoseRingCount = 0x5A9E0F,
            }

            /// <summary>
            /// Addresses for the properties for Team Chaotix missions!
            /// </summary>
            public enum Stage_Missions_TeamChaotix_Properties_BaseAddresses
            {
                /// <summary>
                /// [Int] The amount of crabs to collect in Seaside Hill. Note: For the extra mission, the game automatically detects the amount of placed chao. 
                /// </summary>
                HermitCrabs = 0x8BF420,
                /// <summary>
                /// [Int] The amount of chao to collect in Ocean Palace. 
                /// </summary>
                ChaoRescue = 0x8BF428,
                /// <summary>
                /// [Int] The amount of Robots to kill in Grand Metropolis.
                /// </summary>
                KillRobots = 0x8BF430,
                /// <summary>
                /// [Int] The amount of Gold Turtles to kill in Power Plant.
                /// </summary>
                DestroyGoldTurtles = 0x8BF438,
                
                /// <summary>
                /// [Int] This property is unused, for the used version, see RingCollectionNew
                /// </summary>
                RingCollection = 0x8BF440, 
                /// <summary>
                /// [Int] The amount of rings Chaotix needs to collect to beat Casino Park.
                /// </summary>
                RingCollectionNew = 0x5A9DE5, //Used

                /// <summary>
                /// [Int] The amount of casino chips Chaotix needs to collect. Extra: Amount of objects placed is automatically detected.
                /// </summary>
                CasinoChipCollection = 0x8BF448,

                /// <summary>
                /// [Int] The amount of time Chaotix has to reach the terminal station (Unused).
                /// </summary>
                ReachTheTerminal = 0x8BF450, // Unused

                /// <summary>
                /// [Int] The amount of capsules Chaotix need to destroy to beat Bullet Station. Extra: Amount of objects placed is automatically detected.
                /// </summary>
                DestroyTheCapsules = 0x8BF458,

                /// <summary>
                /// [Int] Team Chaotix: Frog Forest TimeLimit (Unused) 
                /// </summary>
                FinishUndetected = 0x8BF460, // Unused

                /// <summary>
                /// [Int] The amount of chao Chaotix needs to collect to beat Lost Jungle. Extra: Amount of objects placed is automatically detected.
                /// </summary>
                CollectTheChao = 0x8BF468,

                /// <summary>
                /// [Int] The amount of castle keys Chaotix needs to collect to beat Hang Castle. (Unused)
                /// </summary>
                CollectTheCastleKeys = 0x8BF470, // Unused

                /// <summary>
                /// [Int] The amount of torches Chaotix must blow out to beat Mystic Mansion (Unused).
                /// </summary>
                LightTheMansionTorches = 0x8BF478, // Unused

                /// <summary>
                /// [Int] The amount of time Chaotix has for Egg Fleet. (Unused)
                /// </summary>
                EscapeTheEggmanFleet = 0x8BF480, // Unused

                /// <summary>
                /// [Int] The amount of Final Fortress Keys Chaotix must obtain to beat Final Fortress.
                /// </summary>
                CollectTheEggmanKeys = 0x8BF488,
            }

            /// <summary>
            /// Offsets for Team Chaotix Extra missions addresses.
            /// </summary>
            public enum Stage_Missions_TeamChaotix_Properties_Offsets
            {
                /// <summary>
                /// [Int] Amount of items/objects/time allowed for the normal missions.
                /// </summary>
                AmountRequired = 0x0,
                /// <summary>
                /// [Int] Amount of items/objects/time allowed for the extra missions.
                /// </summary>
                AmountRequiredExtra = 0x4,
            }

            /// <summary>
            /// [Byte] | These values in the addresses declare the mission type for Team Chaotix' missions for each individual stage | See Stage_Missions_TeamChaotix_MissonType_MissionTypes for valid entries.
            /// </summary>
            public enum Stage_Missions_TeamChaotix_MissonType_Addresses
            {
                SeasideHill = 0x4020AC,
                OceanPalace = 0x4020AD,
                GrandMetropolis = 0x4020AE,
                PowerPlant = 0x4020AF,
                CasinoPark = 0x4020B0,
                BingoHighway = 0x4020B1,
                RailCanyon = 0x4020B2,
                BulletStation = 0x4020B3,
                FrogForest = 0x4020B4,
                LostJungle = 0x4020B5,
                HangCastle = 0x4020B6,
                MysticMansion = 0x4020B7,
                EggFleet = 0x4020B8,
                FinalFortress = 0x4020B9,
            }

            /// <summary>
            /// These values are all of the individual values which can be assigned to a Team Chaotix stage to define a mission type. See TeamChaotixMissionTypeAddresses
            /// </summary>
            public enum Stage_Missions_TeamChaotix_MissonType_MissionTypes
            {
                /// <summary>
                /// [Normal | Extra Mission] Conditions: Item Collect (Half/EXE Defined) | Item Collect (Full) 
                /// </summary>
                ItemCollect = 0x00,
                /// <summary>
                /// [Normal | Extra Mission] Conditions: Item Collect | Item Collect (without detection by Robots) 
                /// </summary>
                ItemCollectExtraNoDetection = 0x01,
                /// <summary>
                /// [Normal | Extra Mission] Conditions: Robot Cleanup | Robot Cleanup (with time limit)  
                /// </summary>
                RobotCleanup = 0x02,
                /// <summary>
                /// [Normal | Extra Mission] Conditions: Ring Collect | Ring Collect  
                /// </summary>
                RingCollect = 0x03,
                /// <summary>
                /// [Normal | Extra Mission] Conditions: Regular Mission | Regular Mission (with time limit)  
                /// </summary>
                RegularMission = 0x04,
                /// <summary>
                /// [Normal | Extra Mission] Conditions: Regular Mission (without detection by Frogs) | Regular Mission (with time limit, but without detection by Frogs)  
                /// </summary>
                RegularMissionNoFrogs = 0x05,
                /// <summary>
                /// [Normal | Extra Mission] Conditions: Regular Mission (without detection by Robots) | Regular Mission (with time limit, but without detection by Robots)  
                /// </summary>
                RegularMissionNoRobots = 0x06,
            }

            /// <summary>
            /// [Int] These values under the addresses define the goal ring state for each goal ring for a Team Chaotix Stage. 0 = Goal Ring, 1 = Restart Ring
            /// </summary>
            public enum Stage_Missions_TeamChaotix_GoalRingState_Addresses
        {
                TestLevel = 0x7D0C50,
                SeasideHill = 0x7D0C54,
                OceanPalace = 0x7D0C58,
                GrandMetropolis = 0x7D0C5C,
                PowerPlant = 0x7D0C60,
                CasinoPark = 0x7D0C64,
                BingoHighway = 0x7D0C68,
                RailCanyon = 0x7D0C6C,
                BulletStation = 0x7D0C70,
                FrogForest = 0x7D0C74,
                LostJungle = 0x7D0C78,
                HangCastle = 0x7D0C7C,
                MysticMansion = 0x7D0C80,
                EggFleet = 0x7D0C84,
                FinalFortress = 0x7D0C88,

                //TODO: Verify those below
                EggHawk = 0x7D0C8C,
                TeamBattle1 = 0x7D0C90,
                RobotCarnival = 0x7D0C94,
                EggAlbatross = 0x7D0C98,
                TeamBattle2 = 0x7D0C9C,
                RobotStorm = 0x7D0CA0,
                EggEmperor = 0x7D0CA4,
                MetalMadness = 0x7D0CA8,
                MetalOverlord = 0x7D0CAC,
                SeaGate = 0x7D0CB0,
                SeasideCourse = 0x7D0CB4,
                CityCourse = 0x7D0CB8,
                CasinoCourse = 0x7D0CBC,
                BonusStage1 = 0x7D0CC0,
                BonusStage2 = 0x7D0CC4,
                BonusStage3 = 0x7D0CC8,
                BonusStage4 = 0x7D0CCA,
                BonusStage5 = 0x7D0CD0,
                BonusStage6 = 0x7D0CD4,
                BonusStage7 = 0x7D0CD8,
                RailCanyonChaotix = 0x7D0CDC,
                SeasideHill2PRace = 0x7D0CE0,
                GrandMetropolis2PRace = 0x7D0CE4,
                BINGOHighway2PRace = 0x7D0CE8,
                CityTopBattle = 0x7D0CEC,
                CasinoRingBattle = 0x7D0CF0,
                TurtleShellBattle = 0x7D0CF4,
                EggTreat = 0x7D0CF8,
                PinballMatch = 0x7D0CFC,
                HotElevator = 0x7D0D00,
                RoadRock = 0x7D0D04,
                MadExpress = 0x7D0D08,
                TerrorHall = 0x7D0D0C,
                RailCanyonExpert = 0x7D0D10,
                FrogForestExpert = 0x7D0D14,
                EggFleetExpert = 0x7D0D18,
                EmeraldChallenge1 = 0x7D0D1C,
                EmeraldChallenge2 = 0x7D0D20,
                EmeraldChallenge3 = 0x7D0D24,
                EmeraldChallenge4 = 0x7D0D28,
                EmeraldChallenge5 = 0x7D0D2C,
                EmeraldChallenge6 = 0x7D0D30,
                EmeraldChallenge7 = 0x7D0D34,
                SpecialStageMultiplayer1 = 0x7D0D38,
                SpecialStageMultiplayer2 = 0x7D0D3C,
                SpecialStageMultiplayer3 = 0x7D0D40,
            }

            /// <summary>
            /// [Int] The individual Goal Ring States for Team Chaotix Missions
            /// </summary>
            public enum Stage_Missions_TeamChaotix_GoalRingState_GoalRingStates
            {
                GoalRing = 0x00,
                RestartRing = 0x01,
            }

            /// <summary>
            /// At these addresses are defined the teams which each team will fight against in Team Battle. For offsets see Stage_TeamBattle_Stages_Opponents_Offsets, for Teams see Stage_TeamBattle_Stages_Opponents_Teams.
            /// </summary>
            public enum Stage_TeamBattle_Stages_Opponents_Addresses
            {
                /// <summary>
                /// After Power Plant
                /// </summary>
                CityTopLevel = 0x8DD514,
                /// <summary>
                /// It'll be a date to DIE for!
                /// </summary>
                ForestLevel = 0x8DD5A0,
            }

            /// <summary>
            /// Offsets for Stage_TeamBattle_Stages_Opponents_Addresses. Each defines the team which will fight against player player is Team X.
            /// </summary>
            public enum Stage_TeamBattle_Stages_Opponents_Offsets
            {
                TeamVersusSonic = 0x00,
                TeamVersusDark = 0x04,
                TeamVersusRose = 0x08,
                TeamVersusChaotix = 0x0C,
            }

            /// <summary>
            /// Teams which are valid entries for the teams the player will fight against as Team X for each Team Battle, for stages see TStage_TeamBattle_Stages_Opponents_Addresses.
            /// </summary>
            public enum Stage_TeamBattle_Stages_Opponents_Teams
            {
                /// <summary>
                /// [Int]
                /// </summary>
                TeamSonic = 0x00,
                /// <summary>
                /// [Int]
                /// </summary>
                TeamDark = 0x01,
                /// <summary>
                /// [Int]
                /// </summary>
                TeamRose = 0x02,
                /// <summary>
                /// [Int]
                /// </summary>
                TeamChaotix = 0x03,
            }

            // These are pointers to animation files, swap pointers to swap animations
            /// <summary>
            /// After the stage loads, animation files to the boss HUD information loads on the screen. This is the pointer list for each team's animation. Swap those as wanted. 
            /// </summary>
            public enum Stage_TeamBattle_AnimationHUDPointers_Addresses
            {
                CityTopTeamRosePointer = 0x8DCD38,
                CityTopTeamChaotixPointer = 0x8DCD3C,
                CityTopTeamSonicPointer = 0x8DCD40,
                CityTopTeamDarkPointer = 0x8DCD44,
                ForestTeamRosePointer = 0x8DCD48,
                ForestTeamChaotixPointer = 0x8DCD4C,
                ForestTeamSonicPointer = 0x8DCD50,
                ForestTeamDarkPointer = 0x8DCD54,
            }

            /// <summary>
            /// After the stage loads, animation files to the boss HUD information loads on the screen. This is the pointer list for the TEXT for each team's animation. Swap those as wanted. 
            /// </summary>
            public enum Stage_TeamBattle_AnimationHUDPointers_Text_Addresses
            {
                CityTopTeamRosePointer = 0x8DCF84,
                CityTopTeamChaotixPointer = 0x8DCF88,
                CityTopTeamSonicPointer = 0x8DCF8C,
                CityTopTeamDarkPointer = 0x8DCF90,
                ForestTeamRosePointer = 0x8DCF94,
                ForestTeamChaotixPointer = 0x8DCF98,
                ForestTeamSonicPointer = 0x8DCF9C,
                ForestTeamDarkPointer = 0x8DCFA0,
            }

            /// <summary>
            /// For display on menus such as the challenge menu, these are addresses point to a texture to be loaded on team selection. 
            /// </summary>
            public enum Game_GameMenuScreens_ChallangeMode_TeamBattle_Texture_TextPointers
            {
                CityTopTeamRosePointer = 0x44BCAE,
                CityTopTeamChaotixPointer = 0x44BCBF,
                CityTopTeamSonicPointer = 0x44BCD1,
                CityTopTeamDarkPointer = 0x44BCE3,
                ForestTeamRosePointer = 0x44BD1D,
                ForestTeamChaotixPointer = 0x44BD2C,
                ForestTeamSonicPointer = 0x44BD3A,
                ForestTeamDarkPointer = 0x44BD49,
            }

            // Amount of entries is until an invalid level is met
            /// <summary>
            /// See Stage_Position_Team_StartPosition_Offsets_1P2P for offsets. The start positions for each stage for one player modes. Seaside Hill and length of each stage entry is given, use those to get any other level as required, I'm lazy.
            /// </summary>
            public enum Stage_Position_StartPosition_OnePlayer
            {
                /// <summary>
                /// First stage entry (no test level here :/)
                /// </summary>
                SeasideHill = 0x7C2FC8, // 1st stage
                /// <summary>
                /// [Int] The stage ID for each individual stage entry, see StageIDs.
                /// </summary>
                StageEntry_StageID_Offset = 0x0,  
                /// <summary>
                /// In order from 0x00-0x05: Team Sonic / Team Dark / Team Rose / Team Chaotix / Super Hard Mode
                /// </summary>
                NumberOfTeamEntries = 0x5, // Including Super Hard.
                /// <summary>
                /// Length of each team entry, add this multiplied by the team entry 1-5 to the stage base address + 0x4 (for the stage entry) get your offset for the team entry as defined in Stage_Position_Team_StartPosition_Offsets_1P2P.
                /// </summary>
                LengthOfTeamEntry = 0x1C,
                /// <summary>
                /// Length of each stage entry. Add this to the stage base address to get address for next stage.
                /// </summary>
                LengthOfStruct = 0x90, // Used in validation Math.
            }

            /// <summary>
            /// Multiplier for calculating the offsets for each team entry, applies to bragging positions, start and ending positions. Multiply the value of the team here by the length of each team entry as defined for a position and add the base address of stage + 0x4 for each team entry for start/end/bragging position.
            /// </summary>
            public enum Stage_Position_Team_OffsetMultipliers
            {
                TeamSonic = 0x01,
                TeamDark = 0x02,
                TeamRose = 0x03,
                TeamChaotix = 0x04,
                SuperHardMode = 0x05
            }

            /// <summary>
            /// Offset for each one player or two player stage entry for starting positions for each individual stage.
            /// </summary>
            public enum Stage_Position_Team_StartPosition_Offsets_1P2P
            {
                /// <summary>
                /// [Float] X Stage Starting Position
                /// </summary>
                XPosition = 0x0,
                /// <summary>
                /// [Float] Y Stage Starting Position
                /// </summary>
                YPosition = 0x4,
                /// <summary>
                /// [Float] Z Stage Starting Position
                /// </summary>
                ZPosition = 0x8,
                /// <summary>
                /// [Short] Starting Direction/Pitch. Defined using Binary Angle Measurement System.
                /// </summary>
                Direction = 0xC,
                /// <summary>
                /// [Byte] Starting Mode, Stage_Position_Team_StartPosition_StartingModes for options. 
                /// </summary>
                StartingMode = 0x14,
                /// <summary>
                /// [Short] The amount of time the player has no control over the character after spawn (e.g. Running on Seaside Hill).
                /// </summary>
                HoldTime = 0x18,
            }

            /// <summary>
            /// A selection of starting types/modes for One/Two player.
            /// </summary>
            public enum Stage_Position_Team_StartPosition_StartingModes
            {
                /// <summary>
                /// Normal Start Type, no override!
                /// </summary>
                Normal = 0x0,
                /// <summary>
                /// Character starts off running!
                /// </summary>
                Running = 0x1,
                /// <summary>
                /// Character starts off on a rail!
                /// </summary>
                Rail = 0x2
            }

            /// <summary>
            /// See Stage_Position_EndPosition_OnePlayer_Offsets for offsets. The start positions for each stage for two player modes. Seaside Hill and length of each stage entry is given, use those to get any other level as required, I'm lazy.
            /// </summary>
            public enum Stage_Position_EndPosition_OnePlayer
            {
                /// <summary>
                /// First stage entry (no test level here :/)
                /// </summary>
                SeasideHill = 0x7C45B8,
                /// <summary>
                /// [Int] The stage ID for each individual stage entry, see StageIDs.
                /// </summary>
                StageEntry_StageID_Offset = 0x0,
                /// <summary>
                /// In order from 0x00-0x05: Team Sonic / Team Dark / Team Rose / Team Chaotix / Super Hard Mode
                /// </summary>
                NumberOfTeamEntries = 0x5, // Including Super Hard
                /// <summary>
                /// Length of each team entry, add this multiplied by the team entry 1-5 to the stage base address + 0x4 (for the stage entry) get your offset for the team entry as defined in Stage_Position_EndPosition_OnePlayer_Offsets.
                /// </summary>
                LengthOfTeamEntry = 0x14,
                /// <summary>
                /// Length of each stage entry. Add this to the stage base address to get address for next stage.
                /// </summary>
                LengthOfStruct = 0x68, // Used in validation Math.
            }

            /// <summary>
            /// Offsets for the end position addresses from each stage's base address as defined by EndPositionAddressOffsets + StageID*LengthOfStruct
            /// </summary>
            public enum Stage_Position_EndPosition_OnePlayer_Offsets
            {
                /// <summary>
                /// [Float] X Stage Ending Position
                /// </summary>
                XPosition = 0x0,
                /// <summary>
                /// [Float] Y Stage Ending Position
                /// </summary>
                YPosition = 0x4,
                /// <summary>
                /// [Float] Z Stage Ending Position
                /// </summary>
                ZPosition = 0x8,
                /// <summary>
                /// [Short] Camera Angle/Pitch, Defined under Binary Angle Measurement System.
                /// </summary>
                CameraPitch = 0xC,
                /// <summary>
                /// [Short] Unknown, always FF FF ?
                /// </summary>
                Unknown = 0xE,
            }

            /// <summary>
            /// Starting positions for two player matches. The struct of this is the same as the one player equivalent. Except the limits are different.
            /// </summary>
            public enum Stage_Position_StartPosition_TwoPlayer
            {
                /// <summary>
                /// The first stage/base address.
                /// </summary>
                SeasideHill = 0x7C5E18,
                /// <summary>
                /// [Int] The stage ID for each individual stage entry, see StageIDs.
                /// </summary>
                StageEntry_StageID_Offset = 0x0,
                /// <summary>
                /// Teams: Player One/Player Two
                /// </summary>
                NumberOfTeamEntries = 0x2, // For each player
                /// <summary>
                /// Lemgth of each stage entry. Add this to the stage base address + 0x4 (for the stage entry) to get address for next stage.
                /// </summary>
                LengthOfStruct = 0x3C, // Used in validation Math.
            }

            /// <summary>
            /// Bragging positions for two player matches. The struct of this is the same as the one player stage ending position struct.
            /// </summary>
            public enum Stage_Position_StartPosition_TwoPlayer_BraggingPosition
            {
                /// <summary>
                /// The first stage/base address.
                /// </summary>
                SeasideHill = 0x7C6380, // 1st stage
                /// <summary>
                /// Teams: In Order: Sonic/Dark/Rose/Chaotix
                /// </summary>
                NumberOfTeamEntries = 0x4, // For each team
                /// <summary>
                /// Lemgth of each stage entry.
                /// </summary>
                LengthOfStruct = 0x54, // Used in validation Math.
            }

            /// <summary>
            /// [Struct For Each Stage: Stage_HighScores_Addresses_Struct] These are the high score entries for each of the teams required to attain a specific rank ingame. There are four entries for each team for each stage, from Rank D to C to B to A in order. Note that the scores are two bytes each [Short] and are multiplied by 100.
            /// </summary>
            public enum Stage_HighScores_Addresses
            {
                /// <summary>
                /// The very first high score requirement entry, for Team Sonic @ Seaside Hill.
                /// </summary>
                HighScoreEntryStart = 0x7C744C,
                /// <summary>
                /// Length of each stage entry, if you want to go to a specific stage.
                /// </summary>
                HighScoreLevelEntryLength = 0x24,
            }

            // These are all 1 byte values
            /// <summary>
            /// Defines tha addresses used to store certain unlock requirements
            /// </summary>
            public enum Game_UnlockRequirements
            {
                /// <summary>
                /// [ Byte[6] ] [Struct: Game_UnlockRequirements_TwoPlayer] The individual unlock requirements for Action Race, Battle, Special Stage, Ring Race, Bobsled Race, Quick Race and Expert Race. Each is a byte and represents the number of emblems required. 
                /// </summary>
                StartingAddressEmblemRequirement = 0x7433C0,

                /// <summary>
                /// [Byte] The percentage completion of story mode of Team Sonic until Last Story may be unlocked.
                /// </summary>
                MetalSonicLastStoryPercentageRequirementSonic = 0x45642D,
                /// <summary>
                /// [Byte] The percentage completion of story mode of Team Dark until Last Story may be unlocked.
                /// </summary>
                MetalSonicLastStoryPercentageRequirementDark = 0x456456,
                /// <summary>
                /// [Byte] The percentage completion of story mode of Team Rose until Last Story may be unlocked.
                /// </summary>
                MetalSonicLastStoryPercentageRequirementRose = 0x45647F,
                /// <summary>
                /// [Byte] The percentage completion of story mode of Team Chaotix until Last Story may be unlocked.
                /// </summary>
                MetalSonicLastStoryPercentageRequirementChaotix = 0x4564A8,
                /// <summary>
                /// [Byte] THe amount of individual chaos emetalds necessary to collect until Last Story may be unlocked.
                /// </summary>
                RequiredChaosEmeralds = 0x4564CF,
                /// <summary>
                /// If you wish to obtain more than 7 Chaos Emeralds, it is recommended you replace 0x94 here with 0x9D
                /// </summary>
                ChaosEmeraldMaxComparisonAddress = 0x4564D1,
            }

            /// <summary>
            /// [Byte] To change the Super Hard rank unlock requirement, change each of the values in these addresses with an appropriate rank from Game_UnlockRequirements_RankID and perform the ASM Injection Unlock_Super_Hard_At_Rank_X_Or_Higher.
            /// </summary>
            public enum Game_UnlockRequirements_SuperHardRanks
            {
                Checkaddress1 = 0x44E780,
                Checkaddress2 = 0x450429,
                Checkaddress3 = 0x45524D
            }
            
            /// <summary>
            /// [Byte] Where applicable, this defines the individual valid Rank IDs for a rank requirement entry.
            /// </summary>
            public enum Game_UnlockRequirements_RankID
            {
                NotCleared = 0x0,
                ERank = 0x1,
                DRank = 0x2,
                CRank = 0x3,
                BRank = 0x4,
                ARank = 0x5,
            }

            /// <summary>
            /// The addresses which point towards the usage of screen fade transitions such as when transiting to an FMV or to the loading screen for the stage from the main menu.  
            /// </summary>
            public enum Game_GameMenuScreens_Transitions_Addresses
            {
                ScreenTransitionAddress = 0x405766,
                ScreenTransitionGameCompletionAddress = 0x454943,
            }

            /// <summary>
            /// Parameters for various certain game menu screen transitions.
            /// </summary>
            public enum Game_GameMenuScreens_Transitions_Parameters
            {
                /// <summary>
                /// [Byte] Fade speed of the game clear screen. Default:10 
                /// </summary>
                ClearScreenSpeed = 0x454AA9,
                /// <summary>
                /// [Byte] Fade speed of the main menu screen. Default:10 
                /// </summary>
                MainMenuSpeed = 0x454AE9,
            }
            
            

        }
  }
