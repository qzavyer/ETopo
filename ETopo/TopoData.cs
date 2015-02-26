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
            date = cbDate.Value.ToString("dd.MM.yyyy");
            autor = tbAutor.Lines.ToList();
            Close();
        }

        private void FrTopoData_Load(object sender, EventArgs e)
        {
            tbName.Text = name;
            cbDate.Value = date.Length == 0 ? DateTime.Now : Convert.ToDateTime(date);
            tbAutor.Lines = autor == null ? null : autor.ToArray();
        }
    }
}
