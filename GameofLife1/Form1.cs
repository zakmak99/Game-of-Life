using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameofLife1
{
    public partial class Form1 : Form
    {
        // Boundry Type
        string boundryType;
        // Alive cell count
        int cellCount = 0;
        // Default size values
        int gridX = 15;
        int gridY = 15;
        // The universe array]
        bool[,] universe = new bool[15, 15];
        bool[,] scratchPad = new bool[15, 15];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;
        public Form1()
        {
            InitializeComponent();
            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running

            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridX = Properties.Settings.Default.GridX;
            gridY = Properties.Settings.Default.GridY;
            timer.Interval = Properties.Settings.Default.GenInterval;
            universe = new bool[gridX, gridY];
            scratchPad = new bool[gridX, gridY];
           
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int count = CountNeighbors(x, y);
                    if (universe[x, y] == true)
                    {
                        if (count < 2)
                        { scratchPad[x, y] = false; }
                        if (count > 3)
                        { scratchPad[x, y] = false; }
                        if (count == 2 || count == 3)
                        { scratchPad[x, y] = true; }
                    }
                    if (universe[x, y] == false)
                    {
                        if (count == 3)
                        { scratchPad[x, y] = true; }
                        else
                        { scratchPad[x, y] = false; }
                    }
                }
            }
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

                    // Increment generation count
                    generations++;
            CellCount();
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            // Update status strip living cells
            toolStripStatusLabelCells.Text = "Living cells = " + cellCount.ToString();
            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    // Prints the neighbor count in each cell
                    if (neighborCountToolStripMenuItem.Checked == true)
                    {
                        Font font = new Font("Arial", 10f);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        Rectangle rect = new Rectangle(x, y, (int)cellRect.Width, (int)cellRect.Height);
                        int neighbors = CountNeighbors(x,y);
                        if (neighbors > 0)
                        {
                            e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, cellRect, stringFormat);
                        }
                    }

                    
                }
            }
            //Displays the HUD if enabled
            if (hUDToolStripMenuItem.Checked == true)
            {
                Font font = new Font("Arial", 15f);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Far;
                e.Graphics.DrawString("Alive Cells = " + cellCount.ToString() + "\nGeneration = " + generations.ToString() + "\nBoundry Type = " + boundryType + "\nUniverse Size: Width = " + gridX.ToString() + " Height = " + gridY.ToString(), font, Brushes.Red, graphicsPanel1.ClientRectangle, stringFormat);
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = (int)(e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = (int)(e.Y / cellHeight);

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Update alive cell count
                CellCount();
                toolStripStatusLabelCells.Text = "Living Cells = " + cellCount.ToString();
                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if ( xOffset == 0 && yOffset == 0)
                    { continue; }
                    // if xCheck is less than 0 then continue
                    if ( xCheck < 0)
                    { continue; }
                    // if yCheck is less than 0 then continue
                    if ( yCheck < 0)
                    { continue; }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    { continue; }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    { continue; }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    { continue; }
                    // if xCheck is less than 0 then set to xLen - 1
                    if( xCheck < 0)
                    { xCheck = xLen -1; }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    { yCheck = yLen -1; }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    { xCheck = 0; }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    { yCheck = 0; }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        // Handles Neighbor count based off which box is checked
        private int CountNeighbors(int x, int y)
        {
            int count = 0;
            if (toroidalToolStripMenuItem.Checked)
            {
                count = CountNeighborsToroidal(x,y);
                boundryType = "Toroidal";
            }
            if (finiteToolStripMenuItem.Checked)
            { 
                count = CountNeighborsFinite(x, y);
                boundryType = "Finite";
            }

            return count;
        }
        //counts living cells
        private void CellCount()
        {
            int alive = 0;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x,y] == true) alive++;
                }
            }
            cellCount = alive;
        }
        #region menuItems
        // Exits the program
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // Toggles the Finite switch off and checks the Toroidal switch
        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (finiteToolStripMenuItem.Checked == true)
            {
                finiteToolStripMenuItem.Checked = false;
                toroidalToolStripMenuItem.Checked = true;
            }
            graphicsPanel1.Invalidate();
        }
        // Toggle the Toroidal switch off and checks the Finite switch
        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toroidalToolStripMenuItem.Checked == true)
            {
                toroidalToolStripMenuItem.Checked = false;
                finiteToolStripMenuItem.Checked = true;
            }
            graphicsPanel1.Invalidate();
        }
        // Wipes the universe and resets relevent counters
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] temp = new bool[gridX, gridY];
            universe = temp;
            generations = 0;
            timer.Enabled = false;
            CellCount();
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelCells.Text = "Living cells = " + cellCount.ToString();
            graphicsPanel1.Invalidate();

        }// Toggles the switch and determines if the information is displayed
        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (neighborCountToolStripMenuItem.Checked == true)
                neighborCountToolStripMenuItem.Checked = false;
            else if (neighborCountToolStripMenuItem.Checked == false)
                neighborCountToolStripMenuItem.Checked = true;
            graphicsPanel1.Invalidate();
        }
        // Enables the timer
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
        // Diables the timer
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
        // Moves forward one generation
        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }
        // Displays the color dialog menu and updates the background color
        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }
        }
        // Displays the color dialog menu and updates the cell color
        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = cellColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }
        // Displays the color dialog menu and updates the grid color
        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = gridColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }
        // Loads the base values and resets the universe and counters
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridX = Properties.Settings.Default.GridX;
            gridY = Properties.Settings.Default.GridY;
            timer.Interval = Properties.Settings.Default.GenInterval;
            universe = new bool[gridX, gridY];
            timer.Enabled = false;
            generations = 0;
            cellCount = 0;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelCells.Text = "Living cells = " + cellCount.ToString();
            graphicsPanel1.Invalidate();
        }
        // Loads the settings from the last launch
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridX = Properties.Settings.Default.GridX;
            gridY = Properties.Settings.Default.GridY;
            timer.Interval = Properties.Settings.Default.GenInterval;
            universe = new bool[gridX, gridY];
            timer.Enabled = false;
            generations = 0;
            cellCount = 0;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelCells.Text = "Living cells = " + cellCount.ToString();
            graphicsPanel1.Invalidate();
        }
        // gets the value input by the user and randomizes the univese based off of it
        private void fromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int seed = 0;
            SeedDialog dlg = new SeedDialog();
            dlg.Seed = seed;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                seed = dlg.Seed;
                Random rnd = new Random(seed);
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Iterate through the universe in the x, left to right
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        if (rnd.Next(0, 2) == 0)
                        {
                            universe[x, y] = true;
                        }
                        else
                        {
                            universe[x, y] = false;
                        }
                    }
                }
                timer.Enabled = false;
                generations = 0;
                CellCount();
                toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
                toolStripStatusLabelCells.Text = "Living cells = " + cellCount.ToString();
                graphicsPanel1.Invalidate();
            }
        }
        // Using the system time randomizes the universe
        private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        universe[x, y] = true;
                    }
                    else
                    {
                        universe[x, y] = false;
                    }
                }
            }
            timer.Enabled = false;
            generations = 0;
            CellCount();
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelCells.Text = "Living cells = " + cellCount.ToString();
            graphicsPanel1.Invalidate();
        }
        // Displays the Options menu dialog and updates the changed information
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDialog dlg = new OptionsDialog();
            dlg.GridHeight = gridY;
            dlg.GridWidth = gridX;
            dlg.GenInterval = timer.Interval;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                if (gridX != dlg.GridWidth && gridY != dlg.GridHeight)
                {
                    gridX = dlg.GridWidth;
                    gridY = dlg.GridHeight;
                    universe = new bool[gridX, gridY];
                    scratchPad = new bool[gridX, gridY];
                    generations = 0;
                    cellCount = 0;
                    toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
                    toolStripStatusLabelCells.Text = "Living cells = " + cellCount.ToString();
                }

                timer.Interval = dlg.GenInterval;
                
                timer.Enabled = false;
                
                graphicsPanel1.Invalidate();
            }
        }
        // Toggles the HUD check and determines if the information is displayed
        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hUDToolStripMenuItem.Checked == true)
            {
                hUDToolStripMenuItem.Checked = false;
                hUDToolStripMenuItem1.Checked = false;
            }
            else if (hUDToolStripMenuItem.Checked == false)
            {
                hUDToolStripMenuItem.Checked = true;
                hUDToolStripMenuItem1.Checked = true;
            }
            graphicsPanel1.Invalidate();
        }
        // Opens the save dialog and writes the current univers into a document
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!Test Comment");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        if (universe[x, y] == true)
                            currentRow += 'O';

                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        else if (universe[x, y] == false)
                            currentRow += '.';
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }
        // Opens the file dialog and when a valid document is selected rewrites the universe to the specified pattern
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (row.StartsWith("!"))
                        continue;
                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    else maxHeight++;
                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    if (maxWidth == 0)
                        maxWidth = row.Length;
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                gridX = maxWidth;
                gridY = maxHeight;
                universe = new bool[maxWidth, maxHeight];
                scratchPad = new bool[maxWidth, maxHeight];
                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                // Track y value
                int yPos = 0;
                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {

                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.StartsWith("!"))
                        continue;
                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    else
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.
                        if (row[xPos] == 'O')
                                universe[xPos, yPos] = true;
                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                        if (row[xPos] == '.')
                                universe[xPos, yPos] = false; ;

                    }
                    yPos++;
                }

                // Close the file.
                reader.Close();
            }
            // Update displayed values
            generations = 0;
            CellCount();
            timer.Enabled = false;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelCells.Text = "Living cells = " + cellCount.ToString();
            graphicsPanel1.Invalidate();
        }

        #endregion

        #region toolStrips
        //         // Wipes the universe and resets relevent counters
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            bool[,] temp = new bool[gridX, gridY];
            universe = temp;
            generations = 0;
            cellCount = 0;
            timer.Enabled = false;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelCells.Text = "Living cells = " + cellCount.ToString();
            graphicsPanel1.Invalidate();
        }
        // Enables the timer
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
        // Disables the timer
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
        // Advamces the universe by one generation
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }



        #endregion

        
        //Saves setting upon closing program
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.BackColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.GridX = gridX;
            Properties.Settings.Default.GridY = gridY;
            Properties.Settings.Default.GenInterval = timer.Interval;
            Properties.Settings.Default.Save();
        }


    }
}
