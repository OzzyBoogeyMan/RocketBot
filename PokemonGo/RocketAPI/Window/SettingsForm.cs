﻿using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;

namespace PokemonGo.RocketAPI.Window
{
    internal partial class SettingsForm : Form
    {
        private readonly ToolTip toolTip1 = new ToolTip();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            authTypeCb.Text = Settings.Instance.AuthType.ToString();
            if (authTypeCb.Text == "google")
            {
                UserLoginBox.Text = Settings.Instance.Email;
                UserPasswordBox.Text = Settings.Instance.Password;
            }
            else
            {
                UserLoginBox.Text = Settings.Instance.PtcUsername;
                UserPasswordBox.Text = Settings.Instance.PtcPassword;
            }
            latitudeText.Text = Settings.Instance.DefaultLatitude.ToString();
            longitudeText.Text = Settings.Instance.DefaultLongitude.ToString();
            razzmodeCb.Text = Settings.Instance.RazzBerryMode;
            razzSettingText.Text = Settings.Instance.RazzBerrySetting.ToString();
            transferTypeCb.Text = Settings.Instance.TransferType;
            transferCpThresText.Text = Settings.Instance.TransferCPThreshold.ToString();
            transferIVThresText.Text = Settings.Instance.TransferIVThreshold.ToString();
            evolveAllChk.Checked = Settings.Instance.EvolveAllGivenPokemons;
            CatchPokemonBox.Checked = Settings.Instance.CatchPokemon;
            TravelSpeedBox.Text = Settings.Instance.TravelSpeed.ToString();
            // ImageSizeBox.Text = Settings.Instance.ImageSize.ToString();
            // Initialize map:
            //use google provider
            gMapControl1.MapProvider = GoogleMapProvider.Instance;
            //get tiles from server only
            gMapControl1.Manager.Mode = AccessMode.ServerOnly;
            //not use proxy
            GMapProvider.WebProxy = null;
            //center map on moscow
            var lat = ConfigurationManager.AppSettings["DefaultLatitude"];
            var longit = ConfigurationManager.AppSettings["DefaultLongitude"];
            lat.Replace(',', '.');
            longit.Replace(',', '.');
            gMapControl1.Position = new PointLatLng(double.Parse(lat.Replace(",", "."), CultureInfo.InvariantCulture),
                double.Parse(longit.Replace(",", "."), CultureInfo.InvariantCulture));


            //zoom min/max; default both = 2
            gMapControl1.DragButton = MouseButtons.Left;

            gMapControl1.CenterPen = new Pen(Color.Red, 2);
            gMapControl1.MinZoom = trackBar.Maximum = 1;
            gMapControl1.MaxZoom = trackBar.Maximum = 20;
            trackBar.Value = 10;

            //set zoom
            gMapControl1.Zoom = trackBar.Value;

            //disable map focus
            gMapControl1.DisableFocusOnMouseEnter = true;

            transferTypeCb.DrawMode = DrawMode.OwnerDrawFixed;
            transferTypeCb.DrawItem += transferTypeCb_DrawItem;
            transferTypeCb.DropDownClosed += transferTypeCb_DropDownClosed;

            toolTip1.SetToolTip(transferTypeCb, "test1");
        }


        private void saveBtn_Click(object sender, EventArgs e)
        {
            Settings.Instance.SetSetting(authTypeCb.Text, "AuthType");
            if (authTypeCb.Text == "google")
            {
                Settings.Instance.SetSetting(UserLoginBox.Text, "Email");
                Settings.Instance.SetSetting(UserPasswordBox.Text, "Password");
            }
            else
            {
                Settings.Instance.SetSetting(UserLoginBox.Text, "PtcUsername");
                Settings.Instance.SetSetting(UserPasswordBox.Text, "PtcPassword");
            }
            Settings.Instance.SetSetting(latitudeText.Text.Replace(',', '.'), "DefaultLatitude");
            Settings.Instance.SetSetting(longitudeText.Text.Replace(',', '.'), "DefaultLongitude");

            var lat = ConfigurationManager.AppSettings["DefaultLatitude"];
            var longit = ConfigurationManager.AppSettings["DefaultLongitude"];
            lat.Replace(',', '.');
            longit.Replace(',', '.');


            Settings.Instance.SetSetting(razzmodeCb.Text, "RazzBerryMode");
            Settings.Instance.SetSetting(razzSettingText.Text, "RazzBerrySetting");
            Settings.Instance.SetSetting(transferTypeCb.Text, "TransferType");
            Settings.Instance.SetSetting(transferCpThresText.Text, "TransferCPThreshold");
            Settings.Instance.SetSetting(transferIVThresText.Text, "TransferIVThreshold");
            Settings.Instance.SetSetting(TravelSpeedBox.Text, "TravelSpeed");
            //Settings.Instance.SetSetting(ImageSizeBox.Text, "ImageSize");
            Settings.Instance.SetSetting(evolveAllChk.Checked ? "true" : "false", "EvolveAllGivenPokemons");
            Settings.Instance.SetSetting(CatchPokemonBox.Checked ? "true" : "false", "CatchPokemon");
            Settings.Instance.Reload();

            MainForm.ResetMap();
            Close();
        }


