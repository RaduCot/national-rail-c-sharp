using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NationalRail
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Angajati angajatiForm = new Angajati();
            angajatiForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bilete bileteForm = new Bilete();
            bileteForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Trenuri trenuriForm = new Trenuri();
            trenuriForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Transporturi transporturiForm = new Transporturi();
            transporturiForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Calatorii calatoriiForm = new Calatorii();
            calatoriiForm.Show();
        }
    }
}
