# Genie4_Landscape
## Installation Steps:

1. Download the Landscape.dll from the \Compiled Folder and put it in your \Plugins Folder for Genie (tested with 4.0.2.6)
2. Download the \Images Folder (this contains default and a number of blank images) and put this in your \Plugins Folder as well. 
3. Start Genie, plugin should load. 
4. Open the "LandScape 4Genie" Window from the Plugins drop-down menu in Genie. Images should load based on zoneid and/or roomid. 

### Note 1: This plugin will attempt to search for images hosted by play.net and download those to display in the window. If you don't like this, you can disable the option per the below commands. 
### Note 2: Images in the image folder are dependent on the current Automapper naming convention in Genie. The naming convention is as follows:

### zoneid.jpg (example **"1.jpg"**): If no image containing a roomID is found, the plugin will default to whatever image is associated with the zone. For example, if you are in the Crossing in "Zone 1" per Genie's Automapper, the plugin will load the default 1.jpg image for the zone. 
### zoneid_roomid.jpg (example **"1_42.jpg"**): If an image is found with a matching zone and room ID per Automapper naming schema, it will load that specific room image into the plugin. As an example, 1_42.jpg is Zone 1 (The Crossing), Room 42 (The room directly outside the Crossing Bank). 
  
## Basic Commands to use in Genie:

/landscape : Returns a simple confirmation that the plugin is working. 

/landscape enableURL : Enables checking to see if there is a URL of the image on play.net for the specified room (Note: This is enabled by default)

/landscape disableURL : Disables checking to see if there is a URL of the image on play.net, and instead loads images from the local \Images folder only. 

## Screenshot

![Screenshot](/Landscape_Screenshot.jpg)
