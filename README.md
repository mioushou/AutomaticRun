# AutomaticRun

A Stardew Valley mod that lets you toggle auto-run so your farmer keeps moving in any direction without holding a key.

## Features

- **Toggle auto-run** with a configurable key (default: `Left Control` / `Left Stick` on controllers)
- **Collision detection**: optionally stop automatically when hitting a wall or obstacle
- **Controller support**: works with both keyboard and controller inputs
- **Config menu**: requires Generic Mod Config Menu

## Requirements

- Stardew Valley
- [SMAPI](https://smapi.io/)

**Optional:**

- [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098) -> for in-game configuration UI

## Installation

- Download and install the latest version of [SMAPI](https://smapi.io/)
- Download the latest release of AutomaticRun from:
    - Nexus
    - The [releases page](https://github.com/Mioushou/AutomaticRun/releases)
- Extract the contents of the downloaded archive into your `Stardew Valley/Mods` folder
- Install Generic [Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098) if you want in-game configuration options
- Launch the game using SMAPI

## How to use

- Press the configured toggle key (default: `Left Control` / `Left Stick`) to enable or disable auto-run
- When enabled, your farmer will keep moving in the last direction you were walking without needing to hold the movement key
- Works while riding as well
- If collision detection is enabled, auto-run will stop when you hit a wall or obstacle
- You can customize the toggle key and collision behavior in the mod config menu (if you have Generic Mod Config Menu installed) or by editing the `config.json` file in the mod folder

## Demo

**AutomaticRun in action:**
![Demo of AutomaticRun mod in action](assets/autorun.gif)

**With collision detection enabled:**
![Demo of AutomaticRun with collision detection enabled](assets/autorun_collision.gif)
