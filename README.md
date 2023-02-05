# Genie4_Landscape For Dragonrealms (only!)
## Installation Steps:

1. Download the Landscape.dll from the \Compiled Folder and put it in your \Plugins Folder for Genie (tested with 4.0.2.6)
2. Download the \Images Folder (this contains default and a number of blank images) and put this in your \Plugins Folder as well. 
3. Start Genie, plugin should load. 
4. Open the "LandScape 4Genie" Window from the Plugins drop-down menu in Genie. Images should load based on zoneid and/or roomid when the user is logged in and connected via Genie. 

### Note 1: This plugin will attempt to search for images hosted by play.net and download those to display in the window. If you don't like this, you can disable the option per the below commands. 

### Note 2: Images in the image folder are dependent on the current Automapper naming convention in Genie. The naming convention is as follows:

zoneid.jpg (example **"1.jpg"**): If no image containing a roomID is found, the plugin will default to whatever image is associated with the zone. For example, if you are in the Crossing in "Zone 1" per Genie's Automapper, the plugin will load 1.jpg image for the zone. Otherwise, if no image is found, the plugin will load default.jpg in the the \Images folder (for example, when the user is disconnected in Genie). 

zoneid_roomid.jpg (example **"1_42.jpg"**): If an image is found with a matching zone and room ID per Automapper naming schema, it will load that specific room image into the plugin. As an example, 1_42.jpg is Zone 1 (The Crossing), Room 42 (The room directly outside the Crossing Bank). 

### Note 3: A lot of the Zone Images in the \Images folder are blank (just a blank white background with the word blank). Feel free to generate your own! I'll update the images here when I find some I like.... but don't let that stop you.  

### Note 4: Because, you can add your own images to the \Images using the same naming convention! Images should be saved as a .jpg with 512X512 pixels. The plugin window is sized to show images at 512px X 512px. 

## Basic Commands to use in Genie:

/landscape : Returns a simple confirmation that the plugin is working. 

/landscape enableURL : Enables checking to see if there is a URL of the image on play.net for the specified room (Note: This is enabled by default)

/landscape disableURL : Disables checking to see if there is a URL of the image on play.net, and instead loads images from the local \Images folder only. 

## Screenshot

![Screenshot](/Landscape_Screenshot.jpg)
