using System;
using System.Windows.Forms;  //windows forms
using System.Speech.Synthesis;  //for speech synthesis

namespace TTMA
{
    public partial class TTMAMain : Form
    {
        public static string[] Menu = new string[8];  //stores lines from 7 textboxes

        public TTMAMain()
        {
            InitializeComponent();  
        }

        bool GatherText()  //put contents of textboxes in Menu array
        {
            Menu[0] = "Totally Tiffany's Cafe";  //static business name
            Menu[1] = DatesTB.Text; 
            Menu[2] = "Monday - " + MondayTB.Text;
            Menu[3] = "Tuesday - " + TuesdayTB.Text;
            Menu[4] = "Wednesday - " + WednesdayTB.Text;
            Menu[5] = "Thursday - " + ThursdayTB.Text;
            Menu[6] = "Friday Breakfast - " + FBreakfastTB.Text;
            Menu[7] = "Friday Lunch - " + FLunchTB.Text;

            //add up total length of all textboxes
            int TotalTextLength = DatesTB.Text.Length;
            TotalTextLength += MondayTB.Text.Length;
            TotalTextLength += TuesdayTB.Text.Length;
            TotalTextLength += WednesdayTB.Text.Length;
            TotalTextLength += ThursdayTB.Text.Length;
            TotalTextLength += FBreakfastTB.Text.Length;
            TotalTextLength += FLunchTB.Text.Length;

            if (TotalTextLength > 0)  //if all boxes not empty 
                return true;  //return true
            else
                return false; //return false

        }

        private void ReadBTN_Click(object sender, EventArgs e)  //Read button speaks entire menu
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();  //adds speech ability to this function
            //if no text in textboxes, don't speak empty strings
            if ( GatherText() == false )   //reset menu array with current textbox contents
            {
                synth.Speak("All menu sections are empty, nothing to speak.");  //alert user
                return;  //exit function
            }

            for (int i = 0; i < Menu.Length; i++)  //loop through elements
            {
                synth.Speak(Menu[i]);  //speak contents of element
            }
        }

        private void HelpBTN_Click(object sender, EventArgs e)  //reads help text instead of dialog box
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();  //make the computer chatty

            synth.Speak("Use the following keyboard keys for Totally Tiffany's Menu Assistant:");
            synth.Speak("Type in what you are serving for each day of the week.");
            synth.Speak("Up Arrow - Reads the current line.");
            synth.Speak("TAB - switches between sections.");
            synth.Speak("Read Button - Speaks the entire menu out loud.");
            synth.Speak("Print Button - Saves everything to a file, then prints it.)");
        }

        private void PrintBTN_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();  //add speech capability

            if (GatherText() == false)   //reset menu array with current textbox contents
            {
                synth.Speak("All menu sections are empty, nothing to print.");  //alert user
                return;  //exit function
            }

            //speak text saving menu then printing file
            synth.Speak("Saving the menu, and printing " + CopiesNUD.Value.ToString() + " copies.");

            //open a file in OneDrive\Documents\ttma.txt
            System.IO.StreamWriter MenuFile = new System.IO.StreamWriter(Environment.GetEnvironmentVariable("onedriveconsumer") + "\\documents\\TTMA.txt");
            GatherText();  //update menu array
            MenuFile.WriteLine(Menu[0]);  //business name

            for (int i = 1; i < Menu.Length; i++)  //for each element, save to file with blank line after
            {
                MenuFile.WriteLine(Menu[i]);
                MenuFile.WriteLine();
            }

            MenuFile.Close();  //close file
                               //print 4 copies of the file that was jest made

            for (int p = 0; p < CopiesNUD.Value; p++)
            {
                System.Diagnostics.Process.Start("notepad", "/p " + Environment.GetEnvironmentVariable("onedriveconsumer") + "\\documents\\TTMA.txt");
            }
        }
    }  //end class
}  //end namespace