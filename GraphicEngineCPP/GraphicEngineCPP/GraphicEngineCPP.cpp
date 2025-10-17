
/*------------------------------------------------------------------ Imports */
#include <string>
#include <iostream>
#include <windows.h>
#include <codecvt>
#include <locale>
#include <vector>

#pragma once





/*------------------------------------------------------------------ Structs + Inits */
struct PhysicalPixel {
    char backgroundColor[8]; // "#RRGGBB" + null terminator
    char foregroundColor[8];
    char32_t chartype;
};




static PhysicalPixel* oldPixels = nullptr; // global pointer (initially null)
static int g_rows = 0;
static int g_cols = 0;


/*------------------------------------------------------------------ Utility Helpers */
bool comparePixels(PhysicalPixel pixel_1, PhysicalPixel pixel_2) {
    return std::strncmp(pixel_1.backgroundColor, pixel_2.backgroundColor, 8) == 0
        && std::strncmp(pixel_1.foregroundColor, pixel_2.foregroundColor, 8) == 0
        && pixel_1.chartype == pixel_2.chartype;
}

void set_cursor(int x = 0, int y = 0)
{
    HANDLE handle;
    COORD coordinates;
    handle = GetStdHandle(STD_OUTPUT_HANDLE);
    coordinates.X = x;
    coordinates.Y = y;
    SetConsoleCursorPosition(handle, coordinates);
}
void set_console_utf8() {
    SetConsoleOutputCP(CP_UTF8);    // sets console to UTF-8
    SetConsoleCP(CP_UTF8);
}

bool ParseHexColor(const char* hex, int& r, int& g, int& b) {
    if (!hex || hex[0] != '#') return false;

    try {
        std::string s(hex + 1, 6); // take exactly 6 chars after '#'
        if (s.length() != 6) return false;

        int value = std::stoi(s, nullptr, 16);
        r = (value >> 16) & 0xFF;
        g = (value >> 8) & 0xFF;
        b = value & 0xFF;
        return true;
    }
    catch (...) {
        r = g = b = 0; // fallback if parsing fails
        return false;
    }
}

// Extract both foreground and background colors from a pixel
void GetPixelColors(const PhysicalPixel& px,
    int& r, int& g, int& b,
    int& br, int& bg, int& bb) {
    if (!ParseHexColor(px.foregroundColor, r, g, b)) r = g = b = 0;
    if (!ParseHexColor(px.backgroundColor, br, bg, bb)) br = bg = bb = 0;
}





/*------------------------------------------------------------------ Printing Helpers */
void WriteChar32(HANDLE hConsole, char32_t c) {
    wchar_t buffer[2];
    DWORD written = 0;

    if (c <= 0xFFFF) {
        buffer[0] = static_cast<wchar_t>(c);
        WriteConsoleW(hConsole, buffer, 1, &written, nullptr);
    }
    else {
        // surrogate pair for > 0xFFFF
        c -= 0x10000;
        buffer[0] = 0xD800 + ((c >> 10) & 0x3FF); // high surrogate
        buffer[1] = 0xDC00 + (c & 0x3FF);         // low surrogate
        WriteConsoleW(hConsole, buffer, 2, &written, nullptr);
    }
}

std::string Char32ToUtf8(char32_t c) {
    std::u32string tmp(1, c);
    std::wstring_convert<std::codecvt_utf8<char32_t>, char32_t> conv;
    return conv.to_bytes(tmp);
    int f = 1;
}

std::string utf32_to_utf8(char32_t c) {
    std::string out;
    if (c <= 0x7F) {
        out += static_cast<char>(c);
    }
    else if (c <= 0x7FF) {
        out += static_cast<char>(0xC0 | ((c >> 6) & 0x1F));
        out += static_cast<char>(0x80 | (c & 0x3F));
    }
    else if (c <= 0xFFFF) {
        out += static_cast<char>(0xE0 | ((c >> 12) & 0x0F));
        out += static_cast<char>(0x80 | ((c >> 6) & 0x3F));
        out += static_cast<char>(0x80 | (c & 0x3F));
    }
    else {
        out += static_cast<char>(0xF0 | ((c >> 18) & 0x07));
        out += static_cast<char>(0x80 | ((c >> 12) & 0x3F));
        out += static_cast<char>(0x80 | ((c >> 6) & 0x3F));
        out += static_cast<char>(0x80 | (c & 0x3F));
    }
    return out;
}

