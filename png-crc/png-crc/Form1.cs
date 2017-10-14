using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace png_crc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates the CRC table for calculating a 32-bit CRC.
        /// </summary>
        private static void CreateCrcTable()
        {
            uint c;
            int k;
            int n;

            for (n = 0; n < 256; n++)
            {
                c = (uint)n;

                for (k = 0; k < 8; k++)
                {
                    if ((c & 1) == 1)
                    {
                        c = 0xedb88320 ^ (c >> 1);
                    }
                    else
                    {
                        c = c >> 1;
                    }
                }
                CrcTable[n] = c;
            }
            IsTableCreated = true;
        }
        static uint[] CrcTable = new uint[256];
        static bool IsTableCreated = false;

        /// <summary>
        /// Calculates an array of 4 bytes containing the calculated CRC.
        /// </summary>
        /// <param name="buf">The raw data on which to calculate the CRC.</param>
        public static byte[] GetCrc(byte[] buffer)
        {
            uint data = 0xFFFFFFFF;
            int n;

            if (!IsTableCreated)
                CreateCrcTable();

            for (n = 0; n < buffer.Length; n++)
                data = CrcTable[(data ^ buffer[n]) & 0xff] ^ (data >> 8);

            data = data ^ 0xFFFFFFFF;

            byte b1 = Convert.ToByte(data >> 24);
            byte b2 = Convert.ToByte(b1 << 8 ^ data >> 16);
            byte b3 = Convert.ToByte(((data >> 16 << 16) ^ (data >> 8 << 8)) >> 8);
            byte b4 = Convert.ToByte((data >> 8 << 8) ^ data);

            return new byte[] { b1, b2, b3, b4 };
        }

        private void cmCreate_Click(object sender, EventArgs e)
        {
            FileStream imageFS = new FileStream(imSrc.Text, FileMode.Open, FileAccess.Read);
            FileStream chunkFS = new FileStream(tgSrc.Text, FileMode.Open, FileAccess.Read);
            FileStream writeFS = new FileStream(imDst.Text, FileMode.Create);
            byte[] imageData = new byte[imageFS.Length - 12]; //Image until EOF-tag
            byte[] imageEOFt = new byte[12]; //EOF tag (yeah, I'm really lazy)
            byte[] chunkData = new byte[chunkFS.Length]; //The tag metadata
            byte[] chunkDLen = System.BitConverter.GetBytes((uint)chunkData.Length - 4);
            //The length counts only the data field, not itself, the chunk type, or the CRC.

            imageFS.Read(imageData, 0, imageData.Length);
            imageFS.Read(imageEOFt, 0, imageEOFt.Length); imageFS.Close(); imageFS.Dispose();
            chunkFS.Read(chunkData, 0, chunkData.Length); chunkFS.Close(); chunkFS.Dispose();
            byte[] chunkHash = GetCrc(chunkData);
            Array.Reverse(chunkDLen); //Stupid C#.
            writeFS.Write(imageData, 0, imageData.Length);
            writeFS.Write(chunkDLen, 0, chunkDLen.Length);
            writeFS.Write(chunkData, 0, chunkData.Length);
            writeFS.Write(chunkHash, 0, chunkHash.Length);
            writeFS.Write(imageEOFt, 0, imageEOFt.Length);
            writeFS.Flush(); writeFS.Close(); writeFS.Dispose();
            MessageBox.Show("Done!");

            // The first working beta (on first try, holy shit)
            /*FileStream fi = new FileStream(imSrc.Text, FileMode.Open, FileAccess.Read);
            FileStream fa = new FileStream(tgSrc.Text, FileMode.Open, FileAccess.Read);
            FileStream fo = new FileStream(imDst.Text, FileMode.Create);
            byte[] bi = new byte[fi.Length - 12]; //File before EOF
            byte[] ba = new byte[4]; //Tag length (yeah I'm lazy)
            byte[] bb = new byte[fa.Length-ba.Length]; //Clean tags
            byte[] bo = new byte[12]; //EOF tag
            fi.Read(bi, 0, bi.Length);
            fi.Read(bo, 0, bo.Length);
            fa.Read(ba, 0, ba.Length);
            fa.Read(bb, 0, bb.Length);
            fo.Write(bi, 0, bi.Length);
            fo.Write(ba, 0, ba.Length);
            fo.Write(bb, 0, bb.Length);
            byte[] hash = GetCrc(bb);
            fo.Write(hash, 0, hash.Length);
            fo.Write(bo, 0, bo.Length);
            fi.Close(); fa.Close();
            fo.Flush(); fo.Close();
            MessageBox.Show("Done!");*/
        }

        private void cmBuild_Click(object sender, EventArgs e)
        {
            XMP xmp = new XMP();
            string gen = xmp.Make(txName.Text, txDesc.Text, new string[]{
                txTagsGen.Text, txTagsSrc.Text, txTagsChr.Text, txTagsArt.Text});
        }

        private void Form1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string sFile = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            if (optWriteXMP.Checked)
            {
                string sOut = sFile.Substring(0, sFile.Length - 4) + "_tagged.png";
                XMP xmp = new XMP();
                string sXmp = xmp.Make(txName.Text, txDesc.Text, new string[]{
                txTagsGen.Text, txTagsSrc.Text, txTagsChr.Text, txTagsArt.Text});
                xmp.WritePNG(sFile, sOut, sXmp);
                MessageBox.Show("Done!", "PNG-XMP",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            if (optReadXMP.Checked)
            {
                XMP xmp = new XMP();
                byte[] buffer = File.ReadAllBytes(sFile);
                tags t = xmp.Read(buffer);
                MessageBox.Show("Tags:" + "\r\n" +
                    "Name: " + t.Titl + "\r\n" +
                    "Desc: " + t.Desc + "\r\n" +
                    "\r\n" +
                    "tGen: " + t.tGen + "\r\n" +
                    "tSrc: " + t.tSrc + "\r\n" +
                    "tChr: " + t.tChr + "\r\n" +
                    "tArt: " + t.tArt + "\r\n" +
                    "\r\n" +
                    "tOth: " + t.tOth);
            }
        }
    }
}
