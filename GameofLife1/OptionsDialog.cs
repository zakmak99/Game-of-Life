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
    public partial class OptionsDialog : Form
    {
        public OptionsDialog()
        {
            InitializeComponent();
        }

        public int GridHeight
        {
            get
            {
                return (int)numericGridHeight.Value;
            }
            set
            {
                numericGridHeight.Value = value;
            }
        }
        public int GridWidth
        {
            get { return (int)numericGridWidth.Value; }
            set { numericGridWidth.Value = value; }
        }
        public int GenInterval
        {
            get { return  (int)numericInterval.Value; }
            set { numericInterval.Value = value; }

        }
    }
}
