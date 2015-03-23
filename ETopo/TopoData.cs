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

        private void btOk_Click(object sender, EventArgs e)
        {
            name = tbName.Text;
            date = cbDate.Value;
            autor = tbAutor.Lines.ToList();
            Close();
        }

        private void FrTopoData_Load(object sender, EventArgs e)
        {
            tbName.Text = name;
            try
            {
                cbDate.Value = Convert.ToDateTime(date);
            }
            catch 
            {
                cbDate.Value = DateTime.Now;
            }
            tbAutor.Lines = autor == null ? null : autor.ToArray();
        }
    }
}
