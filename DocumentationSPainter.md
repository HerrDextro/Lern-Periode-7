## Documentation for `SPainter`

### Overview

The `SPainter` class is a console-based 2D graphics renderer that enables developers to create pixel-based graphics and handle simple rendering tasks in a console application. It supports dynamic drawing, color management, and basic user interaction via keyboard input.

---

### Namespace

```csharp
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
```

The class is contained within the `Graphic_Renderer` namespace.

---

### Features

1. **Pixel Drawing**: Manipulate pixels with specific colors and characters.
2. **Keyboard Interaction**: Detect key presses using `GetAsyncKeyState`.
3. **Rendering**: Render frames in the console with efficient updates to minimize flickering.
4. **Basic Shapes and Text**: Draw rectangles, write text, and manage colors.
5. **Save and Load**: Save pixel data to a file and load it back for reuse.

---

### Class Properties

- `pixel`: A 2D string array representing the background colors of pixels.
- `pixelLast`: A 2D array storing the previous frame's state for optimized rendering.
- `charType`: A 2D array specifying the character to be rendered at each pixel position.
- `defaultTextColor`: The default text color for characters.
- `defaultBGColor`: The default background color for pixels.

---

### Methods

#### 1. **Constructor**

```csharp
SPainter(int width, int height, string color)
```

Initializes the `SPainter` object with the given width, height, and background color.

#### 2. **Rendering Methods**

- `renderFrame()`: Draws the entire frame to the console.
- `updateFrame()`: Updates only the changed parts of the frame for better performance.

#### 3. **Drawing and Writing**

- `clear()`: Resets the entire drawing area to the default background color.
- `changePixel(string color, int xpos, int ypos)`: Changes the color of a single pixel.
- `fillRectangle(string color, int xstart, int ystart, int xsize, int ysize)`: Draws a filled rectangle.
- `writeText(string text, int xpos, int ypos)`: Writes text at a specific position.

#### 4. **Saving and Loading**

- `saveImage(int xstart, int ystart, int xsize, int ysize, string filepath)`: Saves a region of the pixel data to a file.
- `loadImage(int xpos, int ypos, string filepath)`: Loads pixel data from a file and places it at the specified position.

#### 5. **Input Handling**

- `KeyDown(int keyCode)`: Returns `true` if a specific key is pressed.

#### 6. **Color Management**

- `changeTextColor(string color)`: Changes the text color for drawing.
- `setColor(string color)`: Sets the foreground color for rendering.
- `setBGColor(string color)`: Sets the background color for rendering.

---

### Usage Example

```csharp
using Graphic_Renderer;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the SPainter
        SPainter painter = new SPainter(40, 20, "Black");

        // Draw a red rectangle
        painter.fillRectangle("Red", 5, 5, 10, 5);

        // Write text inside the rectangle
        painter.writeText("Hello!", 7, 7);
        
        // Render the initial frame
        painter.renderFrame();

        // Main game loop
        while (true)
        {
            // Change text color dynamically on key press
            if (painter.KeyDown(SPainter.A))
            {
                painter.changeTextColor("Green");
            }

            // Update the frame with changes
            painter.updateFrame();
        }
    }
}
```

---

### Constants

The class includes predefined constants for:
- **Letters**: `A`, `B`, ..., `Z`
- **Numbers**: `n0`, `n1`, ..., `n9`
- **Special Characters**: `space`, `enter`, `arrowUp`, etc.

These constants are mapped to their respective virtual key codes.

---

### Key Features in Depth

1. **Optimized Rendering**:
   - `updateFrame()` reduces console writes by only redrawing changed pixels.
2. **Keyboard Input**:
   - `KeyDown(int keyCode)` leverages the `GetAsyncKeyState` method to detect keyboard inputs efficiently.
3. **Flexible Graphics**:
   - Use `fillRectangle` and `changePixel` to create dynamic, pixel-perfect graphics in the console.

---

### File Format for Saving

Saved images use a plain text format:
- Each row is represented as a line.
- Pixel colors are separated by semicolons (`;`).

Example:
```
Red;Red;Red;Red;...
Black;Black;Black;...
```

This format ensures simple reading and writing.

---

### Limitations

1. Restricted to the console's capabilities (limited color palette and resolution).
2. Single-threaded; performance may degrade with larger grids or frequent updates.

---

### Recommendations

- Use `updateFrame()` for animations and frequent updates.
- Prefer `renderFrame()` for static content or initialization.
- Ensure color strings match valid `ConsoleColor` values to avoid runtime errors.