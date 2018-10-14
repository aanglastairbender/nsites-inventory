using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using NSites.Global;
using NSites.ApplicationObjects.Classes;

namespace NSites.ApplicationObjects.UserInterfaces.Systems
{
    public partial class ScreenSaverUI : Form
    {
        #region "VARIABLES"
        SystemConfiguration loSystemConfiguration;
        string lChosenFile = "";
        string lTargetFile = "";
        string lScreenSaverImage = "";
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public ScreenSaverUI()
        {
            InitializeComponent();
            loSystemConfiguration = new SystemConfiguration();
        }
        #endregion "END OF CONSTRUCTORS"

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "EVENTS"
        private void ScreenSaverUI_Load(object sender, EventArgs e)
        {
            try
            {
                byte[] hextobyte = GlobalFunctions.HexToBytes(GlobalVariables.ScreenSaverImage);
                pctCurrentPicture.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(hextobyte);
                pctCurrentPicture.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch
            { }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
            }
            catch
            {
                this.Close();
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.InitialDirectory = ".../Main/ScreenSaverImages/";
            openFD.Title = "Insert an Image";
            openFD.Filter = "JPEG Images|*.jpg|GIF Images|*.gif|PNG Images|*.png";
            if (openFD.ShowDialog() == DialogResult.Cancel)
            {
                MessageBoxUI mb = new MessageBoxUI("Operation Cancelled", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
            }
            else
            {
                lChosenFile = openFD.FileName;
                string _FileName = openFD.SafeFileName;
                byte[] m_Bitmap = null;

                FileStream fs = new FileStream(lChosenFile, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                int length = (int)br.BaseStream.Length;
                m_Bitmap = new byte[length];
                m_Bitmap = br.ReadBytes(length);
                br.Close();
                fs.Close();

                lScreenSaverImage = GlobalFunctions.ToHex(m_Bitmap);

                pctCurrentPicture.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(m_Bitmap);
                pctCurrentPicture.BackgroundImageLayout = ImageLayout.Stretch;

                byte[] hextobyte = GlobalFunctions.HexToBytes(GlobalVariables.ScreenSaverImage);
                pctPreviousPicture.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(hextobyte);
                pctPreviousPicture.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmScreenSaver", "Save"))
            {
                return;
            }
            loSystemConfiguration.Value = lScreenSaverImage;
            loSystemConfiguration.Key = "ScreenSaverImage";
            if (loSystemConfiguration.saveSystemConfiguration(GlobalVariables.Operation.Edit))
            {
                GlobalVariables.ScreenSaverImage = lScreenSaverImage;
                MessageBoxUI ms = new MessageBoxUI("Screen Saver has been change successfully!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                ms.ShowDialog();
                ParentList.GetType().GetMethod("changeHomeImage").Invoke(ParentList, null);
                ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
            }
        }

        #endregion "END OF EVENTS"

        private void btnNoPicture_Click(object sender, EventArgs e)
        {
            lScreenSaverImage = "";
        }
    }
}
