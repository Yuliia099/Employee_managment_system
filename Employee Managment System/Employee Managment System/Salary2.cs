using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee_Managment_System
{
    public partial class Salary2 : Form
    {
        public Salary2()
        {
            InitializeComponent();
            
        }
        public class EmployeeSalary
        {
            public int Manager { get; set; }
            public int JuniorDeveloper { get; set; }
            public int SeniorDeveloper { get; set; }
            public int Accountant { get; set; }
            public int Receptionist { get; set; }

            public EmployeeSalary()
            {
                Manager = 600;
                JuniorDeveloper = 450;
                SeniorDeveloper = 800;
                Accountant = 500;
                Receptionist = 400;
            }

            public int CalculateSalary(string position, int workDays)
            {
                int dailySalary = 0;
                switch (position)
                {
                    case "Manager":
                        dailySalary = Manager;
                        break;
                    case "Junior Developer":
                        dailySalary = JuniorDeveloper;
                        break;
                    case "Senior Developer":
                        dailySalary = SeniorDeveloper;
                        break;
                    case "Accountant":
                        dailySalary = Accountant;
                        break;
                    case "Receptionist":
                        dailySalary = Receptionist;
                        break;
                }
                return dailySalary * workDays;
            }
        }

        private void CalculateBtn_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int workDays))
            {
                MessageBox.Show("Будь ласка, введіть коректну кількість робочих днів.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedPosition = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedPosition))
            {
                MessageBox.Show("Будь ласка, виберіть посаду.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EmployeeSalary employee = new EmployeeSalary();
            int salary = employee.CalculateSalary(selectedPosition, workDays);
            richTextBox1.Text = $"Зарплата {selectedPosition} за {workDays} дні(в): {salary}";
        }

        

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home();
            homeForm.Show();
            this.Hide();
        }
    }
}
