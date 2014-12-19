using System;
using System.Linq;
using System.Windows.Forms;

namespace ETopo
{
    public partial class FrTopoData : Form
    {
        public FrTopoData()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btOk_Click(object sender, EventArgs e)
        {
            name = tbName.Text;
            date = cbDate.Text;
            autor = tbAutor.Lines.ToList();
            Close();
        }
    }
}
