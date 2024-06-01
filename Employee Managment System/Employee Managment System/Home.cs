
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Employee_Managment_System
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Employee employeeForm = new Employee();
            employeeForm.Show();

            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            View obj = new View();
            obj.Show();
            this.Hide();
        }

        private void SalaryBtn_Click(object sender, EventArgs e)
        {
            Salary2 obj = new Salary2();
            obj.Show();
            this.Hide();
        }
    }
}
