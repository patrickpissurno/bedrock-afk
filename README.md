# <img src="https://github.com/patrickpissurno/bedrock-afk/blob/main/icon.png?raw=true" width="32" height="32" alt="Bedrock AFK logo"> Bedrock AFK <img src="https://github.com/patrickpissurno/bedrock-afk/blob/main/icon.png?raw=true" width="32" height="32" alt="Bedrock AFK logo">
A Minecraft Bedrock Tool to prevent you from going AFK (and getting kicked)

**It's easy to use and 100% effective. And it's completely safe.**

## How to use

1. Download the [latest release](https://github.com/patrickpissurno/bedrock-afk/releases/latest/download/BedrockAFK.exe)
2. Launch Minecraft Windows 10 Edition (it's the one from Microsoft Store, not the Java one)
3. Open **BedrockAFK** and choose one of the available modes (eg. Walk Forward)
4. Switch back to the game and enjoy

Once you're done with it, just head back to **BedrockAFK** and close the app.

### Notice
It is required that the game window be active (focused) for this tool to work, which means **you can't minimize the game and do something else while AFK on Minecraft**. While this is unfortunate, AFAIK there's no workaround for that due to the way Minecraft Bedrock and all other UWP apps work.

<br>

## How does it work
Under the hood **BedrockAFK** calls the Universal Windows Platform (UWP) APIs so that it can simulate keyboard and mouse presses on Minecraft. Due to the way it's implemented, it's virtually indistinguishable from actual user input.

<br>

## Available modes
1. Walk Forward (every now and then simulates the 'W' key being pressed)
2. Water Bucket (intended to be used with a water bucket, simulating the right mouse button)
3. AFK Fishing (fast) (intended to be used with an AFK fishing contraption, such as [this one](https://www.youtube.com/watch?v=yvsvFrILXJY))
4. AFK Fishing (slow) (same as above, but useful when not using a lure-enchanted fishing rod)

<br>

## Requirements
- Minecraft Windows 10 Edition (a.k.a. the Microsoft Store one)
- .NET Framework 4.7.2 (you shouldn't need to download it if you have Windows 10 1803 or later)
- Windows 10

<br>

## License
See the full license [here](LICENSE).
```
    Bedrock AFK
    Copyright © 2021 Patrick Pissurno

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
```