std::string utf32_to_utf8(const std::u32string& str) {
    std::string result;
    for (char32_t c : str)
        result += utf32_to_utf8(c);
    return result;
}


void WriteUtf32String(HANDLE hConsole, const std::u32string& finished) {
    std::string output;
    output.reserve(finished.size() * 4); // rough estimate to avoid reallocs


    output = utf32_to_utf8(finished);

    DWORD written = 0;
    WriteConsoleA(hConsole, output.c_str(), static_cast<DWORD>(output.size()), &written, nullptr);
}


void WritePixel(HANDLE hConsole, char32_t character, int r, int g, int b, int br, int bg, int bb) {
    // Construct ANSI color string
    std::string color = "\x1b[38;2;" + std::to_string(r) + ";" + std::to_string(g) + ";" + std::to_string(b) +
        "m\x1b[48;2;" + std::to_string(br) + ";" + std::to_string(bg) + ";" + std::to_string(bb) + "m";

    DWORD written = 0;
    WriteConsoleA(hConsole, color.c_str(), static_cast<DWORD>(color.size()), &written, nullptr);

    // Write the actual character
    WriteChar32(hConsole, character);
}

std::u32string fast_utf8_to_utf32(const std::string& str) {
    std::u32string result;
    result.reserve(str.size()); // upper bound

    for (size_t i = 0; i < str.size();) {
        uint32_t codepoint = 0;
        unsigned char c = str[i];
        if (c < 0x80) { codepoint = c; i += 1; }
        else if ((c >> 5) == 0x6) { codepoint = ((c & 0x1F) << 6) | (str[i + 1] & 0x3F); i += 2; }
        else if ((c >> 4) == 0xE) { codepoint = ((c & 0x0F) << 12) | ((str[i + 1] & 0x3F) << 6) | (str[i + 2] & 0x3F); i += 3; }
        else if ((c >> 3) == 0x1E) { codepoint = ((c & 0x07) << 18) | ((str[i + 1] & 0x3F) << 12) | ((str[i + 2] & 0x3F) << 6) | (str[i + 3] & 0x3F); i += 4; }
        result.push_back(static_cast<char32_t>(codepoint));
    }
    return result;
}



// Problem here!!!! 8ms
std::u32string GetColorEscapeCodeNEW(int r, int g, int b, int br, int bg, int bb) {
    std::string color = "\x1b[38;2;" + std::to_string(r) + ";" + std::to_string(g) + ";" + std::to_string(b) +
        "m\x1b[48;2;" + std::to_string(br) + ";" + std::to_string(bg) + ";" + std::to_string(bb) + "m";

    std::wstring_convert<std::codecvt_utf8<char32_t>, char32_t> converter;
    std::u32string utf32_color = converter.from_bytes(color);

    return utf32_color;
}

std::u32string GetColorEscapeCode(int r, int g, int b, int br, int bg, int bb) {
    std::string color = "\x1b[38;2;" + std::to_string(r) + ";" + std::to_string(g) + ";" + std::to_string(b) +
        "m\x1b[48;2;" + std::to_string(br) + ";" + std::to_string(bg) + ";" + std::to_string(bb) + "m";

    // Fast ASCII-only conversion
    std::u32string utf32_color = fast_utf8_to_utf32(color);
    return utf32_color;
}


HANDLE hConsole;




/*------------------------------------------------------------------ Exported Functions */
extern "C" __declspec(dllexport)
void InstanceRuntimePixelArray(int cols, int rows) { // cols -> x, rows -> y
    if (oldPixels != nullptr) {
        delete[] oldPixels;
    }
    set_console_utf8();
    g_rows = rows;
    g_cols = cols;
    oldPixels = new PhysicalPixel[rows * cols];

    hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

    // Enable virtual terminal processing if you want ANSI colors
    DWORD mode = 0;
    GetConsoleMode(hConsole, &mode);
    SetConsoleMode(hConsole, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);

    for (int i = 0; i < rows * cols; i++) {
        strcpy_s(oldPixels[i].backgroundColor, "#000000");
        strcpy_s(oldPixels[i].foregroundColor, "#000000");
        oldPixels[i].chartype = U' ';
    }

    // Remove stdio compatibility (aka fuck up printf)
    std::ios::sync_with_stdio(false);
}