        private void authTypeCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (authTypeCb.Text == "google")
            {
                UserLabel.Text = "Email:";
            }
            else
            {
                UserLabel.Text = "Username:";
            }
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            var localCoordinates = e.Location;
            gMapControl1.Position = gMapControl1.FromLocalToLatLng(localCoordinates.X, localCoordinates.Y);

            if (e.Clicks >= 2)
            {
                gMapControl1.Zoom += 5;
            }

            var X = Math.Round(gMapControl1.Position.Lng, 6);
            var Y = Math.Round(gMapControl1.Position.Lat, 6);
            var longitude = X.ToString();
            var latitude = Y.ToString();
            latitudeText.Text = latitude;
            longitudeText.Text = longitude;
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            gMapControl1.Zoom = trackBar.Value;
        }

        private void FindAdressButton_Click(object sender, EventArgs e)
        {
            gMapControl1.SetPositionByKeywords(AdressBox.Text);
            gMapControl1.Zoom = 15;
        }

        private void authTypeLabel_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void transferCpThresText_TextChanged(object sender, EventArgs e)
        {
        }

        private void transferTypeCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (transferTypeCb.Text == "CP")
            {
                CPThresholdLabel.Visible = true;
                transferCpThresText.Visible = true;
            }
            else
            {
                CPThresholdLabel.Visible = false;
                transferCpThresText.Visible = false;
            }

            if (transferTypeCb.Text == "IV")
            {
                IVThresholdLabel.Visible = true;
                transferIVThresText.Visible = true;
            }
            else
            {
                IVThresholdLabel.Visible = false;
                transferIVThresText.Visible = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
        }

        private void FindAdressButton_Click_1(object sender, EventArgs e)
        {
            gMapControl1.SetPositionByKeywords(AdressBox.Text);
            gMapControl1.Zoom = 15;
            var X = Math.Round(gMapControl1.Position.Lng, 6);
            var Y = Math.Round(gMapControl1.Position.Lat, 6);
            var longitude = X.ToString();
            var latitude = Y.ToString();
            latitudeText.Text = latitude;
            longitudeText.Text = longitude;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void evolveAllChk_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void TravelSpeedBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void razzSettingText_KeyPress(object sender, KeyPressEventArgs e)
        {
            var ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void AdressBox_Leave(object sender, EventArgs e)
        {
            if (AdressBox.Text.Length == 0)
            {
                AdressBox.Text = "Enter an address or a coordinate";
                AdressBox.ForeColor = SystemColors.GrayText;
            }
        }

        private void AdressBox_Enter(object sender, EventArgs e)
        {
            if (AdressBox.Text == "Enter an address or a coordinate")
            {
                AdressBox.Text = "";
                AdressBox.ForeColor = SystemColors.WindowText;
            }
        }

        private void AdressBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void transferTypeCb_DropDownClosed(object sender, EventArgs e)
        {
            toolTip1.Hide(transferTypeCb);
        }

        private void transferTypeCb_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            } // added this line thanks to Andrew's comment
            string tip;
            switch (transferTypeCb.GetItemText(transferTypeCb.Items[e.Index]))
            {
                case "Leave Strongest":
                    tip = "N/A";
                    break;
                case "All":
                    tip = "Transfer all pokemons";
                    break;
                case "CP Duplicate":
                    tip = "Only keeping the highest CP pokemon of each type";
                    break;
                case "IV Duplicate":
                    tip = "Only keeping the highest IV pokemon of each type";
                    break;
                case "CP/IV Duplicate":
                    tip = "Keeping one highest pokemon CP and one highest IV pokemon of each type";
                    break;
                case "CP":
                    tip = "Only keeping pokemons that have a CP higher than the threshold";
                    break;
                case "IV":
                    tip = "Only keeping pokemons that have a IV higher than the threshold";
                    break;
                default:
                    tip = "Not transfering any pokemons";
                    break;
            }
            e.DrawBackground();
            using (var br = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(transferTypeCb.GetItemText(transferTypeCb.Items[e.Index]), e.Font, br, e.Bounds);
            }
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                toolTip1.Show(tip, transferTypeCb, e.Bounds.Right, e.Bounds.Bottom);
            }
            e.DrawFocusRectangle();
        }
    }
}