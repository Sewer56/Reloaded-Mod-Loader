#include <windows.h> // Windows API Functions.
#include <string> // Additional String-based Functions.
using namespace std; // Might be useful, in my C++ improvement experience.

// DLL Exports of Various Game Functions
extern "C" __declspec(dllexport) void __stdcall Play_Song(int Song_Name_Pointer);

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