#include <windows.h> // Windows API Functions.
#include <string> // Additional String-based Functions.
#include <thread> // Threading.
using namespace std; // Might be useful, in my C++ improvement experience.

// DLL Exports of Various Game Functions
extern "C" __declspec(dllexport) void __stdcall Play_Song(int Song_Name_Pointer);
extern "C" __declspec(dllexport) void __stdcall Play_Sound(int Sound_Effect_BANK_And_ID, int Unknown_Sound_Stream_Address, char Unknown_AlwaysZero, int Unknown_AlwaysZero_II);

// Pointer to function responsible for switching the current track/song.
static const void *const Play_Song_Pointer = (void*)0x0043E7B0;

// Switches the currently active song/audio track and plays it.
// Note: If a track is not playing (never the case under normal circumstances), the track will not play by invoking this.
// Parameter is a pointer to a string with the ADX file name in /dvdroot/bgm/, for custom tracks, feel free to write the names manually to memory and pass the address to them.
void __stdcall Play_Song(int Song_Name_Pointer)
{
	__asm
	{
		mov eax, Song_Name_Pointer // Pointer to Song Name (Parameter) is stored in Register EAX.
		call Play_Song_Pointer // Call the responsible function!
	}
}

// Plays a sound such as collecting a ring.
// Unknown_Sound_Stream_Address = Pointer pointed to by 0x00A2F8B0
// Sound_Effect_BANK_And_ID = Bank Number as Char + Sound Number, 3 digits.
// e.g. bank1 - Sound003 = 0x1003
static const void* const Play_Sound_Pointer = (void*)0x004405F0;
typedef void(__stdcall *Play_Sound_Function)(char Unknown_AlwaysZero, int Unknown_AlwaysZero_II);
void __stdcall Play_Sound(int Sound_Effect_BANK_And_ID, int Unknown_Sound_Stream_Address, char Unknown_AlwaysZero, int Unknown_AlwaysZero_II)
{
	try
	{

		Play_Sound_Function Unknown_Method = (Play_Sound_Function)Play_Sound_Pointer;
		__asm
		{
			mov esi, Unknown_Sound_Stream_Address
			mov ebx, Sound_Effect_BANK_And_ID
		}
		Unknown_Method(0, 0);
		__asm
		{
			cmp eax, 0
			je endlabel
			mov[eax + 0x0C], 0x85
			endlabel:
		}
	}
	catch (exception ex) {} 
}

// Loads a level, maybe.
// Calls the game-state handler.
static const void* const Load_Level_Pointer = (void*)0x401300;
static const void* const Load_Level_Pointer_II = (void*)0x4023B0;
typedef int(__stdcall *Load_Level_Function_X)(int Level_ID);
extern "C" __declspec(dllexport) void __stdcall Load_Level_Function(int Level_ID);
extern "C" __declspec(dllexport) void __stdcall Load_Test_Level_Function(int Level_ID);
void __stdcall Load_Level_Function(int Level_ID)
{
	try
	{
		DWORD JumpAddress = 0x0402B49;
		__asm
		{
			// Register Setup
			mov eax, Level_ID
			mov ebx, Level_ID
			mov edi, 0x008D66E8

			// Titlecard Function Call
			mov[edi + 0x8], 1
			mov[edi + 0x24], 0
			mov[edi + 0x38], eax
			mov[edi + 0x28], eax

			// The Function Call
			push edi
			call Load_Level_Pointer

			// Instructions After | New Setup
			mov ecx, [edi + 0x28]
			mov edx, [edi + 0x2C]
			mov eax, [edx + ecx * 4]
			mov[edi + 0x30], eax
			mov[edi + 8], 2

			// Jump!
			//jmp [JumpAddress]
		}
	}
	catch (exception ex) {}
}

void __stdcall Load_Test_Level_Function(int Level_ID)
{
	try
	{
		DWORD JumpAddress = 0x0402B49;
		__asm
		{
			// Register Setup
			mov eax, Level_ID
			mov ebx, Level_ID
			mov edi, 0x008D66E8

			// Titlecard Function Call
			mov[edi + 0x8], 1
			mov[edi + 0x24], 0
			mov[edi + 0x38], eax
			mov[edi + 0x28], eax

			// The Function Call
			push edi
			call Load_Level_Pointer

			// Instructions After | New Setup
			mov ecx, 1
			mov edx, [edi + 0x2C]
			mov eax, [edx + ecx * 4]
			mov[edi + 0x30], eax
			mov[edi + 8], 2

			// Jump!
			//jmp [JumpAddress]
		}
	}
	catch (exception ex) {}
}

// Loads a level, maybe.
// Calls the game-state handler.
typedef int(__stdcall *Load_Level_Function_X2)();
extern "C" __declspec(dllexport) int __stdcall Load_Level_Function_II(int Game_State_ID);
int __stdcall Load_Level_Function_II(int Game_State_ID)
{
	try
	{
		__asm
		{
			mov esi, 0x00A81FF8
			mov ecx, 0x008D66E8
			mov edi, 0x008D66E8
			mov eax, Game_State_ID

			//push esi
			//push ecx
			mov[edi + 0x8], eax
			call Load_Level_Pointer_II
			mov[edi + 0x8], 1
		}
	}
	catch (exception ex) {}

	return 1;
}

