using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClockV2.View
{
    public partial class DialougeAdd : Form
    {
        public DialougeAdd()
        {
            InitializeComponent();
            DTPicker.CustomFormat = "dd/MM/yyyy @ hh:mm:ss";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void DialougeAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
