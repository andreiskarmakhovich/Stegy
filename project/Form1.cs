

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Drawing.Imaging;
using Ionic.Zip;
namespace project
{
    public partial class Form1 : Form
    {
        //TODO: переименовать кнопки и текстбоксы, добавить дефолтный пароль
        readonly Stego steg = new Stego();
        public Form1()
        {
            InitializeComponent();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button3.Text = "Choose the file";
            button4.Visible = true;
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            OpenFileDialog choosePhoto = new OpenFileDialog();
            choosePhoto.Filter = "Choose the photo (PNG)|*.png";
            choosePhoto.RestoreDirectory = true;
            if (choosePhoto.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(choosePhoto.FileName);
                steg.photoLocation = choosePhoto.FileName;
            }
            if (steg.photoLocation != "")
                checkBox1.Checked = true;
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                OpenFileDialog chooseFile = new OpenFileDialog();
                chooseFile.Filter = "All (*.*)|*.*";
                chooseFile.RestoreDirectory = true;
                if (chooseFile.ShowDialog() == DialogResult.OK)
                {
                    steg.fileLocation = chooseFile.FileName;
                }
            }
            else
            {
                FolderBrowserDialog choosePath = new FolderBrowserDialog();
                if (choosePath.ShowDialog() == DialogResult.OK)
                {
                    steg.fileLocation = choosePath.SelectedPath;
                }
            }
            if (steg.fileLocation != "")
                checkBox2.Checked = true;
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog choosePhoto = new SaveFileDialog();
            choosePhoto.Filter = "Choose the photo (PNG)|*.png";
            choosePhoto.RestoreDirectory = true;
            if (choosePhoto.ShowDialog() == DialogResult.OK)
            {
                steg.locationToSave = choosePhoto.FileName;
            }
            if (steg.locationToSave != "")
                checkBox3.Checked = true;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
                steg.password = textBox1.Text;
            else
                steg.password = "1";
            textBox1.Clear();
            if (radioButton1.Checked == true)
            {
                Bitmap container = new Bitmap(pictureBox1.Image);
                steg.HideFile(container);
                pictureBox2.Image = container;
                container.Save(steg.locationToSave);
                MessageBox.Show("Done!");
            }
            else
            {
                steg.UnhideFile((Bitmap)pictureBox1.Image);
            }
            steg.fileLocation = "";
            steg.photoLocation = "";
            steg.locationToSave = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            GC.Collect();
        }
        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            button4.Visible = !button4.Visible;
            button3.Text = "Save";
            checkBox3.Visible = !checkBox3.Visible;
        }
    }
}
