using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using System.IO;

 

//Simple HTML Text Editor Application
//Version 1.0.0
namespace TextEditor
{

    public partial class Form1 : Form
    {

        //Global Variables:

        //Start of Global Variables

        //This is the index
        int run;
        //Small dictionary for html (html keywords)
        const int htmlDic_size = 9;
        string[] htmlDic = new string[htmlDic_size] {"<body>","<html>","<h1>", "<h2>", "<h3>", "<h4>", "<h5>", "<h6>","<p>" };

        //Arrays that store current and new 
        //keyword occurances
        int[] htmlDic_occur = new int[htmlDic_size];
        int[] htmlDic_newOccur = new int[htmlDic_size];

        //Constants used when window is scaled (Is currently buggy)
        const double txBoxRatio = 0.497792494481236;
        const double webBrowserRatio = 0.429359823399558;

        //End Of Global Variables

        //Basically a Main() function
        //Here I initialize the window and its components
        //With their default values
        public Form1()
        {
            InitializeComponent();

            //Modifying the richTextBox2 (text box that displays number lines)
            richTextBox2.Cursor = Cursors.PanNE;
            richTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            richTextBox2.Font = new Font(richTextBox1.Font.FontFamily,
                          richTextBox1.Font.Size);

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        //Function is called each time there is a key pressed while window is in focus of 
        //richTextBox1 (text box that the user types his code in
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            run = 0;
            string Content = richTextBox1.Text;
            int caretPos = richTextBox1.SelectionStart;
            //Updates line count on the other rich text document
            updateNumberLabel();



            while (run < (htmlDic_size))
            {
                Style(richTextBox1, Content, htmlDic[run], run, caretPos);
                run++;
            }


        }
        //Drop down save button
        //Calls Save() & Refreshes the page
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Modify this sometime!
            Save();
            webBrowser1.Refresh();

        }

        //Saves the file the user is editing
        public void Save()
        {
            TextWriter writer = new StreamWriter(@"C:\Users\mineg\Desktop\Try My Best\index.html");
            writer.Write(richTextBox1.Text);
            writer.Close();

        }

        //Sets the color of tags and reverts back to the default text color
        public void Style(RichTextBox richTextBox1,string Content,string keyWords,int run, int caretPos)
        {
            //These two variables are responsible for holding
            //Starting and ending caret positions
            int start;
            int end;

            //Before styling the words, check that it is worth doing so
            //Style NEW words, don't waste resources styling OLD words
            htmlDic_newOccur[run] = CheckOccurrences(Content, keyWords);

            if (Content.Contains(keyWords) && htmlDic_newOccur[run] > htmlDic_occur[run])
            {

                //First selection: any <generic tag>
                htmlDic_occur[run]++;
                start = caretPos - keyWords.Length;
                end = caretPos;
                richTextBox1.Select(start, end);
                richTextBox1.SelectionColor = Color.FromArgb(238, 9, 90);

                //Insert closing tag </generic tag>
                Insert(richTextBox1, keyWords, caretPos);

                //Restore Original Text Color in between
                richTextBox1.SelectionStart = caretPos;
                richTextBox1.SelectionColor = Color.FromArgb(213, 208, 208);

                //Current Color Palette:
                // Red text Color: Color.FromArgb(238, 9, 90);
                // Original text Color: Color.FromArgb(213, 208, 208);

            }

        }

        //Simple funtion that checks the occurance of a word in a string
        public static int CheckOccurrences(string str1, string pattern)
        {
            int count = 0;
            int a = 0;

            while ((a = str1.IndexOf(pattern, a)) != -1)
            {
                a += pattern.Length;
                count++;
            }
            return count;
        }


        //This function simply inserts an ending tag inmediately after the user correctly
        //types the initial tag
        //NOTE: Work On this function later
        public void Insert(RichTextBox rtb, string insert, int caretPos) {
            //My method makes use of the Clipboard thus:
            string tmpClip = Clipboard.GetText(); //I must retrieve anything the user currently has in his clipboard
            Clipboard.SetText("</" + insert.Substring(1)); //Overwrite what is in the clipboard
            richTextBox1.SelectionStart = caretPos;
            richTextBox1.SelectionLength = 0;
            richTextBox1.Paste(); //Paste

            Clipboard.SetText(tmpClip); //Revert original clipboard text
        }


        //Modify Later
        //Courtesy of: https://www.codeproject.com/Articles/12152/Numbering-lines-of-RichTextBox-in-NET
        //Start of "Code Project"

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            updateNumberLabel();
        }


        //Updates the richtextbox2 to display the line number
        private void updateNumberLabel()
        {
            //we get index of first visible char and 
            //number of first visible line
            Point pos = new Point(0, 0);
            int firstIndex = richTextBox1.GetCharIndexFromPosition(pos);
            int firstLine = richTextBox1.GetLineFromCharIndex(firstIndex)+1;

            //now we get index of last visible char 
            //and number of last visible line
            pos.X = richTextBox1.Width;
            pos.Y = richTextBox1.Height+menuStrip1.Height;
            int lastIndex = richTextBox1.GetCharIndexFromPosition(pos)+1;
            int lastLine = richTextBox1.GetLineFromCharIndex(lastIndex)+1;

            //finally, renumber label
            richTextBox2.Text = "";
            for (int i = firstLine; i <= lastLine; i++)
            {
                richTextBox2.Text += i + "\n";
            }

        }
        //End of "Code Project" (stuff that is not my code)

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Function is called each time the window is resized
        //This function is responsible for 
        private void Form1_Resize(object sender, EventArgs e)
        {
            richTextBox2.Height = ClientSize.Height;
            richTextBox1.Height = ClientSize.Height;

            richTextBox1.Width = (int)((txBoxRatio) * ClientSize.Width);
            webBrowser1.Width = (int)(webBrowserRatio * ClientSize.Width);

            //Since the view changes, you need to update the 
            //current line counter
            updateNumberLabel();
        }
    }
}
