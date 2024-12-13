# Documentation for SPainter Class

## Overview
The `SPainter` class provides functionality to render graphical content directly onto the console. It supports manipulating console text, setting background and foreground colors, drawing shapes, and saving/loading graphical data for console applications.

### Namespace
`Graphic_Renderer`

## Class: SPainter

### Constructor
```csharp
public SPainter(int width, int height, string color)
```
- **Description**: Initializes a new instance of the `SPainter` class with specified dimensions and default background color.
- **Parameters**:
  - `width` (int): The width of the console in characters.
  - `height` (int): The height of the console in characters.
  - `color` (string): The default background color.

---

## Methods

### 1. `updateFrame()`
```csharp
public void updateFrame()
```
- **Description**: Updates the console display by redrawing pixels that have changed since the last frame.

### 2. `updateText()`
```csharp
public void updateText()
```
- **Description**: Updates all text elements on the console that are not represented by solid pixels (`█`).

### 3. `clear()`
```csharp
public void clear()
```
- **Description**: Clears the console by resetting all pixels and character types to default values.

### 4. `renderFrame()`
```csharp
public void renderFrame()
```
- **Description**: Renders the initial frame to the console by drawing all pixels and text elements.

### 5. `writeText()`
```csharp
public void writeText(string text, int xpos, int ypos)
```
- **Description**: Writes a string of text at a specified position on the console.
- **Parameters**:
  - `text` (string): The text to be displayed.
  - `xpos` (int): The X-coordinate for the text.
  - `ypos` (int): The Y-coordinate for the text.

### 6. `changeTextColor()`
```csharp
public void changeTextColor(string color)
```
- **Description**: Changes the default text color.
- **Parameters**:
  - `color` (string): The new text color.

### 7. `changePixel()`
```csharp
public void changePixel(string color, int xpos, int ypos)
```
- **Description**: Changes the color of a specific pixel on the console.
- **Parameters**:
  - `color` (string): The new color for the pixel.
  - `xpos` (int): The X-coordinate of the pixel.
  - `ypos` (int): The Y-coordinate of the pixel.

### 8. `fillRectangle()`
```csharp
public void fillRectangle(string color, int xstart, int ystart, int xsize, int ysize)
```
- **Description**: Fills a rectangular area with a specified color.
- **Parameters**:
  - `color` (string): The fill color.
  - `xstart` (int): The X-coordinate of the top-left corner.
  - `ystart` (int): The Y-coordinate of the top-left corner.
  - `xsize` (int): The width of the rectangle.
  - `ysize` (int): The height of the rectangle.

### 9. `saveImage()`
```csharp
public void saveImage(int xstart, int ystart, int xsize, int ysize, string filepath)
```
- **Description**: Saves a portion of the console screen as a text-based representation to a file.
- **Parameters**:
  - `xstart` (int): The starting X-coordinate.
  - `ystart` (int): The starting Y-coordinate.
  - `xsize` (int): The width of the area to save.
  - `ysize` (int): The height of the area to save.
  - `filepath` (string): The path of the file to save to.

### 10. `loadImage()`
```csharp
public void loadImage(int xpos, int ypos, string filepath)
```
- **Description**: Loads a previously saved image into the console at the specified position.
- **Parameters**:
  - `xpos` (int): The X-coordinate where the image will be placed.
  - `ypos` (int): The Y-coordinate where the image will be placed.
  - `filepath` (string): The path of the file to load.

---

## Private Helper Methods

### `populateList()`
```csharp
private string[,] populateList(string[,] list, string input)
```
- **Description**: Fills a 2D string array with a specified value.

### `toSingleString()`
```csharp
private string toSingleString(string[] input)
```
- **Description**: Converts an array of strings into a single concatenated string with line breaks.

### `setColor()`
```csharp
private void setColor(string color)
```
- **Description**: Sets the console foreground color.

### `setBGColor()`
```csharp
private void setBGColor(string color)
```
- **Description**: Sets the console background color.

---

## Usage Example
```csharp
// Initialize the SPainter
SPainter painter = new SPainter(40, 20, "Black");

// Draw a rectangle
painter.fillRectangle("Red", 5, 5, 10, 5);

// Write text
painter.writeText("Hello, World!", 10, 10);

// Render the frame
painter.renderFrame();

// Save the image
painter.saveImage(0, 0, 40, 20, "output.txt");

// Load the image back
painter.loadImage(0, 0, "output.txt");

// Update the frame
painter.updateFrame();
```

---

## Notes
- The console's dimensions must be adjusted to match the application's requirements for best results.
- Supported colors correspond to `ConsoleColor` enum values.

