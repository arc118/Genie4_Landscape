using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeniePlugin.Interfaces;
using System.Security.Permissions;
using System.Runtime.CompilerServices;
using System.IO;
using GeniePlugin.Interfaces;
using System.Diagnostics;

namespace Landscape4Genie
{

    // To work with Genie, the class must extend IPlugin, using GeniePlugin.Interfaces
    public class Landscape : IPlugin
    {
        //Constant variable for the Properties of the plugin
        //At the top for easy changes.
        readonly string _NAME = "Landscape 4Genie";
        readonly string _VERSION = "1.0";
        readonly string _AUTHOR = "Guadrul";
        readonly string _DESCRIPTION = "Visual Graphics Window for Genie, based on Zone and Map IDs. Graphics courtesy of MidJourney and/or OpenAI";

        public System.Windows.Forms.Form _parent;       //Required for plugin
        public static IHost _host;                             //Required for plugin
        public MainForm _form;                          //Required for plugin and for using a pop-up Window in Genie

        private bool _enabled = true;                   //Required for plugin, for method Enabled

        public string zoneID;
        public string roomID;
        private bool enableURL = true;

        #region iPlugin Required Properties

        //Required for Plugin - Name Called when Genie needs the name of the plugin (On menu)
        //Return Value:
        //              string: Text that is the name of the Plugin
        public string Name
        {
            get { return _NAME; }
        }

        //Required for Plugin - Called when Genie needs the plugin Author (plugins window)
        //Return Value:
        //              string: Text that is the Author of the plugin
        public string Author
        {
            get { return _AUTHOR; }
        }

        //Required for Plugin - Called when Genie needs the plugin version (error text
        //                      or the plugins window)
        //Return Value:
        //              string: Text that is the version of the plugin
        public string Version
        {
            get { return _VERSION; }
        }

        //Required for Plugin - Called when Genie needs the plugin Description (plugins window)
        //Return Value:
        //              string: Text that is the description of the plugin
        //                      This can only be up to 200 Characters long, else it will appear
        //                      "truncated"
        public string Description
        {
            get { return _DESCRIPTION; }
        }

        //Required for Plugin - Called when Genie needs disable/enable the plugin (Plugins window,
        //                      and from the CLI), or when Genie needs to know the status of the 
        //                      plugin (???)
        //Get:
        //      Not Known what it is used for
        //Set:
        //      Used by Plugins Window + CLI
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        #endregion

        #region iPlugin Required Methods

        //Required for Plugin - Initialize() is called on first load
        //Parameters:
        //              IHost Host:  The host (instance of Genie) making the call
        public void Initialize(IHost Host)
        {
            _host = Host;
        }

        //Required for Plugin - Show() is called when clicking on the plugin 
        //name from the Menu item Plugins
        public void Show()
        {
            OpenWindow(_host.ParentForm);
        }

        //Required for Plugin - VariableChanged() is called when a global variable in genie
        //                      is changed
        //Parameters:
        //              string Text:  The variable name in Genie that changed
        public void VariableChanged(string Variable)
        {
            //////////////////////////////
            //ToDo: On Room Change, update image in form
            // genie variable: roomid
            // genie variable: zoneid 
            //////////////////////////////

            if (_host.get_Variable("zoneid") != null && _host.get_Variable("roomid") != null)
            {
                zoneID = _host.get_Variable("zoneid");
                roomID = _host.get_Variable("roomid");

                if (_form != null)
                    _form.Update_Image();
            }
 
        }

        //Required for Plugin - ParseText()
        //Parameters:
        //              string Text:    The DIRECT text comes from the game (non-"xml")
        //              string Window:  The Window the Text was received from
        //Return Value:
        //              string: Text that will be sent to the main window
        public string ParseText(string Text, string Window)
        {
            return Text;
        }

        //Required for Plugin - ParseInput() is called when user enters text in the command box
        //Parameters:
        //              string Text:  The text the user entered in the command box
        //Return Value:
        //              string: Text that will be sent to the game
        public string ParseInput(string Text)
        {
            //Example commands to send plugin in Genie 
            //Can enable/disable loading an image from URL, otherwise defaults to Image Folder
            if (Text == "/landscape")
            {
                _host.SendText("#echo Landscape Plugin is Enabled.");
                return "";
            }

            //Enable loading DR-art pictures from URL
            if (Text == "/landscape enableURL")
            {
                _host.SendText("#echo Enabling option to load DR-Art from Play.net. Note: This is already enabled by default");
                enableURL = true;
                return "";
            }

            //Disable DR-Art loading from URL
            if (Text == "/landscape disableURL")
            {
                _host.SendText("#echo Disabling option to load DR-Art from Play.net");
                enableURL = false;
                return "";
            }

            return Text;
        }

