﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using pk3DS.Core.CTR;

namespace pk3DS.Subforms
{
    public partial class MapPermView : Form
    {
        public MapPermView()
        {
            InitializeComponent();
            MapMatrixes = Directory.GetFiles("mapMatrix");
            MapGRs = Directory.GetFiles("mapGR");
        }

        private readonly string[] MapMatrixes;
        private readonly string[] MapGRs;
        private int mapScale = -1;
        public int DrawMap = -1;

        public void SetMap(int Map)
        {
            DrawMap = Map;
            PB_Map.Image = CHK_AutoDraw.Checked ? GetMapImage(sliceArea: true) : null;
        }

        public Bitmap GetMapImage(bool crop = false, bool entity = true, bool sliceArea = false)
        {
            // Load MM
            byte[][] MM = Mini.UnpackMini(File.ReadAllBytes(MapMatrixes[DrawMap]), "MM");
            var mm = OWSE.mm = new MapMatrix(MM);

            // Unknown
            if (ModifierKeys == Keys.Control)
                Clipboard.SetText(mm.Unk2String());

            // Load GR TileMaps
            for (int i = 0; i < mm.EntryList.Length; i++)
            {
                if (mm.EntryList[i] == 0xFFFF) // Mystery Zone
                    continue;
                byte[][] GR = Mini.UnpackMini(File.ReadAllBytes(MapGRs[mm.EntryList[i]]), "GR");
                mm.Entries[i] = new MapMatrix.Entry(GR[0]) {coll = new MapMatrix.Collision(GR[2])};
            }
            mapScale = Math.Max(1, (int)NUD_Scale.Value);
            Bitmap img = mm.Preview(mapScale, (int)NUD_Flavor.Value);

            baseImage = (Bitmap)img.Clone();

            if (sliceArea && mapScale > 3)
            {
                int area = 40*mapScale;
                for (int x = 0; x < img.Width; x++)
                {
                    for (int y = 0; y < img.Height; y++)
                    {
                        if (x % area == 0 || y % area == 0)
                            img.SetPixel(x,y,Color.FromArgb(0x10,0xFF,0,0));
                    }
                }
            }

            if (entity && mapScale == 8)
                img = OverlayEntities(img);

            if (crop)
                img = WinFormsUtil.TrimBitmap(img);

            return img;
        }

        internal static Bitmap baseImage;

        private Bitmap OverlayEntities(Bitmap img)
        {
            const float opacity = 0.66f;
            // Overlay every... overworld entity
            foreach (var e in OWSE.CurrentZone.Entities.Furniture)
            {
                int x = e.X;
                int y = e.Y;
                for (int sx = 0; sx < e.WX; sx++) // Stretch X
                {
                    for (int sy = 0; sy < e.WY; sy++) // Stretch Y
                    {
                        try { WinFormsUtil.LayerImage(img, Properties.Resources.F, (x + sx) * mapScale, (y + sy) * mapScale, opacity); }
                        catch { }
                    }
                }
            }
            foreach (var e in OWSE.CurrentZone.Entities.NPCs)
            {
                int x = e.X;
                int y = e.Y;
                try { WinFormsUtil.LayerImage(img, Properties.Resources.N, x * mapScale, y * mapScale, opacity); }
                catch { }
            }
            foreach (var e in OWSE.CurrentZone.Entities.Warps)
            {
                int x = (int)e.PX; // shifted warps look weird
                int y = (int)e.PY; // shifted warps look weird
                for (int sx = 0; sx < e.Width; sx++) // Stretch X
                {
                    for (int sy = 0; sy < e.Height; sy++) // Stretch Y
                    {
                        try { WinFormsUtil.LayerImage(img, Properties.Resources.W, (x + sx) * mapScale, (y + sy) * mapScale, opacity); }
                        catch { }
                    }
                }
            }
            foreach (var e in OWSE.CurrentZone.Entities.Triggers1)
            {
                int x = e.X;
                int y = e.Y;
                for (int sx = 0; sx < e.Width; sx++) // Stretch X
                {
                    for (int sy = 0; sy < e.Height; sy++) // Stretch Y
                    {
                        try { WinFormsUtil.LayerImage(img, Properties.Resources.T1, (x + sx) * mapScale, (y + sy) * mapScale, opacity); }
                        catch { }
                    }
                }
            }
            foreach (var e in OWSE.CurrentZone.Entities.Triggers2)
            {
                int x = e.X;
                int y = e.Y;
                for (int sx = 0; sx < e.Width; sx++) // Stretch X
                {
                    for (int sy = 0; sy < e.Height; sy++) // Stretch Y
                    {
                        try { WinFormsUtil.LayerImage(img, Properties.Resources.T2, (x + sx) * mapScale, (y + sy) * mapScale, opacity); }
                        catch { }
                    }
                }
            }

            // Overlay Map Data
            // Flyto
            {
                int x = (int)OWSE.CurrentZone.ZD.PX2;
                int y = (int)OWSE.CurrentZone.ZD.PY2;
                for (int sx = 0; sx < 1; sx++) // Stretch X
                {
                    for (int sy = 0; sy < 1; sy++) // Stretch Y
                    {
                        try { WinFormsUtil.LayerImage(img, Properties.Resources.FLY, (x + sx) * mapScale, (y + sy) * mapScale, opacity/2); }
                        catch { }
                    }
                }
            }
            // Unknown
            //{
            //    using(var g = Graphics.FromImage(img))
            //        foreach (var l in OWSE.mm.LoadLines)
            //            try { g.DrawLine(new Pen(Color.Red, 4), l.p2 * mapScale, l.p1 * mapScale, l.p4 * mapScale, l.p3 * mapScale); }
            //            catch {}
            //}

            return img;
        }

        // UI
        private void HoverMap(object sender, MouseEventArgs e)
        {
            if (mapScale < 0)
                return;

            if (PB_Map.Image == null)
                return;

            int X = e.X / mapScale;
            int Y = e.Y / mapScale;

            int entryX = X/40;
            int entryY = Y/40;

            int entry = (entryY *(PB_Map.Image.Width/40/mapScale)) + entryX;
            int epX = X%40;
            int epY = Y%40;
            int tile = (epY * 40) + epX;
            try
            {
                var tileVal = OWSE.mm.Entries[entry] == null
                    ? "No Tile"
                    : OWSE.mm.Entries[entry].Tiles[tile].ToString("X8");

                L_MapCoord.Text = string.Format("V:0x{3}{2}X:{0,3}  Y:{1,3}", X, Y, Environment.NewLine, tileVal);
            }
            catch { }
        }

        private void B_Redraw_Click(object sender, EventArgs e)
        {
            if (DrawMap != -1)
                PB_Map.Image = GetMapImage(sliceArea: true);
        }

        private void MapPermView_FormClosing(object sender, FormClosingEventArgs e)
        {
            CHK_AutoDraw.Checked = false;
            Hide();
            e.Cancel = true;
        }

        private void FocusPanel(object sender, EventArgs e)
        {
            if (ContainsFocus)
                PAN_MAP.Focus();
        }

        private void DclickMap(object sender, EventArgs e)
        {
            DialogResult dr = WinFormsUtil.Prompt(MessageBoxButtons.YesNoCancel, "Copy image to Clipboard?",
                "Yes: Map & Overworlds" + Environment.NewLine + "No: Map Only");
            if (dr == DialogResult.No) // Map Only
                Clipboard.SetImage(WinFormsUtil.TrimBitmap(baseImage));
            if (dr == DialogResult.Yes)
                Clipboard.SetImage(PB_Map.Image);
        }
    }
}
