using Ionic.Zip;
using System;
using System.Drawing;
using System.IO;
namespace project
{
    class Stego
    {
        public string photoLocation = "";
        public string fileLocation = "";
        public string locationToSave = "";
        public string password = "";
        public void HideFile(Bitmap container)
        {
            try
            {
                MemoryStream fileToHide = EcryptTheFile(fileLocation, password);
                byte[] bSize = fileToHide.ToArray();
                int iSize = bSize.Length;
                int r = (iSize & Convert.ToInt32("111111110000000000000000", 2)) >> 16;
                int g = (iSize & Convert.ToInt32("000000001111111100000000", 2)) >> 8;
                int b = (iSize & Convert.ToInt32("000000000000000011111111", 2)) >> 0;
                container.SetPixel(0, 0, Color.FromArgb(r, g, b));
                int[] z = new int[2];
                z[0] = 1;
                z[1] = 0;
                Color basic = container.GetPixel(z[0], z[1]);
                byte rColor, gColor, bColor;
                for (int i = 0; i < bSize.Length; i++)
                {
                    int maskR = Convert.ToInt32("11111000", 2);
                    int maskG = Convert.ToInt32("11111000", 2);
                    int maskB = Convert.ToInt32("11111100", 2);
                    int maskR1 = Convert.ToInt32("00000111", 2);
                    int maskG1 = Convert.ToInt32("00111000", 2);
                    int maskB1 = Convert.ToInt32("11000000", 2);
                    rColor = (byte)((basic.R & maskR) | ((bSize[i] & maskR1) >> 0));
                    gColor = (byte)((basic.G & maskG) | ((bSize[i] & maskG1) >> 3));
                    bColor = (byte)((basic.B & maskB) | ((bSize[i] & maskB1) >> 6));
                    Color final = Color.FromArgb(rColor, gColor, bColor);
                    container.SetPixel(z[0], z[1], final);
                    NextPixel(z, container);
                    basic = container.GetPixel(z[0], z[1]);
                }
                System.Windows.Forms.MessageBox.Show("Done!");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Critical error oquired");
                GC.Collect();
                System.Environment.Exit(1);
            }
        }
        void NextPixel(int[] z, Bitmap image)
        {
            z[0]++;
            if (z[0] == image.Size.Width)
            {
                z[0] = 0;
                z[1]++;
            }
        }
        public void UnhideFile(Bitmap container)
        {
            try
            {
                Color placeToCheck = container.GetPixel(0, 0);
                int iSize = (placeToCheck.R << 16) | (placeToCheck.G << 8) | (placeToCheck.B << 0);
                MemoryStream hiddenFile = new MemoryStream(iSize);
                int[] z = new int[2];
                z[0] = 1;
                z[1] = 0;
                byte hidden = 0;
                placeToCheck = container.GetPixel(z[0], z[1]);
                int maskR = Convert.ToInt32("00000111", 2);
                int maskG = Convert.ToInt32("00000111", 2);
                int maskB = Convert.ToInt32("00000011", 2);
                for (int i = 0; i < iSize; i++)
                {
                    hidden = (byte)(hidden | ((placeToCheck.R & maskR) << 0));
                    hidden = (byte)(hidden | ((placeToCheck.G & maskG) << 3));
                    hidden = (byte)(hidden | ((placeToCheck.B & maskB) << 6));
                    NextPixel(z, container);
                    placeToCheck = container.GetPixel(z[0], z[1]);
                    hiddenFile.WriteByte(hidden);
                    hidden = 0;
                }
                hiddenFile.Seek(0, SeekOrigin.Begin);
                DecryptTheFile(hiddenFile, password, fileLocation);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "An error oquired");
            }
        }
        MemoryStream EcryptTheFile(string nameOfTheFile, string passwordToEncrypt)
        {
            MemoryStream fileToHide = new MemoryStream();
            ZipFile encryptedContainer = new ZipFile();
            encryptedContainer.Password = passwordToEncrypt;
            encryptedContainer.Encryption = EncryptionAlgorithm.WinZipAes256;
            encryptedContainer.AddFile(nameOfTheFile, "");
            encryptedContainer.Save(fileToHide);
            fileToHide.Seek(0, SeekOrigin.Begin);
            return fileToHide;
        }
        void DecryptTheFile(MemoryStream hiddenFile, string passwordToDecrypt, string pathToSave)
        {
            try
            {
                ZipFile encryptedContainer = ZipFile.Read(hiddenFile);
                encryptedContainer[0].ExtractWithPassword(pathToSave, passwordToDecrypt);
                System.Windows.Forms.MessageBox.Show("Done!");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "An error oquired");
            }
        }
    }
}
