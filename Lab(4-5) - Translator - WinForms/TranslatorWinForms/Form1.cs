﻿using ListViewSortAnyColumn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TranslatorWinForms
{
    public partial class Form1 : Form
    {
        static public List<string[]> words = new List<string[]>();
        static public Dictionary<string, string> Dict;
        List<string> fonts = new List<string>();
        bool isBold = false, isUnderlined = false, isItalic = false;
        FontStyle style = new FontStyle();



    public Form1()
        {

            InitializeComponent();
            foreach (FontFamily oneFontFamily in FontFamily.Families)
            {

                toolStripComboBox1.Items.Add(oneFontFamily.Name);
            }

            toolStripComboBox1.Text = this.richTextBox1.Font.Name.ToString();
            

            richTextBox1.Focus();
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "TXT files|*.txt";
            string CombinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..//..//");
            theDialog.InitialDirectory = System.IO.Path.GetFullPath(CombinedPath);
            listView1.Items.Clear();
            words.Clear();
            StringComparer comparer = StringComparer.CurrentCultureIgnoreCase;
            Dict = new Dictionary<string, string>(comparer);

            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string line in File.ReadAllLines(theDialog.FileName.ToString()))
                {
                    string[] word = line.Split(' ');
                    if (word.Length != 2)
                        continue;
                    if (word[0].All(c => Char.IsLetter(c)) && word[1].All(c => Char.IsLetter(c)))
                        words.Add(word);

                }
                listView1.Columns[0].Text = words[0][0];
                listView1.Columns[1].Text = words[0][1];


                for (int i = 1; i < words.Count; i++)
                {
                    if (!Dict.ContainsKey(words[i][0]))
                    {
                        Dict.Add(words[i][0], words[i][1]);
                        listView1.Items.Add(
                            new ListViewItem(new[]
                            {
                            words[i][0],
                            words[i][1]
                            }));
                    }
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog1 = new SaveFileDialog();
            SaveFileDialog theDialog = new SaveFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "TXT files|*.txt";
            string CombinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..//..//");
            theDialog.InitialDirectory = System.IO.Path.GetFullPath(CombinedPath);
            if (theDialog.ShowDialog() == DialogResult.OK)

                for (int i = 0; i < words.Count; i++)
                {
                    File.AppendAllText(theDialog.FileName.ToString(), words[i][0] + " " + words[i][1] + Environment.NewLine);
                }

        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Translate_click(object sender, EventArgs e)
        {
            if (words.Count == 0)
            {
                MessageBox.Show("No dictionary added", "Error");
                return;
            }
            richTextBox2.Clear();
            List<string[]> Lines = new List<string[]>();
            if (richTextBox1.Text.Contains('\n'))
            {
                string[] SplitLines = richTextBox1.Text.Split('\n');
                foreach (string a in SplitLines)
                {
                    string[] LineOfWords = a.Split(' ');
                    LineOfWords[LineOfWords.Length - 1] += " \n";
                    Lines.Add(LineOfWords);
                }
            }
            else
            {
                Lines.Add(richTextBox1.Text.Split(' '));
            }

            foreach (string[] LineOfWords in Lines)
            {
                foreach (string a in LineOfWords)
                {
                    if (Dict.ContainsKey(a))
                    {
                        richTextBox2.SelectionColor = Color.FromArgb(0, 0, 0);
                        richTextBox2.AppendText(Dict[a] + " ");
                    }
                    else if (Dict.ContainsValue(a))
                    {
                        richTextBox2.SelectionColor = Color.FromArgb(0, 0, 0);
                        richTextBox2.AppendText(Dict.FirstOrDefault(x => x.Value == a).Key + " ");
                    }
                    else if (a == "420")
                        richTextBox2.AppendText("kod Spalińskiego\n");
                    else
                    {
                        richTextBox2.SelectionColor = Color.FromArgb(255, 0, 0);
                        richTextBox2.AppendText(a + " ");
                    }
                }
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var toDelete = listView1.SelectedItems;
                foreach (ListViewItem item in toDelete)
                {
                    string toDelKey = item.SubItems[0].Text;
                    //remove word pair from dictionary
                    Dict.Remove(toDelKey);
                    //remove it from the list
                    item.Remove();
                }

            }
        }

        // ColumnClick event handler.
        private void ColumnClick(object o, ColumnClickEventArgs e)
        {
            ItemComparer sorter = listView1.ListViewItemSorter as ItemComparer;
            if (sorter == null)
            {
                sorter = new ItemComparer(e.Column);
                sorter.Order = SortOrder.Ascending;
                listView1.ListViewItemSorter = sorter;
            }
            // if clicked column is already the column that is being sorted
            if (e.Column == sorter.Column)
            {
                // Reverse the current sort direction
                if (sorter.Order == SortOrder.Ascending)
                    sorter.Order = SortOrder.Descending;
                else
                    sorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.Column = e.Column;
                sorter.Order = SortOrder.Ascending;
            }
            listView1.Sort();
        }

        private void AddWord_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            AddWord _AddWord = new AddWord(this);
            if(words.Count == 0)
            { 
                _AddWord.label1.Text = "English";
                _AddWord.label2.Text = "Polish";
            }
            else
            {
                _AddWord.label1.Text = listView1.Columns[0].Text;
                _AddWord.label2.Text = listView1.Columns[1].Text;
            }
            dr = _AddWord.ShowDialog();
            if (dr == DialogResult.OK)
            {
                for (int i = 1; i < words.Count; i++)
                {
                    if (!Dict.ContainsKey(words[i][0]))
                    {
                        Dict.Add(words[i][0], words[i][1]);
                        listView1.Items.Add(
                            new ListViewItem(new[]
                            {
                                Form1.words[i][0],
                                Form1.words[i][1]
                            }));
                    }
                }
            }
            else
            {
                //MessageBox.Show("nothing added");
            }
        }

        private void toolStripComboBox1_Selected(object sender, EventArgs e)
        {
            richTextBox1.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Regular);
            richTextBox2.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Regular);
        }

        private void toolStrip_ForeColor(object sender, EventArgs e)
        {
            colorDialog1.Color = richTextBox1.ForeColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                richTextBox1.ForeColor = colorDialog1.Color;
        }

        private void toolStripItalic_Click(object sender, EventArgs e)
        {
            if (!isItalic)
            {
                isItalic = true;
                //combine styles
                richTextBox1.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Italic);
                richTextBox2.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Italic);
            }
            else
            {
                isItalic = false;
                richTextBox1.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Regular);
                richTextBox2.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Regular);
            }
        }

        private void toolStripUnderline_Click(object sender, EventArgs e)
        {
            if (!isUnderlined)
            {
                isUnderlined = true;
                richTextBox1.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Underline);
                richTextBox2.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Underline);
            }
            else
            {
                isUnderlined = false;
                richTextBox1.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Regular);
                richTextBox2.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Regular);
            }
        }

        private void toolStrip_UnderlineColor(object sender, EventArgs e)
        {
            //underline color
            colorDialog1.Color = richTextBox1.ForeColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK) ;
                // = colorDialog1.Color;
        }

        private void toolStripBold_Click(object sender, EventArgs e)
        {
            if (!isBold)
            {
                isBold = true;
                richTextBox1.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Bold);
                richTextBox2.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Bold);
            }
            else
            {
                isBold = false;
                richTextBox1.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Regular);
                richTextBox2.Font = new Font(toolStripComboBox1.SelectedItem.ToString(), 12, FontStyle.Regular);
            }
        }
    }
}

