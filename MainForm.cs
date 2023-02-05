using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Landscape4Genie
{
    public partial class MainForm : Form
    {
        string title_Connection;

        string imageFolder;
        string[] images;

        //These variables are static and are used to correlate zone with map name, they must be updated to be current with Automapper maps in Genie
        //These lists are keyed to the current Automapper Zone ID names. 
        string[] map_number = new string[] { "1", "1a", "1ab", "1e", "1j", "1l", "1m", "2", "2a", "2d", "3a", "4", "4a", "4a", "5", "5a", "6", "7", "7a", "7c", "8", "8a", "9a", "9b", "10", "11", "12", "12a", "13", "14b", "14c", "14d", "30", "30a", "30b", "31", "31a", "31b", "32", "33", "33a", "34", "34a", "35", "35a", "40", "40a", "41", "42", "47", "47a", "48", "50", "58", "59", "60", "61", "62", "62b", "63", "65", "66", "67", "67a", "68", "68a", "68b", "69", "90", "90a", "90b", "90c", "90d", "90f", "91a", "92", "93", "94", "95", "96", "97", "98", "98a", "99", "99a", "99b", "99c", "99d", "106", "107", "107a", "108", "113", "114", "116", "118e", "123", "126", "127", "128", "129", "132", "133", "134", "135", "136", "137", "138", "139", "140", "141", "150", "174", "500", "501", "502", "997", "998", "999", "6439", "TF1", "Unknown" };
        string[] map_name = new string[] { "The Crossing", "Crossing Thief", "Croc Maze", "Crossing Estate HQ", "Market Plaza", "Crossing Amuesment Pier", "Crossing, Jadewater Mansion", "Lake of Dreams", "Crossing Temple", "Escape Tunnels", "Temple Hill", "Crossing West Gate", "Tiger Clan: Gor'Tog Culture Faire 416 (2015)", "Tiger Clan Home", "Young Ogres and Scout Ogres", "Knife Clan", "Crossing North Gate and the Brambles", "Northern Trade Road", "Vineyard", "DiSilveron", "Crossing East Gate", "LostCrossing", "Rock Trolls", "Sorrow's Reach", "Abandoned Mine and Lairocott Brach", "Leucros, Vipers, and Rock Guardians", "Faenrae Reavers and WindHounds", "Misenseor Abbey", "Dirge", "Greater Fist", "Faldesu River", "Moon Mage Guild NTR", "Riverhaven", "Dunshade Manor", "Riverhaven Thief Passages", "Riverhaven East Gate", "Zaulfung", "Maelshyve's Fortress", "Riverhaven North Gate", "Riverhaven West Gate", "Road to Therenborough", "Mistwood Forest", "Rossman's Landing", "Old Throne City", "Throne City Thief", "Langenfirth and Surrounding Areas (to Therenborough)", "Siksraja ", "Kerteor, Fornsted, and Hraval ", "Therenborough and Keep", "Muspari ", "Muspari Thief Passages ", "Haizen Cugis (Windy Hamlet) ", "Segoltha River ", "Acenamacra Village ", "Boggy Wood ", "Southern Trade Road Part 1 (Ferry to Leth Deriel) ", "Leth Deriel ", "Southern Trade Route Part 2 (Leth to Gondola) ", "Tref Dinta ", "Oshu'ehhrsk Manor ", "Under the Gondola ", "Shard to Gondola Area ", "Shard ", "Shard Thief Passages ", "Shard South Gate ", "Ice Caves - Adan'f Spirit Dancers and Mages ", "Blackthorn Canyon, Lost Ground ", "Shard West Gate Hunting Area and Horse Clan ", "Ratha ", "Sand Sprites and Ochre Laheke ", "Ratha Sewers ", "Ehhrsk Highway ", "Taisgath ", "Basalt Isle and leihrems Barrow ", "Cavern of Glass ", "Reshalia Trade Road ", "Silver Leucros ", "Old Fields Road ", "Coastal Road ", "Pokekehekepi Beach ", "Korgi ", "Road to Aesry (Frostweavers, Snow Gobs) ", "Sea Caves ", "Aesry Surlaenis'a ", "Tethloren Island ", "Yvhh Latami and Snaer Hafwa Hunting Area ", "Aesry Necropolis ", "Aesry Highlands ", "Harajaal ", "MerKresh ", "Belarritaco Bay ", "Miss 112", "West Segoltha ", "Ain Ghazal ", "Hibarnhvidar ", "Firecats ", "Himineldar Shel ", "Hawstkaal Road ", "Boar Clan ", "Hunters Glade ", "Pilgramage Trail ", "Bloodvines ", "Adders Coil ", "Blighted Tangle ", "Dark Burrows ", "VelaTohr Overlook ", "Hag's Crag ", "Asketi's Mount ", "Adders Nest ", "Temple of the North Wind: Catacombs ", "Death Squirells ", "Fang Cove ", "Sleeping Dragon Corn Maze 436 (2020) ", "The Grey Raven Prison ", "Soul of Maelshyve ", "The Seacaves of Peri'el ", "Wyvern Arena ", "Transports ", "The Microcosm ", "Hollow Eve Festival 439 (2021) ", "The Arch ", "1"};

        //Tracks Zone Id with Map Name (based on Genie 4 Automapper Names)
        List<Map_Listing> trackList = new List<Map_Listing>();

        public string image_DRArt = null;
        public bool loaded_URL = false;

        public MainForm()
        {
            InitializeComponent();
            this.Text = "Landscape 4 Genie";
            title_Connection = this.Text;

            //Initialize Automapper list of maps names from Strings
            for (int i = 0; i < map_number.Length; i++)
            {
                Map_Listing tempList = new Map_Listing(map_number[i], map_name[i]);
                trackList.Add(tempList);
            }

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            loadImage_PictureBox();
            label_Zone.Text = "0";
            label_Room.Text = "0";
            label_mapName.Text = "Unknown";
            label_roomName.Text = "Unknown";

        }

        //initialize Image Directory, load image names 
        private void loadImage_PictureBox()
        {
            //Get path for Image Directory (currently Genie \Plugins\Images directory)
            string currentDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            if (currentDirectory != null) imageFolder = Path.Combine(currentDirectory, "Plugins\\Images");

            //Load all images from the Images Directory with jpg ending
            if (System.IO.Directory.Exists(imageFolder))
                images = Directory.GetFiles(imageFolder, "*.jpg");
            //If no Images folder found, output error
            else MessageBox.Show("Error: No Images Folder found in your Genie Plugin Directory. Please ensure there is an Image Folder in your Plugin Directory", "Error: Landscape 4Genie Image Folder Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            //Load default on startup
            LoadDefaultImage();

        }

        /// <summary>
        /// Update Image Viewer when zoneId or roomID change (called from Landscape.cs)
        /// </summary>
        public void Update_Image()
        {

            //Wait for Connection to Game
            if (Landscape._host.get_Variable("connected") == "0")
            {
                this.Text = title_Connection + ": Waiting for Connection";
                label_mapName.Text = "Unknown";
                label_roomName.Text = "Unknown";

                //Revert to default image if disconnected
                LoadDefaultImage();
            }

            //If connected to game, update called from Plugin when Zone ID and Room ID change
            else if (Landscape._host.get_Variable("connected") == "1")
            {
                this.Text = title_Connection + ": Connected";

                    // Get Zone and Room ID      
                    label_Zone.Text = Landscape._host.get_Variable("zoneid");
                    label_Room.Text = Landscape._host.get_Variable("roomid");
                    label_roomName.Text = Landscape._host.get_Variable("roomname");

                    //Find Image following string format "ZoneID_RoomID.png" ex: 1_42.png
                    string temp_room = label_Zone.Text + "_" + label_Room.Text + ".jpg";

                    //Find Image of default zone "ZoneID.png" ex: 1.png
                    string temp_zone = label_Zone.Text + ".jpg";

                    //Find the Genie Automapper Name for the Zone
                    string truncate_name = trackList.Find(trackList => trackList.map_index == Landscape._host.get_Variable("zoneid"))?.map_name.ToString();
                    
                    //Truncate Name so it fits in label on MainForm
                    truncate_name = truncate_name.Truncate(24);
                    label_mapName.Text = truncate_name; 
                                      
                    if (label_mapName.Text == null) label_mapName.Text = "Map Name (not found)"; 

                    //If a DR-art image is found in XML stream from game, load it from the url play.net
                    if (image_DRArt != null)
                    {

                        //Check to see if image from URL is stored locally so it doesn't need to be downloaded
                        int temp_Index = Array.FindIndex(images, images => images.Contains(imageFolder + "\\" + image_DRArt));
                        
                        if (temp_Index == -1)
                        {
                            //Image not found, so load from URL
                            if (loaded_URL == false)
                            {
                                 
                                string url_DR_art = "https://www.play.net/bfe/DR-art/" + image_DRArt;

                                if (url_DR_art != null)
                                {
                                    pictureBox_Image.Load(url_DR_art);
                                    loaded_URL = true;
                                }

                                //Save the image to local for future use
                                using (WebClient client = new WebClient())
                                {
                                    client.DownloadFile(url_DR_art, imageFolder + "\\" + image_DRArt);
                                }
                            }

                        }

                        //Load Dr-Art image if it's found stored locally
                        else
                        {
                            pictureBox_Image.Load(images[temp_Index]);
                            loaded_URL = true;
                        }
                    }

                    //Otherwise, find Zone and Room Image in Images Folder and load into Plugin
                    else if (images.Contains(imageFolder + "\\" + temp_room))
                    {
                        int temp_Index = Array.FindIndex(images, images => images.Contains(imageFolder + "\\" + temp_room));
                        pictureBox_Image.Load(images[temp_Index]);
                    }

                    //Fallback - Find Default Zone Image and load from Images Folder into Plugin if no image is found
                    else if (images.Contains(imageFolder + "\\" + temp_zone))
                    {
                        int temp_Index = Array.FindIndex(images, images => images.Contains(imageFolder + "\\" + temp_zone));
                        pictureBox_Image.Load(images[temp_Index]);
                    }
            }
        } //Update Image

        /// <summary>
        /// Load Default Image at Images\default.jpg
        /// </summary>
        public void LoadDefaultImage()
        {
            try
            {
                int temp_Index = Array.FindIndex(images, images => images.Contains(imageFolder + "\\default.jpg"));
                if (temp_Index > 0)
                    pictureBox_Image.Load(images[temp_Index]);
                else pictureBox_Image.Load(images[0]);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to load Images from Image Directory", "Error: Landscape 4Genie Plugin",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        } //Load Default Image

    } //MainForm

    //Class for holding map names and zone IDs from Automapper
    public class Map_Listing
    {
         public string map_index { get; set; }
         public string map_name { get; set; }

        public Map_Listing(string temp_index, string temp_name)
        {
            map_index = temp_index;
            map_name = temp_name;
        }
    }

    public static class StringExtensions
    {

        /// <summary>
        /// Truncates string so that it is no longer than the specified number of characters.
        /// </summary>
        /// <param name="str">String to truncate.</param>
        /// <param name="length">Maximum string length.</param>
        /// <returns>Original string or a truncated one if the original was too long.</returns>
        public static string Truncate(this string str, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
            }

            if (str == null)
            {
                return null;
            }

            int maxLength = Math.Min(str.Length, length);
            return str.Substring(0, maxLength);
        }
    }

}
