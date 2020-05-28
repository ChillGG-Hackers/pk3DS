﻿using pk3DS.Core;
using System;
using System.Linq;
using System.Windows.Forms;
using pk3DS.Core.Structures;

namespace pk3DS
{
public partial class MoveEditor6 : Form
    {
        public MoveEditor6(byte[][] infiles)
        {
            files = infiles;
            movelist[0] = "";

            InitializeComponent();
            Setup();
            RandSettings.GetFormSettings(this, groupBox1.Controls);
        }

        private readonly byte[][] files;
        private readonly string[] types = Main.Config.getText(TextName.Types);
        private readonly string[] moveflavor = Main.Config.getText(TextName.MoveFlavor);
        private string[] movelist = Main.Config.getText(TextName.MoveNames);
        private readonly string[] MoveCategories = { "Status", "Physical", "Special", };
        private readonly string[] StatCategories = { "None", "Attack", "Defense", "Special Attack", "Special Defense", "Speed", "Accuracy", "Evasion", "All", };

        private readonly string[] TargetingTypes =
        { "Single Adjacent Ally/Foe",
            "Any Ally", "Any Adjacent Ally", "Single Adjacent Foe", "Everyone but User", "All Foes",
            "All Allies", "Self", "All Pokémon on Field", "Single Adjacent Foe (2)", "Entire Field",
            "Opponent's Field", "User's Field", "Self",
        };

        private readonly string[] InflictionTypes =
        { "None",
            "Paralyze", "Sleep", "Freeze", "Burn", "Poison",
            "Confusion", "Attract", "Capture", "Nightmare", "Curse",
            "Taunt", "Torment", "Disable", "Yawn", "Heal Block",
            "?", "Detect", "Leech Seed", "Embargo", "Perish Song",
            "Ingrain",
        };

        private readonly string[] MoveQualities =
        { "Only DMG",
            "No DMG -> Inflict Status", "No DMG -> -Target/+User Stat", "No DMG | Heal User", "DMG | Inflict Status", "No DMG | STATUS | +Target Stat",
            "DMG | -Target Stat", "DMG | +User Stat", "DMG | Absorbs DMG", "One-Hit KO", "Affects Whole Field",
            "Affect One Side of the Field", "Forces Target to Switch", "Unique Effect",  };

        private void Setup()
        {
            CB_Move.Items.Clear();
            CB_Type.Items.Clear();
            CB_Category.Items.Clear();
            CB_Stat1.Items.Clear();
            CB_Stat2.Items.Clear();
            CB_Stat3.Items.Clear();
            CB_Targeting.Items.Clear();
            CB_Quality.Items.Clear();
            CB_Inflict.Items.Clear();
            CLB_Flags.Items.Clear();
            foreach (string s in movelist) CB_Move.Items.Add(s);
            foreach (string s in types) CB_Type.Items.Add(s);
            foreach (string s in MoveCategories) CB_Category.Items.Add(s);
            foreach (string s in StatCategories) CB_Stat1.Items.Add(s);
            foreach (string s in StatCategories) CB_Stat2.Items.Add(s);
            foreach (string s in StatCategories) CB_Stat3.Items.Add(s);
            foreach (string s in TargetingTypes) CB_Targeting.Items.Add(s);
            foreach (string s in MoveQualities) CB_Quality.Items.Add(s);
            foreach (string s in InflictionTypes) CB_Inflict.Items.Add(s);
            foreach (var s in Enum.GetNames(typeof(MoveFlag6)).Skip(1)) CLB_Flags.Items.Add(s);
            CB_Inflict.Items.Add("Special");

            CB_Move.Items.RemoveAt(0);
            CB_Move.SelectedIndex = 0;
        }

        private int entry = -1;

        private void ChangeEntry(object sender, EventArgs e)
        {
            SetEntry();
            entry = Array.IndexOf(movelist, CB_Move.Text);
            GetEntry();
        }

