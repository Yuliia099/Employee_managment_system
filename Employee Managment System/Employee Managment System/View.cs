using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Employee_Managment_System
{
    public partial class View : Form
    {
        private Dictionary<string, object> employeeData;
        public View()
        {
            InitializeComponent();
            LoadEmployeeData();
           
        }
        private void LoadEmployeeData()
        {
            employeeData = StateManager.LoadState();
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            string input = EmployeeID.Text;

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Будь ласка, введіть ID або ім'я працівника.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isID = int.TryParse(input, out int employeeID);

            if (isID)
            {
                SearchByID(employeeID.ToString());
            }
            else
            {
                SearchByName(input);
            }
        }

        private void SearchByID(string employeeID)
        {
            if (employeeData.ContainsKey(employeeID))
            {
                var employeeInfo = (Dictionary<string, object>)employeeData[employeeID];

                label18.Text = employeeID;
                label11.Text = employeeInfo["EmployeeName"].ToString();
                label13.Text = employeeInfo["EmployeePosition"].ToString();
                label12.Text = employeeInfo["EmployeeGender"].ToString();
                label14.Text = employeeInfo["EmployeeEducation"].ToString();
                label5.Text = employeeInfo["EmployeeAddress"].ToString();
                label16.Text = employeeInfo["EmployeePhone"].ToString();
                label17.Text = employeeInfo["EmployeeDoB"].ToString();
            }
            else
            {
                MessageBox.Show($"Працівник з ID {employeeID} не знайдений.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchByName(string employeeName)
        {
            var employeeEntry = employeeData
                .FirstOrDefault(emp => ((Dictionary<string, object>)emp.Value)["EmployeeName"].ToString().Equals(employeeName, StringComparison.OrdinalIgnoreCase));

            if (!employeeEntry.Equals(default(KeyValuePair<string, object>)))
            {
                var employeeID = employeeEntry.Key;
                var employeeInfo = (Dictionary<string, object>)employeeEntry.Value;

                label18.Text = employeeID;  
                label11.Text = employeeInfo["EmployeeName"].ToString();
                label13.Text = employeeInfo["EmployeePosition"].ToString();
                label12.Text = employeeInfo["EmployeeGender"].ToString();
                label14.Text = employeeInfo["EmployeeEducation"].ToString();
                label5.Text = employeeInfo["EmployeeAddress"].ToString();
                label16.Text = employeeInfo["EmployeePhone"].ToString();
                label17.Text = employeeInfo["EmployeeDoB"].ToString();
            }
            else
            {
                MessageBox.Show($"Працівник з ім'ям {employeeName} не знайдений.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void HomeBtn_Click(object sender, EventArgs e)
         {
            Home homeForm = new Home();
            homeForm.Show();
            this.Hide();
         }

       

        private void SearchBtn2_Click(object sender, EventArgs e)
        {
            Info();
        }
        private void Info()
        {
           
            string selectedProfession = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedProfession))
            {
                MessageBox.Show("Будь ласка, оберіть професію.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            listBox1.Items.Clear();
            bool employeesFound = false;

            foreach (var employee in employeeData)
            {
                string employeeId = employee.Key;
                Dictionary<string, object> employeeInfo = (Dictionary<string, object>)employee.Value;
                if (employeeInfo["EmployeePosition"].ToString() == selectedProfession)
                {
                    string employeeName = employeeInfo["EmployeeName"].ToString();
                    listBox1.Items.Add($"{employeeId} - {employeeName}");
                    employeesFound = true; 
                }
            }

            
            if (!employeesFound)
            {
                MessageBox.Show($"Для професії '{selectedProfession}' немає співробітників.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Info();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
    }
}
