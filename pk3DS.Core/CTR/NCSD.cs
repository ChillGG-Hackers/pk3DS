﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pk3DS.Core.CTR
{
    public class NCSD
    {
        public Header header;
        public CardInfoHeader cardinfoheader;
        public List<NCCH> NCCH_Array;
        public bool Card2;
        public byte[] Data;

        public class Header
        {
            public byte[] Signature; //Size 0x100;
            public uint Magic;
            public uint MediaSize;
            public ulong TitleId;
            //public byte[] padding; //Size: 0x10
            public NCCH_Meta[] OffsetSizeTable; //Size: 8
            //public byte[] padding; //Size: 0x28
            public byte[] flags; //Size: 0x8
            public ulong[] NCCHIdTable; //Size: 0x8;
            //public byte[] Padding2; //Size: 0x30;
        }

        public class CardInfoHeader
        {
            public uint WritableAddress;
            public uint CardInfoBitmask;
            public CardInfoNotes CIN;
            public ulong NCCH0TitleId;
            public ulong Reserved0;
            public byte[] InitialData; // Size: 0x30
            public byte[] Reserved1; // Size: 0xC0
            public byte[] NCCH0Header; // Size: 0x100

            public class CardInfoNotes
            {
                public byte[] Reserved0; // Size: 0xF8;
                public ulong MediaSizeUsed;
                public ulong Reserved1;
                public uint Unknown;
                public byte[] Reserved2; //Size: 0xC;
                public ulong CVerTitleId;
                public ushort CVerTitleVersion;
                public byte[] Reserved3; //Size: 0xCD6;
            }
        }

        public class NCCH_Meta
        {
            public uint Offset;
            public uint Size;
        }

        private const ulong MEDIA_UNIT_SIZE = 0x200;

        public ulong GetWritableAddress()
        {
            return Card2
                ? Align((header.OffsetSizeTable[NCCH_Array.Count - 1].Offset * NCCH.MEDIA_UNIT_SIZE)
                        + (header.OffsetSizeTable[NCCH_Array.Count - 1].Size * NCCH.MEDIA_UNIT_SIZE) + 0x1000, 0x10000) / MEDIA_UNIT_SIZE
                : 0x00000000FFFFFFFF;
        }

        public void BuildHeader()
        {
            Data = new byte[0x4000];
            Array.Copy(header.Signature, Data, 0x100);
            Array.Copy(BitConverter.GetBytes(header.Magic), 0, Data, 0x100, 4);
            Array.Copy(BitConverter.GetBytes(header.MediaSize), 0, Data, 0x104, 4);
            Array.Copy(BitConverter.GetBytes(header.TitleId), 0, Data, 0x108, 8);
            for (int i = 0; i < header.OffsetSizeTable.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(header.OffsetSizeTable[i].Offset), 0, Data, 0x120 + (8 * i), 4);
                Array.Copy(BitConverter.GetBytes(header.OffsetSizeTable[i].Size), 0, Data, 0x124 + (8 * i), 4);
            }
            Array.Copy(header.flags, 0, Data, 0x188, header.flags.Length);
            for (int i = 0; i < header.NCCHIdTable.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(header.NCCHIdTable[i]), 0, Data, 0x190 + (8 * i), 8);
            }
            //CardInfoHeader
            Array.Copy(BitConverter.GetBytes(cardinfoheader.WritableAddress), 0, Data, 0x200, 4);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CardInfoBitmask), 0, Data, 0x204, 4);
            Array.Copy(cardinfoheader.CIN.Reserved0, 0, Data, 0x208, cardinfoheader.CIN.Reserved0.Length);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.MediaSizeUsed), 0, Data, 0x300, 8);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.Reserved1), 0, Data, 0x308, 8);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.Unknown), 0, Data, 0x310, 4);
            Array.Copy(cardinfoheader.CIN.Reserved2, 0, Data, 0x314, cardinfoheader.CIN.Reserved2.Length);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.CVerTitleId), 0, Data, 0x320, 8);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.CIN.CVerTitleVersion), 0, Data, 0x328, 2);
            Array.Copy(cardinfoheader.CIN.Reserved3, 0, Data, 0x32A, cardinfoheader.CIN.Reserved3.Length);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.NCCH0TitleId), 0, Data, 0x1000, 8);
            Array.Copy(BitConverter.GetBytes(cardinfoheader.Reserved0), 0, Data, 0x1008, 8);
            Array.Copy(cardinfoheader.InitialData, 0, Data, 0x1010, cardinfoheader.InitialData.Length);
            Array.Copy(cardinfoheader.Reserved1, 0, Data, 0x1040, cardinfoheader.Reserved1.Length);
            Array.Copy(cardinfoheader.NCCH0Header, 0, Data, 0x1100, cardinfoheader.NCCH0Header.Length);
            Array.Copy(Enumerable.Repeat((byte)0xFF, 0x2E00).ToArray(), 0, Data, 0x1200, 0x2E00);
        }

        public Header CreateHeaderFromBytes(byte[] data)
        {
            Header header = new Header();
            Data = data;
            header.Signature = new byte[0x100];
            Array.Copy(data, header.Signature, 0x100);
            header.Magic = BitConverter.ToUInt32(data, 0x100);
            header.MediaSize = BitConverter.ToUInt32(data, 0x104);
            header.TitleId = BitConverter.ToUInt64(data, 0x108);
            header.OffsetSizeTable = new NCCH_Meta[8];
            for (int i = 0; i < 8; i++)
            {
                header.OffsetSizeTable[i] = new NCCH_Meta();
                header.OffsetSizeTable[i].Offset = BitConverter.ToUInt32(data, 0x120 + (8 * i));
                header.OffsetSizeTable[i].Size = BitConverter.ToUInt32(data, 0x124 + (8 * i));
            }
            header.flags = new byte[8];
            Array.Copy(data, 0x188, header.flags, 0, 8);
            header.NCCHIdTable = new ulong[8];
            for (int i = 0; i < 8; i++)
            {
                header.NCCHIdTable[i] = BitConverter.ToUInt64(data, 0x190 + (8 * i));
            }
            return header;
        }

        public void ExtractFilesFromNCSD(string NCSD_PATH, string outputDirectory, RichTextBox TB_Progress = null, ProgressBar PB_Show = null)
        {
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            updateTB(TB_Progress, "Extracting game data (CXI) from .3DS file...");
            byte[] headerBytes = new byte[0x200];
            using (FileStream fs = new FileStream(NCSD_PATH, FileMode.Open, FileAccess.Read))
            {
                fs.Read(headerBytes, 0, headerBytes.Length);
            }

            header = CreateHeaderFromBytes(headerBytes);
            string ncchPath = ExtractCXIfromNCSD(NCSD_PATH, outputDirectory, header.OffsetSizeTable[0].Size, PB_Show);
            updateTB(TB_Progress, "CXI extracted, extracting files from CXI...");
            NCCH ncch = new NCCH();
            ncch.ExtractNCCHFromFile(ncchPath, outputDirectory, TB_Progress, PB_Show);
            File.Delete(ncchPath);
        }

        private static string ExtractCXIfromNCSD(string NCSD_PATH, string outputDirectory, uint ncchSize, ProgressBar PB_Show = null)
        {
            byte[] buffer = new byte[MEDIA_UNIT_SIZE * 10];
            string outputFile = Path.Combine(outputDirectory, "game.cxi");
            if (PB_Show.InvokeRequired)
                PB_Show.Invoke((MethodInvoker)delegate { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = Convert.ToInt32(ncchSize); });
            else { PB_Show.Minimum = 0; PB_Show.Step = 1; PB_Show.Value = 0; PB_Show.Maximum = Convert.ToInt32(ncchSize); }

            using (FileStream inputFileStream = new FileStream(NCSD_PATH, FileMode.Open, FileAccess.Read),
                              outputFileStream = new FileStream(outputFile, FileMode.Append, FileAccess.Write))
            {
                inputFileStream.Seek(0x4000, SeekOrigin.Begin);
                for (int i = 0; i < ncchSize; i++)
                {
                    inputFileStream.Read(buffer, 0, buffer.Length);
                    outputFileStream.Write(buffer, 0, buffer.Length);
                    if (PB_Show.InvokeRequired)
                        PB_Show.Invoke((MethodInvoker)PB_Show.PerformStep);
                    else { PB_Show.PerformStep(); }

                }
            }
            return outputFile;
        }

        internal static ulong Align(ulong input, ulong alignsize)
        {
            ulong output = input;
            if (output % alignsize != 0)
            {
                output += alignsize - (output % alignsize);
            }
            return output;
        }

        internal static void updateTB(RichTextBox RTB, string progress)
        {
            try
            {
                if (RTB.InvokeRequired)
                    RTB.Invoke((MethodInvoker)delegate
                    {
                        RTB.AppendText(Environment.NewLine + progress);
                        RTB.SelectionStart = RTB.Text.Length;
                        RTB.ScrollToCaret();
                    });
                else
                {
                    RTB.SelectionStart = RTB.Text.Length;
                    RTB.ScrollToCaret();
                    RTB.AppendText(progress + Environment.NewLine);
                }
            }
            catch { }
        }
    }
}