        private void GetEntry()
        {
            if (entry < 1) return;
            byte[] data = files[entry];
            {
                RTB.Text = moveflavor[entry].Replace("\\n", Environment.NewLine);

                CB_Type.SelectedIndex = data[0x00];
                CB_Quality.SelectedIndex = (data[0x01] == (byte)255) ? (byte)1 : data[0x01];
                CB_Category.SelectedIndex = (data[0x02] == (byte)255) ? (byte)1 : data[0x02];
                NUD_Power.Value = data[0x3];
                NUD_Accuracy.Value = data[0x4];
                NUD_PP.Value = data[0x05];
                NUD_Priority.Value = (sbyte)data[0x06];
                NUD_HitMin.Value = data[0x7] & 0xF;
                NUD_HitMax.Value = data[0x7] >> 4;
                short inflictVal = BitConverter.ToInt16(data, 0x08);
                CB_Inflict.SelectedIndex = inflictVal < 0 ? CB_Inflict.Items.Count - 1 : inflictVal;
                NUD_Inflict.Value = data[0xA];
                NUD_0xB.Value = data[0xB]; // 0xB ~ Something to deal with skipImmunity
                NUD_TurnMin.Value = data[0xC];
                NUD_TurnMax.Value = data[0xD];
                NUD_CritStage.Value = data[0xE];
                NUD_Flinch.Value = data[0xF];
                NUD_Effect.Value = BitConverter.ToUInt16(data, 0x10);
                NUD_Recoil.Value = (sbyte)data[0x12];
                NUD_Heal.Value = data[0x13];

                CB_Targeting.SelectedIndex = data[0x14];
                CB_Stat1.SelectedIndex = data[0x15];
                CB_Stat2.SelectedIndex = data[0x16];
                CB_Stat3.SelectedIndex = data[0x17];
                NUD_Stat1.Value = (sbyte)data[0x18];
                NUD_Stat2.Value = (sbyte)data[0x19];
                NUD_Stat3.Value = (sbyte)data[0x1A];
                NUD_StatP1.Value = data[0x1B];
                NUD_StatP2.Value = data[0x1C];
                NUD_StatP3.Value = data[0x1D];

                var move = new Move6(data);
                var flags = (uint)move.Flags;
                for (int i = 0; i < CLB_Flags.Items.Count; i++)
                    CLB_Flags.SetItemChecked(i, ((flags >> i) & 1) == 1);
            }
        }

        private void SetEntry()
        {
            if (entry < 1) return;
            byte[] data = files[entry];
            {
                data[0x00] = (byte)CB_Type.SelectedIndex;
                data[0x01] = (byte)CB_Quality.SelectedIndex;
                data[0x02] = (byte)CB_Category.SelectedIndex;
                data[0x03] = (byte)NUD_Power.Value;
                data[0x04] = (byte)NUD_Accuracy.Value;
                data[0x05] = (byte)NUD_PP.Value;
                data[0x06] = (byte)(int)NUD_Priority.Value;
                data[0x07] = (byte)((byte)NUD_HitMin.Value | ((byte)NUD_HitMax.Value << 4));
                int inflictval = CB_Inflict.SelectedIndex; if (inflictval == CB_Inflict.Items.Count) inflictval = -1;
                Array.Copy(BitConverter.GetBytes((short)inflictval), 0, data, 0x08, 2);
                data[0x0A] = (byte)NUD_Inflict.Value;
                data[0x0B] = (byte)NUD_0xB.Value;
                data[0x0C] = (byte)NUD_TurnMin.Value;
                data[0x0D] = (byte)NUD_TurnMax.Value;
                data[0x0E] = (byte)NUD_CritStage.Value;
                data[0x0F] = (byte)NUD_Flinch.Value;
                Array.Copy(BitConverter.GetBytes((ushort)NUD_Effect.Value), 0, data, 0x10, 2);
                data[0x12] = (byte)(int)NUD_Recoil.Value;
                data[0x13] = (byte)NUD_Heal.Value;
                data[0x14] = (byte)CB_Targeting.SelectedIndex;
                data[0x15] = (byte)CB_Stat1.SelectedIndex;
                data[0x16] = (byte)CB_Stat2.SelectedIndex;
                data[0x17] = (byte)CB_Stat3.SelectedIndex;
                data[0x18] = (byte)(int)NUD_Stat1.Value;
                data[0x19] = (byte)(int)NUD_Stat2.Value;
                data[0x1A] = (byte)(int)NUD_Stat3.Value;
                data[0x1B] = (byte)NUD_StatP1.Value;
                data[0x1C] = (byte)NUD_StatP2.Value;
                data[0x1D] = (byte)NUD_StatP3.Value;

                uint flagval = 0;
                for (int i = 0; i < CLB_Flags.Items.Count; i++)
                    flagval |= CLB_Flags.GetItemChecked(i) ? 1u << i : 0;
                BitConverter.GetBytes(flagval).CopyTo(data, 0x1E);                
            }
            files[entry] = data;
            Main.Config.SetText(TextName.MoveNames, movelist);
        }