// Loads a level, maybe.
// Calls the game-state handler.
extern "C" __declspec(dllexport) int __stdcall Change_Game_State(int Game_State_ID);
int __stdcall Change_Game_State(int Game_State_ID)
{
	try
	{
		__asm
		{
			mov esi, 0x00A81FF8
			mov ecx, 0x008D66E8
			mov edi, 0x008D66E8
			mov eax, Game_State_ID

			//push esi
			//push ecx
			mov[edi + 0x8], eax
			call Load_Level_Pointer_II
		}
	}
	catch (exception ex) {}

	return 1;
}

// Loads a level, maybe.
// Calls the game-state handler.
typedef int(__thiscall *Font_Load_Test_Function)(void* pThis);
Font_Load_Test_Function Font_Load_Test_Function_Call;
extern "C" __declspec(dllexport) void __stdcall Font_Load_Test_Function_Method();
void __stdcall Font_Load_Test_Function_Method()
{
	Font_Load_Test_Function_Call = (Font_Load_Test_Function)0x00419B00;
	Font_Load_Test_Function_Call((void*)0x8D66E8);
}

// Loads a level, maybe.
// Calls the game-state handler.
typedef int(__thiscall *Load_Main_Menu)(void* pThis);
Load_Main_Menu Load_Main_Menu_Function_Call;
extern "C" __declspec(dllexport) void __stdcall Load_Main_Menu_Function();
void __stdcall Load_Main_Menu_Function()
{
	Load_Main_Menu_Function_Call = (Load_Main_Menu)0x00401210;
	Load_Main_Menu_Function_Call((void*)0x008D66E8);
}



// Loads a level, maybe.
// Calls the game-state handler.
typedef int(__thiscall *Load_Debug_Function_X)(void* pThis);
Load_Debug_Function_X Load_Debug_Function_Method_Call;
extern "C" __declspec(dllexport) void __stdcall Load_Debug_Function();
void __stdcall Load_Debug_Function()
{
	try
	{
		Load_Debug_Function_Method_Call = (Load_Debug_Function_X)0x00643E40;
		Load_Debug_Function_Method_Call((void*)0x008DB5AC);
	}
	catch (exception ex) {}
}


// Loads a level, maybe.
// Calls the game-state handler.
typedef int(__cdecl *Load_AFS_Language_File_X)();
Load_AFS_Language_File_X Load_AFS_Language_File_Call;
extern "C" __declspec(dllexport) void __stdcall Load_AFS_Language_File();
void __stdcall Load_AFS_Language_File()
{
	Load_AFS_Language_File_Call = (Load_AFS_Language_File_X)0x0043E2F0;
	Load_AFS_Language_File_Call();
}

// Pointer to function responsible for switching the current track/song.
static const void *const Load_File_Into_Memory_Function_Pointer = (void*)0x00446100;
extern "C" __declspec(dllexport) void __stdcall Load_File_Into_Memory(int Pointer);
void __stdcall Load_File_Into_Memory(int Pointer)
{
	__asm
	{
		mov eax, Pointer
		call Load_File_Into_Memory_Function_Pointer
	}
}

// Pointer to function responsible for switching the current track/song.
static const void *const Reload_Bank3_Pointer = (void*)0x004401C0;
static const void *const Load_File_Pointer = (void*)0x00440A60;
static const void *const Something_Pointer = (void*)0x004402B0;
static const void *const Test = (void*)0x004400D0;

extern "C" __declspec(dllexport) void __stdcall Reload_Bank3(int PointerX);
void __stdcall Reload_Bank3(int PointerX)
{
	try 
	{
		__asm
		{
			mov eax, [PointerX]
			call Test

			/*
			mov esi, 0x0FEF5AA0 // Unknown constant used in I/O operations.
			push esi
			call Load_File_Pointer
			mov byte ptr[esi + 3], 0
			mov byte ptr[esi + 4], 0
			call Something_Pointer
			call Reload_Bank3_Pointer
			*/
		}
	}
	catch (exception ex) {}
}

// Pointer to function responsible for switching the current track/song.
static const void *const Test_Pointer_X = (void*)0x00427120;
extern "C" __declspec(dllexport) void __stdcall Test_Method_X();
void __stdcall Test_Method_X()
{
	try
	{
		__asm
		{
			//mov ecx, 0x008D66E8
			mov esi, 0x00A81FF8
			call Test_Pointer_X
		}
	}
	catch (exception ex) {}
}

// Pointer to function responsible for switching the current track/song.
static const void *const Exit_Stage = (void*)0x404640;
extern "C" __declspec(dllexport) void __stdcall Exit_Stage_X();
void __stdcall Exit_Stage_X()
{
	try
	{
		__asm
		{
			//mov ecx, 0x008D66E8
			mov esi, 0x008D66E8
			call Exit_Stage
		}
	}
	catch (exception ex) {}
}

// Pointer to function responsible for switching the current track/song.
static const void *const Experiment = (void*)0x00407260;
extern "C" __declspec(dllexport) void __stdcall Experiment_X();
void __stdcall Experiment_X()
{
	try
	{
		__asm
		{
			call Experiment
		}
	}
	catch (exception ex) {}
}