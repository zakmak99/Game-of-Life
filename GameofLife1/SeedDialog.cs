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
    public partial class SeedDialog : Form
    {
        public SeedDialog()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            numericSeed.Value = rand.Next();
        }
        public int Seed
        {
            get { return (int)numericSeed.Value; }
            set { numericSeed.Value = value; }
        }
    }
}