        private void B_Table_Click(object sender, EventArgs e)
        {
            var items = files.Select(z => new Move6(z));
            Clipboard.SetText(TableUtil.GetTable(items, movelist));
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            SetEntry();
            RandSettings.SetFormSettings(this, groupBox1.Controls);
        }

        private void B_RandAll_Click(object sender, EventArgs e)
        {
            Random rnd = Util.rand;
            if (!CHK_Category.Checked && !CHK_Type.Checked)
            {
                if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Will only randomize Move stats.", "Double check options on the right before continuing.") != DialogResult.Yes) return; 
                for (int i = 0; i < CB_Move.Items.Count; i++)
                {
                    CB_Move.SelectedIndex = i;
                    if (i == 165 || i == 174) continue; // Don't change Struggle or Curse
                    else {
                        if (NUD_Power.Value > 10) {
                            if (rnd.Next(3) != 2)
                            {
                                // Regular chance
                                NUD_Power.Value = rnd.Next(11) * 5 + 50; // 50 ... 100                        
                                NUD_PP.Value = rnd.Next(3) * 5 + 15; // PP: 15-25
                            }
                            else
                            {
                                // Extreme chance
                                NUD_Power.Value = rnd.Next(27) * 5 + 20; // 20 ... 150
                                NUD_PP.Value = rnd.Next(8) * 5 + 5; // PP: 5-40
                            }
                            // Tiny chance for massive power jumps
                            for (int j = 0; j < 2; j++)
                            {
                                if (rnd.Next(100) == 0)
                                {
                                    NUD_Power.Value += 50;
                                }
                            }
                        }
                        if (NUD_Accuracy.Value >= 5)
                        {
                            // "Sane" accuracy randomization
                            // Broken into three tiers based on original accuracy
                            // Designed to limit the chances of 100% accurate OHKO moves and
                            // keep a decent base of 100% accurate regular moves.
                            if (NUD_Accuracy.Value <= 50)
                            {
                                // lowest tier (acc <= 50)
                                // new accuracy = rand(20...50) inclusive
                                // with a 10% chance to increase by 50%
                                NUD_Accuracy.Value = rnd.Next(7) * 5 + 20;
                                if (rnd.Next(10) == 0)
                                {
                                    NUD_Accuracy.Value = (NUD_Accuracy.Value * 3 / 2) / 5 * 5;
                                }
                            }
                            else if (NUD_Accuracy.Value < 90)
                            {
                                // middle tier (50 < acc < 90)
                                // count down from 100% to 20% in 5% increments with 20%
                                // chance to "stop" and use the current accuracy at each
                                // increment
                                // gives decent-but-not-100% accuracy most of the time
                                NUD_Accuracy.Value = 100;
                                while (NUD_Accuracy.Value > 20)
                                {
                                    if (rnd.Next(10) < 2)
                                        break;
                                    NUD_Accuracy.Value -= 5;
                                }
                            }
                            else
                            {
                                // highest tier (90 <= acc <= 100)
                                // count down from 100% to 20% in 5% increments with 40%
                                // chance to "stop" and use the current accuracy at each
                                // increment
                                // gives high accuracy most of the time
                                NUD_Accuracy.Value = 100;
                                while (NUD_Accuracy.Value > 20)
                                {
                                    if (rnd.Next(10) < 4)
                                        break;
                                    NUD_Accuracy.Value -= 5;
                                }
                            }
                        }
                        if (NUD_HitMax.Value > 1)
                        {
                            // Divide randomized power by average hit count, round to
                            // nearest 5
                            NUD_Power.Value = (int)(NUD_Power.Value / NUD_HitMax.Value / 3) * 5;
                            if (NUD_Power.Value == 0)
                            {
                                NUD_Power.Value = 5;
                            }
                        }
                        NUD_PP.Value = 40;
                    }
                }
                CB_Move.SelectedIndex = 0;
                WinFormsUtil.Alert("All Moves have had their Base values modified!");
            }

            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Randomize Moves? Cannot undo.", "Double check options on the right before continuing.") != DialogResult.Yes) return;
            for (int i = 0; i < CB_Move.Items.Count; i++)
            {
                CB_Move.SelectedIndex = i; // Get new Move
                if (i == 165 || i == 174) continue; // Don't change Struggle or Curse

                // Change Damage Category if Not Status
                if (CB_Category.SelectedIndex > 0 && CHK_Category.Checked) // Not Status
                    CB_Category.SelectedIndex = rnd.Next(1, 3);

                // Change Move Type
                if (CHK_Type.Checked)
                {
                    int newType = rnd.Next(0, 17); // Randomize new type
                    string newTypeSTR = types[newType]; // Text for new type
                    string oldTypeSTR = types[CB_Type.SelectedIndex]; // Text for old type
                    int oldIndex = 0;
                    //Console.WriteLine(types.Any(CB_Move.Text.Contains));
                    //if (CB_Move.Text.Contains(oldTypeSTR))
                    //Console.WriteLine(Array.IndexOf(movelist, CB_Move.Text));
                    oldIndex = Array.IndexOf(movelist, CB_Move.Text); // Where in the list of moves is it?
                    bool finished = false;
                    foreach (string type in types)
                    {
                        if (CB_Move.Text.Contains(type) & type != newTypeSTR & !finished)                            {
                                
                            Console.WriteLine("Was:");                                    
                            Console.WriteLine(movelist[oldIndex]); // Print the old name
                            /* if (newTypeSTR == "Electric")
                                newTypeSTR = "Elec.";
                            if (newTypeSTR == "Fighting")
                                newTypeSTR = "Fight.";    
                            if (newTypeSTR == "Psychic")
                                newTypeSTR = "Psy."; */                          
                            movelist[oldIndex] = CB_Move.Text.Replace(type, newTypeSTR); // Replace old type string with new one
                            Console.WriteLine("Now:");
                            Console.WriteLine(movelist[oldIndex]); // print the new name
                            if (type == oldTypeSTR)
                                finished = true;
                        }                        
                        
                    }
                    CB_Type.SelectedIndex = newType;
                }
            }
            WinFormsUtil.Alert("All Moves have been randomized!");
            Setup();
        }

        private void B_Metronome_Click(object sender, EventArgs e)
        {
            if (WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Play using Metronome Mode?", "This will set the Base PP for every other Move to 0!") != DialogResult.Yes) return;

            for (int i = 0; i < CB_Move.Items.Count; i++)
            {
                CB_Move.SelectedIndex = i;
                if (CB_Move.SelectedIndex != 117 || CB_Move.SelectedIndex != 32)
                    NUD_PP.Value = 0;
                if (CB_Move.SelectedIndex == 117)
                    NUD_PP.Value = 40;
                if (CB_Move.SelectedIndex == 32)
                    NUD_PP.Value = 1;
            }
            CB_Move.SelectedIndex = 0;
            WinFormsUtil.Alert("All Moves have had their Base PP values modified!");
        }
    }
}


