using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace MusicPlayerApp
{
    public partial class Form1 : Form
    {
        private PlayMusic playMusic = new PlayMusic();

        //Create Global Variables of String Type Array to save the titles or name of the //Tracks and path of the track 
        String[] paths, files;

        

        public Form1()
        {
            InitializeComponent();
            customiseDesign();
        }
        private void customiseDesign()
        {
            panelMediaSubmenu.Visible = false;
            panelPlaylistsSubmenu.Visible = false;
        }

        private void hideSubMenu()
        {
            if (panelMediaSubmenu.Visible == true)
                panelMediaSubmenu.Visible = false;
            if (panelPlaylistsSubmenu.Visible == true)
                panelPlaylistsSubmenu.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        #region Media section
        private void btnMedia_Click(object sender, EventArgs e)
        {
            showSubMenu(panelMediaSubmenu);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // code to add new song
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Mp3 Files|*.mp3";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    playMusic.open(ofd.FileName);
                }
            }
            //hideSubMenu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openChildForm(new Form2());
            // code to search for songs

            //hideSubMenu();
        }
        #endregion

        #region Playlists section
        private void btnPlaylists_Click(object sender, EventArgs e)
        {
            showSubMenu(panelPlaylistsSubmenu);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //openChildForm(new Form3());
            // code to create playlist

            //hideSubMenu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // code to view playlist and manage

           // hideSubMenu();
        }
        #endregion

        #region Help section
        private void btnHelp_Click(object sender, EventArgs e)
        {
            // help button to show some text and contact 

            
        }
        #endregion

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            playMusic.play();
        }

        private void btnSelectSongs_Click(object sender, EventArgs e)
        {
            //Code to Select Songs 
            OpenFileDialog ofd = new OpenFileDialog();

            //Code to select multiple files 
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                files = ofd.SafeFileNames; //Save the names of the track in files array 
                paths = ofd.FileNames; //Save the paths of the tracks in path array 

                //Display the music titles in listbox 
                for (int i = 0; i < files.Length; i++)
                {
                    listBoxSongs.Items.Add(files[i]); //Display Songs in Listbox 
                }

            }
        }

        private void listBoxSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer1.URL = paths[listBoxSongs.SelectedIndex];
            }
            catch(IndexOutOfRangeException r)
            { }
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            ListBox.ObjectCollection list = listBoxSongs.Items;
            Random random = new Random();
            int w = list.Count;
            listBoxSongs.BeginUpdate();
            while (w > 1)
            {
                w--;
                int u = random.Next(w + 1);
                object value = list[u];
                list[u] = list[w];
                list[w] = value;
            }
            listBoxSongs.EndUpdate();
            listBoxSongs.Invalidate();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            playMusic.stop();
        }
    }
}
