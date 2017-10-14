using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace png_crc
{
    class tags
    {
        public string Titl = "";
        public string Desc = "";
        public string tGen = "";
        public string tSrc = "";
        public string tChr = "";
        public string tArt = "";
        public string tOth = "";
    }
    class XMP
    {
        private string[] sTagOrder = new string[] { "tGen", "tSrc", "tChr", "tArt" };
        private int[] Find(byte[] imaeg, int ofs)
        {
            //Finds and strips raw XMP data from any file format.
            //Returns first raw XMP data found.
            // 0.0 = No XMP tags identified
            // n.0 = Unexpected end of XMP
            int[] o = new int[] { -1, -1 };
            byte[] ofsID = toRAW("W5M0MpCehiHzreSzNTczkc9d");
            byte[] lenID = toRAW("<?xpacket end=");

            for (int a = ofs; a < imaeg.Length; a++)
                if (bCmpBytes(imaeg, a, ofsID))
                {
                    o[0] = a + ofsID.Length; break;
                }
            if (o[0] == -1) return o;
            for (int a = o[0]; a < imaeg.Length; a++)
                if (bCmpBytes(imaeg, a, lenID))
                {
                    o[1] = a - o[0]; break;
                }
            return o;
        }
        private int[][] FindAll(byte[] imaeg)
        {
            int ofs = 0;
            ArrayList ranges = new ArrayList();
            while (ofs < imaeg.Length)
            {
                int[] newrange = FindPNG(imaeg, ofs);
                if (newrange[1] == -1) break;
                ofs = newrange[1] + 1;
                ranges.Add(newrange);
            }
            int[][] ret = new int[ranges.Count][];
            for (int a = 0; a < ret.Length; a++)
                ret[a] = (int[])ranges[a];
            return ret;
        }
        public tags Read(byte[] imaeg)
        {
            //Finds and parses raw XMP data from any file format.
            //Returns formatted XMP tags from first raw XMP data.
            tags ret = new tags();
            int[] ofs = Find(imaeg, 0);
            if (ofs[0] == -1) return ret;
            if (ofs[1] == -1)
            {
                ret.Titl = "unk!fmt";
                return ret;
            }
            string sXmp = unRAW(imaeg, ofs[0], ofs[1]);
            //System.Windows.Forms.MessageBox.Show(sXmp);
            ret.Titl = Parse(sXmp, "title")[0];
            ret.Desc = Parse(sXmp, "description")[0];
            string[] sRawTags = Parse(sXmp, "subject");
            for (int a = 0; a < sRawTags.Length; a++)
            {
                if (sRawTags[a].StartsWith("tGen!"))
                    ret.tGen += sRawTags[a].Substring(5) + ", ";
                else if (sRawTags[a].StartsWith("tSrc!"))
                    ret.tSrc += sRawTags[a].Substring(5) + ", ";
                else if (sRawTags[a].StartsWith("tChr!"))
                    ret.tChr += sRawTags[a].Substring(5) + ", ";
                else if (sRawTags[a].StartsWith("tArt!"))
                    ret.tArt += sRawTags[a].Substring(5) + ", ";
                else ret.tOth += sRawTags[a] + ", ";
            }
            if (ret.tGen.EndsWith(", ")) ret.tGen =
                ret.tGen.Substring(0, ret.tGen.Length - 2);
            if (ret.tSrc.EndsWith(", ")) ret.tSrc =
                ret.tSrc.Substring(0, ret.tSrc.Length - 2);
            if (ret.tChr.EndsWith(", ")) ret.tChr =
                ret.tChr.Substring(0, ret.tChr.Length - 2);
            if (ret.tArt.EndsWith(", ")) ret.tArt =
                ret.tArt.Substring(0, ret.tArt.Length - 2);
            if (ret.tOth.EndsWith(", ")) ret.tOth =
                ret.tOth.Substring(0, ret.tOth.Length - 2);
            return ret;
        }
        private int[] FindPNG(byte[] imaeg, int ofs)
        {
            //Finds exact XMP offset in PNG type images.
            //Returns int-array containing start-offset and length.
            int[] o = new int[] { -1, -1 };
            byte[] ofsID = toRAW("iTXtXML:com.adobe.xmp");
            byte[] lenID = toRAW("<?xpacket end=");
            byte[] lenI2 = toRAW("?>");

            for (int a = ofs; a < imaeg.Length; a++)
                if (bCmpBytes(imaeg, a, ofsID))
                {
                    o[0] = a - 4; break;
                }
            if (o[0] == -1) return o;
            for (int a = o[0]; a < imaeg.Length; a++)
                if (bCmpBytes(imaeg, a, lenID))
                {
                    o[1] = a + lenID.Length; break;
                }
            if (o[1] == -1) return o;
            for (int a = o[1]; a < imaeg.Length; a++)
                if (bCmpBytes(imaeg, a, lenI2))
                {
                    o[1] = a + 4 - o[0] + lenI2.Length; break;
                }
            return o;
        }
        private int[][] FindAllPNG(byte[] imaeg)
        {
            int ofs = 0;
            ArrayList ranges = new ArrayList();
            while (ofs < imaeg.Length)
            {
                int[] newrange = FindPNG(imaeg, ofs);
                ofs = newrange[0] + newrange[1] + 1;
                if (newrange[1] == -1) break;
                ranges.Add(newrange);
            }
            int[][] ret = new int[ranges.Count][];
            for (int a = 0; a < ret.Length; a++)
                ret[a] = (int[])ranges[a];
            return ret;
        }
        private string[] Parse(string sXmp, string sType)
        {
            //Parses and returns the values of a specified XMP tag type.
            string ret = sXmp;
            if (!ret.Contains("</dc:" + sType + ">"))
                return new string[] { "" };

            ret = ret.Substring(ret.IndexOf("<dc:" + sType + ">")
                + sType.Length + "<dc:>".Length);
            ret = ret.Substring(0, ret.IndexOf("</dc:" + sType + ">"));
            ret = ret.Substring(ret.IndexOf("<rdf:li") + 1);
            string[] aret;
            aret = SplitRem(ret, "<rdf:li");
            for (int a = 0; a < aret.Length; a++)
            {
                aret[a] = aret[a].Substring(aret[a].IndexOf(">") + 1);
                aret[a] = aret[a].Substring(0, aret[a].IndexOf("</rdf:li>"));
            }
            return aret;
        }
        public string Make(string sName, string sDesc, string[] sOrderedTags)
        {
            //Creates and returns XMP data, given name,
            //description, and a set of ordered tags.
            StringBuilder ret = new StringBuilder(1024);
            ret.AppendLine("<?xpacket begin='ï»¿' id='W5M0MpCehiHzreSzNTczkc9d'?>");
            ret.AppendLine("<x:xmpmeta xmlns:x='adobe:ns:meta/' x:xmptk='PicSys 0.0.1'>");
            ret.AppendLine("<rdf:RDF xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#'>");
            ret.AppendLine(" <rdf:Description rdf:about=''");
            ret.AppendLine("  xmlns:dc='http://purl.org/dc/elements/1.1/'>");
            if (!string.IsNullOrEmpty(sName))
            {
                ret.AppendLine("  <dc:title>");
                ret.AppendLine("   <rdf:Alt>");
                ret.AppendLine("    <rdf:li xml:lang='x-default'>" + 
                    toUTF8(sName) + "</rdf:li>");
                ret.AppendLine("   </rdf:Alt>");
                ret.AppendLine("  </dc:title>");
            }
            if (!string.IsNullOrEmpty(sDesc))
            {
                ret.AppendLine("  <dc:description>");
                ret.AppendLine("   <rdf:Alt>");
                ret.AppendLine("    <rdf:li xml:lang='x-default'>" +
                    toUTF8(sDesc) + "</rdf:li>");
                ret.AppendLine("   </rdf:Alt>");
                ret.AppendLine("  </dc:description>");
            }
            bool bHasTags = false;
            string[][] sTags = new string[sOrderedTags.Length][];
            for (int a = 0; a < sTags.Length; a++)
            {
                sTags[a] = SplitRem(sOrderedTags[a], ",");
                if (sTags[a].Length > 0) bHasTags = true;
            }
            if (bHasTags)
            {
                ret.AppendLine("  <dc:subject>");
                ret.AppendLine("   <rdf:Bag>");
                for (int a = 0; a < sTags.Length; a++)
                {
                    for (int b = 0; b < sTags[a].Length; b++)
                    {
                        ret.AppendLine(
                            "    <rdf:li>" + sTagOrder[a] + "!" +
                            toUTF8(sTags[a][b]) + "</rdf:li>");
                    }
                }
                ret.AppendLine("   </rdf:Bag>");
                ret.AppendLine("  </dc:subject>");
            }
            ret.AppendLine(" </rdf:Description>");
            ret.AppendLine("</rdf:RDF>");
            ret.AppendLine("</x:xmpmeta>");
            ret.Append("<?xpacket end='w'?>");
            //ret.AppendLine("");
            return ret.ToString();
        }
        public void WritePNG(string sFName, string sFOut, string XMP)
        {
            //Writes XMP data to a PNG format image.
            string pngHead = "iTXtXML:com.adobe.xmp";
            //chunkHead + 5xNULL + xmpData
            int iXmpOfs = pngHead.Length + 5;
            byte[] iTXt = new byte[iXmpOfs + XMP.Length];
            toRAW(pngHead).CopyTo(iTXt, 0);
            toRAW(XMP).CopyTo(iTXt, iXmpOfs);

            byte[] iTXtH = GetCrc(iTXt);
            byte[] eof = new byte[] {
                0x00, 0x00, 0x00, 0x00, 0x49, 0x45,
                0x4e, 0x44, 0xae, 0x42, 0x60, 0x82 };
            //The length counts only the data field, 
            //not itself, the chunk type, or the CRC.
            byte[] iTXtL = System.BitConverter.
                GetBytes(iTXt.Length - 4);
            Array.Reverse(iTXtL); //length

            FileStream fs = new FileStream(sFName,
                FileMode.Open, FileAccess.Read);
            byte[] bSrc = new byte[fs.Length - eof.Length];
            fs.Read(bSrc, 0, bSrc.Length);
            fs.Close(); fs.Dispose();

            int rangebegin = 0;
            int[][] oldranges = FindAllPNG(bSrc);
            MemoryStream ms = new MemoryStream();
            for (int a = 0; a < oldranges.Length; a++)
            {
                ms.Write(bSrc, rangebegin,
                    oldranges[a][0] - rangebegin);
                rangebegin = oldranges[a][0] +
                    oldranges[a][1] + 1;
            }

            if (bSrc.Length - rangebegin > 0)
                ms.Write(bSrc, rangebegin,
                    bSrc.Length - rangebegin);  //image
            //ms.Write(bSrc, 0, bSrc.Length);     //image
            ms.Write(iTXtL, 0, iTXtL.Length);   //len
            ms.Write(iTXt, 0, iTXt.Length);     //tags
            ms.Write(iTXtH, 0, iTXtH.Length);   //hash
            ms.Write(eof, 0, eof.Length);       //eof
            
            FileStream fso = new FileStream(sFOut,
                FileMode.Create, FileAccess.Write);
            ms.WriteTo(fso); ms.Close(); ms.Dispose();
            fso.Flush(); fso.Close(); fso.Dispose();
        }

        private string[] SplitRem(string s, string prm)
        {
            return s.Split(new string[] { prm },
                StringSplitOptions.RemoveEmptyEntries);
        }
        private bool bCmpBytes(byte[] b1, int iOfs, byte[] b2)
        {
            if (b1.Length < b2.Length + iOfs) return false;
            for (int a = 0; a < b2.Length; a++)
                if (b1[a + iOfs] != b2[a]) return false;
            return true;
        }
        private string toUTF8(string s)
        {
            byte[] ba = System.Text.Encoding.UTF8.GetBytes(s);
            char[] ca = new char[ba.Length];
            for (int a = 0; a < ba.Length; a++)
                ca[a] = (char)ba[a];
            return new string(ca);
        }
        private string unUTF8(string s)
        {
            byte[] ba = new byte[s.Length];
            for (int a = 0; a < ba.Length; a++)
                ba[a] = (byte)s[a];
            return System.Text.Encoding.UTF8.GetString(ba);
        }
        private byte[] toRAW(string s)
        {
            byte[] b = new byte[s.Length];
            for (int a = 0; a < b.Length; a++)
                b[a] = (byte)s[a];
            return b;
        }
        private string unRAW(byte[] b, int ofs, int len)
        {
            char[] ca = new char[len];
            for (int a = 0; a < len; a++)
                ca[a] = (char)b[a + ofs];
            return new string(ca);
        }

        static uint[] CrcTable = new uint[256];
        static bool IsTableCreated = false;
        private static void CreateCrcTable()
        {
            uint c; int k, n;
            for (n = 0; n < 256; n++)
            {
                c = (uint)n;
                for (k = 0; k < 8; k++)
                    if ((c & 1) == 1)
                        c = 0xedb88320 ^ (c >> 1);
                    else c = c >> 1;
                CrcTable[n] = c;
            }
            IsTableCreated = true;
        }
        public static byte[] GetCrc(byte[] buffer)
        {
            int n;
            uint data = 0xFFFFFFFF;
            if (!IsTableCreated) CreateCrcTable();
            for (n = 0; n < buffer.Length; n++)
                data = CrcTable[(data ^ buffer[n]) & 0xff] ^ (data >> 8);

            data = data ^ 0xFFFFFFFF;
            byte b1 = Convert.ToByte(data >> 24);
            byte b2 = Convert.ToByte(b1 << 8 ^ data >> 16);
            byte b3 = Convert.ToByte(((data >> 16 << 16) ^ (data >> 8 << 8)) >> 8);
            byte b4 = Convert.ToByte((data >> 8 << 8) ^ data);
            return new byte[] { b1, b2, b3, b4 };
        }
    }
}
