using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabTDS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            var solver = new ResolutionSolver();
            
            var solverResult = solver.Solve(textBox1.Text);
            textBox2.Text = string.Join(Environment.NewLine, solverResult.Steps);

            if (solverResult.Items[solverResult.Items.Count - 1].Stmts.Count == 0)
            {
                textBox2.Text += $"{Environment.NewLine}{Environment.NewLine}Доказано";
            }
            else
            {
                textBox2.Text += $"${Environment.NewLine}{Environment.NewLine}Не доказано";
            }
        }
    }
}
