using System;
using System.IO;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;

namespace TTMA
{
    public partial class TTMAMain : Form
    {
        #region Constants
        private const int SPEECH_RATE = 3;
        private const int SPEECH_VOLUME = 100;
        private const float FONT_SIZE = 16f;
        #endregion

        #region Variables
        private string[] _menu;
        private readonly SpeechSynthesizer _synth = new SpeechSynthesizer();

        // printing fields
        private PrintDocument _printDoc;
        private string[] _menuToPrint;
        private int _currentPrintLine;
        #endregion

        public TTMAMain()
        {
            InitializeComponent();

            // Show month and day only (no year) for the dates range
            DateTime nextMonday = GetNextMonday(DateTime.Now);
            DatesTB.Text = nextMonday.ToString("M") + " to " + nextMonday.AddDays(4).ToString("M");

            _synth.Rate = SPEECH_RATE;
            _synth.Volume = SPEECH_VOLUME;

            // initialize PrintDocument
            _printDoc = new PrintDocument();
            _printDoc.PrintPage += PrintDoc_PrintPage;
        }

        /// <summary>
        /// Gets the next Monday from the given date. If the date is already Monday, returns the following Monday.
        /// </summary>
        public DateTime GetNextMonday(DateTime date)
        {
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;

            // If today is Monday, daysUntilMonday will be 0, so add 7 to get next Monday
            if (daysUntilMonday == 0)
                daysUntilMonday = 7;

            return date.AddDays(daysUntilMonday);
        }

        /// <summary>
        /// Builds the menu array from current textbox values.
        /// </summary>
        private string[] BuildMenu()
        {
            var menu = new string[8];
            menu[0] = "Totally Tiffany's Cafe";
            menu[1] = DatesTB.Text;
            menu[2] = "Monday - " + MondayTB.Text;
            menu[3] = "Tuesday - " + TuesdayTB.Text;
            menu[4] = "Wednesday - " + WednesdayTB.Text;
            menu[5] = "Thursday - " + ThursdayTB.Text;
            menu[6] = "Friday Breakfast - " + FBreakfastTB.Text;
            menu[7] = "Friday Lunch - " + FLunchTB.Text;
            return menu;
        }

        /// <summary>
        /// Returns true if any user-editable textbox contains non-whitespace text.
        /// </summary>
        private bool HasAnyMenuText()
        {
            return !string.IsNullOrWhiteSpace(MondayTB.Text)
                || !string.IsNullOrWhiteSpace(TuesdayTB.Text)
                || !string.IsNullOrWhiteSpace(WednesdayTB.Text)
                || !string.IsNullOrWhiteSpace(ThursdayTB.Text)
                || !string.IsNullOrWhiteSpace(FBreakfastTB.Text)
                || !string.IsNullOrWhiteSpace(FLunchTB.Text);
        }

        private void ReadBTN_Click(object sender, EventArgs e)
        {
            if (!HasAnyMenuText())
            {
                _synth.SpeakAsync("All menu sections are empty, nothing to speak.");
                return;
            }

            _menu = BuildMenu();
            _synth.SpeakAsyncCancelAll();

            // Queue each line to be spoken
            for (int i = 0; i < _menu.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(_menu[i]))
                    _synth.SpeakAsync(_menu[i] + ".");
            }
        }

        private void HelpBTN_Click(object sender, EventArgs e)
        {
            _synth.SpeakAsyncCancelAll();
            _synth.SpeakAsync("Use the following keyboard keys for Totally Tiffany's Menu Assistant.");
            _synth.SpeakAsync("Type in what you are serving for each day of the week.");
            _synth.SpeakAsync("Up Arrow - Reads the current line. TAB switches between sections.");
            _synth.SpeakAsync("Read Button - Speaks the entire menu out loud. Print Button - Saves and prints.");
        }

        private void PrintBTN_Click(object sender, EventArgs e)
        {
            if (!HasAnyMenuText())
            {
                _synth.Speak("All menu sections are empty, nothing to print.");
                return;
            }

            _menu = BuildMenu();
            int copies = (int)CopiesNUD.Value;
            string copiesText = copies == 1 ? "copy" : "copies";
            _synth.SpeakAsync($"Saving the menu, and printing {copies} {copiesText}.");

            // Save to file
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documents, "TTMA.txt");

            try
            {
                using (var writer = new StreamWriter(filePath, false))
                {
                    writer.WriteLine(_menu[0]);
                    for (int i = 1; i < _menu.Length; i++)
                    {
                        writer.WriteLine(_menu[i]);
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to save menu: " + ex.Message;
                MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _synth.SpeakAsync("Failed to save menu.");
                return;
            }

            // Prepare for printing via PrintDocument
            _menuToPrint = _menu;
            _currentPrintLine = 0;

            try
            {
                // Apply the number of copies requested
                _printDoc.PrinterSettings.Copies = (short)copies;
                _printDoc.Print();
            }
            catch (Exception exPrint)
            {
                string errorMsg = "Printing failed: " + exPrint.Message;
                MessageBox.Show(errorMsg, "Print error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _synth.SpeakAsync("Printing failed.");
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;

            // Use Tahoma font with proper disposal
            using (var font = new Font("Tahoma", FONT_SIZE, FontStyle.Bold))
            {
                var brush = Brushes.Black;

                // Start at the top-left margin
                float x = e.MarginBounds.Left;
                float y = e.MarginBounds.Top;
                float maxWidth = e.MarginBounds.Width;

                // Measure a single blank line height to insert between sections
                float blankLineHeight = g.MeasureString(" ", font, (int)maxWidth).Height;

                // Print until we run out of space or lines
                while (_currentPrintLine < _menuToPrint.Length)
                {
                    string line = _menuToPrint[_currentPrintLine];

                    if (string.IsNullOrEmpty(line))
                    {
                        // Add blank line spacing
                        y += blankLineHeight;
                        _currentPrintLine++;
                        continue;
                    }

                    // Measure string height when wrapped to the page width
                    SizeF sz = g.MeasureString(line, font, (int)maxWidth);

                    // Check if there's enough space for this line plus the extra blank line
                    if (y + sz.Height + blankLineHeight > e.MarginBounds.Bottom)
                    {
                        // Not enough space; request another page
                        break;
                    }

                    var layout = new RectangleF(x, y, maxWidth, sz.Height);
                    g.DrawString(line, font, brush, layout);

                    // Advance by the string height and add one blank line between sections
                    y += sz.Height + blankLineHeight;
                    _currentPrintLine++;
                }
            }

            // Determine if more pages are required
            if (_currentPrintLine < _menuToPrint.Length)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
                // Reset for next print job
                _currentPrintLine = 0;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            try
            {
                _synth.SpeakAsyncCancelAll();
                _synth.Dispose();
            }
            catch { /* swallow errors during closing */ }

            try
            {
                _printDoc?.Dispose();
            }
            catch { /* swallow errors during closing */ }
        }
    }
}