# TouchScreenAPI

TouchScreenAPI is a mod for [Space Engineers](https://www.spaceengineersgame.com/) (game developed by Keen Software House). This mod allows players to interact with the LCD blocks as if they were touch screen, introducing a robust UI framework with advanced elements that other modders can utilize through its API.

![TouchScreenAPI](https://github.com/user-attachments/assets/ad21e979-33c2-4aef-ad78-b2386270c203)

## How It Works

In the base game, LCD blocks in Space Engineers serve as static displays for information, much like TVs. They do not support any form of player interaction. However, with the Touch Screen API mod, these LCD blocks are transformed into fully interactive touch screens, unlocking a wide range of possibilities for gameplay and customization.

The mod achieves this by tracking the player’s 3D position and calculating the direction they are facing relative to the screen. This allows the mod to project a virtual cursor onto the screen, enabling players to interact with UI elements as if they were using a touch screen interface.

## Features

- **Touch Screen Interaction:** Convert standard LCD blocks into interactive touch screens.
- **UI Framework:** Access advanced UI elements and integrate them into your own mods.
- **API Access:** Simple and efficient API that can be used by other modders.
- **Popular Mods:** Touch Screen API is the foundation of two of the most popular mods of 2022 and 2023:
  - **[Electric Network Info App](https://steamcommunity.com/sharedfiles/filedetails/?id=2917216762):** Displays detailed electrical network information from the grid it is connected.
  - **[Button Pad App](https://steamcommunity.com/sharedfiles/filedetails/?id=2933676834):** Adds customizable button pads for various in-game functionalities.

## Installation

1. Download the mod from the [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2668820525) or from the GitHub releases.
2. Copy the mod files to your Space Engineers mods folder. [Optional if using Steam Workshop]
3. Enable the mod in your game settings.

## Mouse Clicks and Joystick

It accepts **Left click** as primary action button, some apps may have separated functionalities for **Right click** and **Middle click** as well.
On joysticks use **RB** for primary and **LB** to reproduce right click action. For middle click action press both **RB** and **LB**.

## Ui Kit - Elements

The Ui Kit is a decent library of common UI elements, there are more elements planned.
It is completely optional, the Touch features can be imported and used separated from the UI elements if the mod only needs the cursor feature.

- **TouchApp** - It is the main element, control draw loops, updates and dispose.
- **Button** - A simple button with a text label. There is an empty version of it that can have icons as children.
- **Chart** - Line charts on a grid, can have multiple lines.
- **Checkbox** - Simple checkbox.
- **Label** - Text Label, one of the simplest elements.
- **ProgressBar** - Progress bar, have a vertical option, also optional label.
- **Selector** - Multi options selector.
- **Slider** - A slider with a draggable thumb.
- **SliderRange** - Another slide, but for a range value, has two thumbs.
- **Switch** - A multi options switcher, can be used like tabs for a panel.
- **TextField** - Text input field, editable with keyboard. One of the most complex elements.
- **View** - The major container, has properties like margin, padding, border, alignment, etc.
- **ScrollView** - Like a view, but with scroll, automatically hide children out of bounds.
- **WindowBar** - Useful for app names and buttons for options at the top of a window.
- **EmptyElement** - Useful for adding custom MySprites to the App's hierarchy.

## Ui Kit - Styling

Every component have some properties to change their visuals, they're inspired and works like html+css elements. May looks pretty straight forward if you're familiar with website development.
The most important are these, for a complete list and documentation see the project github page:

- **Enabled** - If false, the element will not be drawn.
- **Absolute** - If true, the element will not align and anchor with the parent.
- **Flex** - The ratio of the parent that this element should fill.
- **Pixels** - Fixed size in pixels, not related to parent.
- **Margin** - Margin value for spacing elements on the four sides.
- **Padding** - Padding value for inner spacing of an element on the four sides.
- **Border** - Border value for an element. It is colored by BorderColor property.
- **Anchor** - Position of the children on the same axis of the Direction. Like start, center, end, etc.
- **Alignment** - The alignment of children on the crossed axis of the Direction
- **Direction** - The direction the elements will line, like Column and Row. Can be reversed.
- **Gap** - Adds a spacing between children. Better than adding margin to each child.
- **BgColor** - Background color.

## Credits

- Developed by [Adriano Lima](https://github.com/adrianulima)
- Special thanks to the Space Engineers modding community for their continuous support and feedback.

> [!IMPORTANT]
> This mod is not affiliated with or endorsed by Keen Software House. It is a fan-made project developed independently for Space Engineers.

## Contact

For questions, feedback, or support, please contact on Discord (@adrianolima) or open an issue on GitHub.

## Examples

See some touch app examples at <https://github.com/adrianulima/TouchScreenAPI_Examples>

![TouchUI_Adriano_textfield_gif](https://user-images.githubusercontent.com/13324869/143987989-53ea6aaa-ef02-48b0-aa5c-6bfb4ff418d7.gif)
