using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameofLife1
{
    public partial class Form1 : Form
    {
        int gridX = 15;
        int gridY = 15;
        // The universe array
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

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
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
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

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

                    if (neighborCountToolStripMenuItem.Checked == true)
                    {
                        Font font = new Font("Arial", 10f);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        Rectangle rect = new Rectangle(x, y, 100, 100);
                        int neighbors = CountNeighbors(x,y);
                        if (neighbors > 0)
                        {
                            e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, cellRect, stringFormat);
                        }
                    }
                }
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
                float cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                float cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = (int)(e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = (int)(e.Y / cellHeight);

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

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

        private int CountNeighbors(int x, int y)
        {
            int count = 0;
            if (toroidalToolStripMenuItem.Checked)
            {count = CountNeighborsToroidal(x,y); }
            if (finiteToolStripMenuItem.Checked)
            { count = CountNeighborsFinite(x, y); }

            return count;
        }
        #region menuItems

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (finiteToolStripMenuItem.Checked == true)
            {
                finiteToolStripMenuItem.Checked = false;
                toroidalToolStripMenuItem.Checked = true;
            }
        }

        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toroidalToolStripMenuItem.Checked == true)
            {
                toroidalToolStripMenuItem.Checked = false;
                finiteToolStripMenuItem.Checked = true;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] temp = new bool[gridX, gridY];
            universe = temp;
            generations = 0;
            timer.Enabled = false;
            graphicsPanel1.Invalidate();

        }
        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (neighborCountToolStripMenuItem.Checked == true)
                neighborCountToolStripMenuItem.Checked = false;
            else if (neighborCountToolStripMenuItem.Checked == false)
                neighborCountToolStripMenuItem.Checked = true;
            graphicsPanel1.Invalidate();
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }
        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }
        }

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
        #endregion

        #region toolStrips
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            bool[,] temp = new bool[gridX, gridY];
            universe = temp;
            generations = 0;
            timer.Enabled = false;
            graphicsPanel1.Invalidate();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }



        #endregion

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDialog dlg = new OptionsDialog();
            dlg.GridHeight = gridY;
            dlg.GridWidth = gridX;
            dlg.GenInterval = timer.Interval;
            if ( DialogResult.OK == dlg.ShowDialog())
            {
                gridX = dlg.GridWidth;
                gridY = dlg.GridHeight;
                timer.Interval = dlg.GenInterval;
                universe = new bool[gridX, gridY];
                scratchPad = new bool[gridX, gridY];

                graphicsPanel1.Invalidate();
            }
        }

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

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridX = Properties.Settings.Default.GridX;
            gridY = Properties.Settings.Default.GridY;
            timer.Interval = Properties.Settings.Default.GenInterval;
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridX = Properties.Settings.Default.GridX;
            gridY = Properties.Settings.Default.GridY;
            timer.Interval = Properties.Settings.Default.GenInterval;
        }

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
                       if ( rnd.Next(0, 2) == 1)
                        {
                            universe[x, y] = true;
                        }
                    }
                }
                graphicsPanel1.Invalidate();
            }
        }

        private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