        //Required for Plugin - ParseXML()
        //Parameters:
        //              string Text:  That "xml" text comes from the game
        public void ParseXML(string xml)
        {
            string temp_number = "not found";
            string local_image_name = "0";

            //Below code looks for a picture attribute in resource element of raw XML stream
            //Upon finding a number (other than 0), it will update the Landscape Form to load the image from a URL at play.net
            //On finding <resource picture="XXXX"/> in raw xml stream
            //Example: <resource picture="3012"/>
            if (xml.Contains("resource") && enableURL == true)
            {

                //Load XML from string xml
                XmlDocument resource_string = new XmlDocument();
                resource_string.PreserveWhitespace = true;
                try { resource_string.LoadXml(xml); }
                catch (System.IO.FileNotFoundException)
                {
                    MessageBox.Show("XML String isn't properly formatted XML");
                }

                //Load xml as xmlelement to search for picture attribute in resource element
                XmlElement resource_element = resource_string.DocumentElement;

                // Check to see if the resource element has a picture attribute.
                if (resource_element.HasAttribute("picture"))
                {
                    //Check for Day/Night Cycle
                    //if (_host.get_Variable("Time.isDay") == "0")
                    // _host.SendText("#echo >Log Currently Nighttime, might return no picture ID");

                    String picture_ID = resource_element.GetAttribute("picture");

                    //If a non-zero picture number is found, save it
                    if (picture_ID != "0")
                    {
                        temp_number = picture_ID;
                        //_host.SendText("#echo >Log Found picture for url:" + picture_ID);
                    }
                    //No valid picture number found from resource xml attribute
                    else temp_number = "not found";

                }
                //No resource element found in raw xml
                else temp_number = "not found";

                //Determine if picture attribute was found from resource element in raw xml
                if (temp_number != "not found" && temp_number != null && temp_number != "0")
                {
                    //Found picture number, set it to a jpg string name to pass to Landscape Image Viewer
                    local_image_name = temp_number + ".jpg";

                    //If the form hasn't been opened don't load picture number 
                    if (_form != null)
                    {
                        //Set the Landscape Form Image Viewer to the current picture number found in xml stream
                        _form.image_DRArt = local_image_name;
                        //_host.SendText("#echo >Log Loading Image from URL: " + local_image_name);
                    }
                }
                //If we dont find a picture number, disable showing DR-Art image from url, so it doesn't display Dr-Art images in form
                else if (temp_number == "not found")
                {
                    if (_form != null)
                    {
                        //Disable viewing the URL of the picture number in Landscape Image viewer
                        _form.image_DRArt = null;
                        _form.loaded_URL = false;
                    }
                }

            } //end look for resource XML element

            else temp_number = "not found";

        } // Parse XML

        //Required for Plugin - ParentClosing()
        public void ParentClosing()
        {

        }

        #endregion

        #region Custom Properties for your Plugin

        #endregion

        #region Custom Methods for your plugin

        //Opens the settings window.  Called when a user clicks on the menu item for 
        //this plugin (via above call)
        //
        //Parameters:
        //              Form Parent:  The parent form of the plugin.  Genie in this case
        public void OpenWindow(Form parent)
        {
            if (_form == null || _form.IsDisposed)
            {
                _form = new MainForm();
                _form.MdiParent = _parent;
            }
            _form.Show();
            _form.BringToFront();
        }


        //This can be removed if needed, used to compile a stand-alone plugin independent of launching Genie (for testing)
        //To compile as a stand-alone plug-in in Visual Basic (tested with VS 2019):
        //                   Under Properties, then Applications in Project, change compile from "Class Library" to "Windows Application"
        //                   "Class Library" compiles plug-in as DLL for Genie
        //                   "Windows Application" compiles plug-in as .EXE file (for testing outside of Genie)

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            
        }
        
        #endregion
    }

}
