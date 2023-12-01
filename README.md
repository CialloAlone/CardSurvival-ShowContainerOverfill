![Overfill to filled compare](media/SackCompare.png)

# Show Container Overfill

## Bar Indicator
Changes the weight bar to red if a container is holding more than the weight bonus.  For example, Sacks and Travois both can carry more than the weight they can offset.

## Overflow Limit Option
Adds a toggle to limit filling containers to only their weight bonus.

This allows the user to fill a container without needing to manually add and remove items until exactly at or below the weight bonus.

This can be toggled on or off in real-time using the hotkey (L by default).

When the overfill lock is enabled, the bars will be changed to a slightly darker yellow.


# Options

|Name|Default|Description|
|--|--|--|
|Default Bar Color|Yellow - Game's default color|The default color of the bar when not overfilled or locked.|
|Overfill Lock Color|Darker Yellow|The color of the storage bars when overfill lock is enabled and under the weight bonus.|
|Overfill Color|Red|The color of the storage bars when a container contains more weight than the weight bonus.|
|Overfill Toggle Key|L|Toggles limiting containers to only accept cards up to the weight bonus.  For example, A sack can contain 1000 weight, but only 600 weight is 'free'|
|Start Overfill Toggle Enabled|false|If true, the game will start with the overfill limit enabled.|


## Configuration Note

The easiest way to edit the above options is to use the BepInEx Configuration Manager found here:  https://github.com/BepInEx/BepInEx.ConfigurationManager
It supports a UI which can select colors as well as easily bind keys.

Alternatively, the configuration file is located in the game's directory at ```BepInEx\config\ShowContainerOverfill.cfg```


# Installation 
This section describes how to manually install the mod.

**If using the Vortex mod manager from NexusMods, these steps are not needed.**

## Overview
This mod requires the BepInEx mod loader.

## BepInEx Setup
If BepInEx has already been installed, skip this section.

Download BepInEx from https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x64_5.4.21.0.zip

* Extract the contents of the BepInEx zip file into the game's directory:
```<Steam Directory>\steamapps\common\Card Survival Tropical Island```

    __Important__:  The .zip file *must* be extracted to the root folder of the game.  If BepInEx was extracted correctly, the following directory will exist: ```<Steam Directory>\steamapps\common\Card Survival Tropical Island\BepInEx```.  This is a common install issue.

* Run the game.  Once the main menu is shown, exit the game.
    
* In the BepInEx folder, there will now be a "plugins" directory.

## Mod Setup
* Download the ShowContainerOverfill.zip.  
    * If on Nexumods.com, download from the Files tab.
    * Otherwise, download from https://github.com/NBKRedSpy/CardSurvival-ShowContainerOverfill/releases/

* Extract the contents of the zip file into the ```BepInEx/plugins``` folder.

* Run the Game.  The mod will now be enabled.

# Uninstalling

## Uninstall
This resets the game to an unmodded state.

Delete the BepInEx folder from the game's directory
```<Steam Directory>\steamapps\common\Card Survival Tropical Island\BepInEx```

## Uninstalling This Mod Only

This method removes this mod, but keeps the BepInEx mod loader and any other mods.

Delete the ```ShowContainerOverfill.dll``` from the ```<Steam Directory>\steamapps\common\Card Survival Tropical Island\BepInEx\plugins``` directory.

# Compatibility
Safe to add and remove from existing saves.

# Change Log 

## 2.0.0
Added "prevent overfill" toggle functionality

## 1.0.0  
* Initial Release