extern "C" __declspec(dllexport)
void ProcessArray(PhysicalPixel* newPixels, int xAmt, int yAmt) { // cols -> x, rows -> y
    // Displays content of array
    try {
        for (int y = 0; y < yAmt; y++) {
            int streak = 0;
            for (int x = 0; x < xAmt; x++) {

                // Calculate supposed index
                int index = (y * xAmt) + x;

                int len = (xAmt * yAmt) - 1;

                index = max(index, 0);
                index = min(index, len);


                bool pixelsEqual = comparePixels(newPixels[index], oldPixels[index]);
                if (!pixelsEqual) {
                    streak++;

                    /*
                    // If pixel updated -> Repaint

                    // Calculate colors
                    int r, g, b, br, bg, bb;
                    GetPixelColors(newPixels[index], r, g, b, br, bg, bb);

                    // Calculate character
                    char32_t character = (newPixels[index].chartype == U' ')
                        ? U'\u2584'
                        : newPixels[index].chartype;

                    // Update old array with new data


                    set_cursor(x, y);

                    WritePixel(hConsole, character, r, g, b, br, bg, bb);
                    */
                }
                if ((pixelsEqual || x == xAmt-1) && streak > 0) {
                    
                    if (x == xAmt - 1) { streak--; }


                    std::u32string line = U"";

                    set_cursor(x - (streak-1), y);
                    for (int i = 0; i < streak; i++) {
                        int actualIndex = index - (streak - 0) + i;


                        // Calculate colors
                        int r, g, b, br, bg, bb;
                        GetPixelColors(newPixels[actualIndex], r, g, b, br, bg, bb);

                        char32_t character = (newPixels[actualIndex].chartype == U' ')
                            ? U'\u2584'
                            : newPixels[actualIndex].chartype;

                        line += GetColorEscapeCode(r, g, b, br, bg, bb);
                        line += character;
                        oldPixels[actualIndex] = newPixels[actualIndex]; // This line somehow breaks the printing??? TODO Investigate this shi
                    }
                    streak = 0;
                    WriteUtf32String(hConsole, line);
                }
            }

        }
    }
    catch (const std::exception& ex) {
        std::cout << ex.what();
    }
}

#include <chrono>
#include <thread>

int main() {
    // Initialize a 10x10 pixel array
    const int xAmt = 40;
    const int yAmt = 20;
    InstanceRuntimePixelArray(xAmt, yAmt);

    // Create a new pixel array for testing
    PhysicalPixel* testPixels = new PhysicalPixel[xAmt * yAmt];

    int xpos = 10;
    int ypos = 10;
    int xvel = 1;
    int yvel = 1;

    int iteration = 0;

    while (true) {
        iteration++;
        // Fill it with a simple test pattern
        for (int y = 0; y < yAmt; y++) {
            for (int x = 0; x < xAmt; x++) {
                int index = y * xAmt + x;  // correct

                strcpy_s(testPixels[index].foregroundColor, "#0000FF"); // red
                strcpy_s(testPixels[index].backgroundColor, "#000000"); // black
                if (iteration % 2 == 0) {
                    strcpy_s(testPixels[index].foregroundColor, "#000000");
                }


                testPixels[index].chartype = U' ';
            }
        }
        int index = ypos * xAmt + xpos;
        
        strcpy_s(testPixels[index].foregroundColor, "#ff0000"); // red
        strcpy_s(testPixels[index].backgroundColor, "#ff0000"); // black
        testPixels[index].chartype = U' ';

        if (xpos >= 38 || xpos <= 0) {
            xvel *= -1;
        }

        if (ypos >= 19 || ypos <= 0) {
            yvel *= -1;
        }


        xpos += xvel;
        ypos += yvel;

        ProcessArray(testPixels, xAmt, yAmt);
        //std::this_thread::sleep_for(std::chrono::milliseconds(10));
        // Todo fix start print ????????????????



    }





    // Process the array (draw it to console)
    ProcessArray(testPixels, xAmt, yAmt);

    // Wait for user input before exiting
    std::cout << "\n\nPress Enter to exit...";
    std::cin.get();

    delete[] testPixels;
    return 0;
}




BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_DETACH:
        if (oldPixels != nullptr) {
            delete[] oldPixels;
            oldPixels = nullptr;
        }
        break;
    }
    return TRUE;
}



