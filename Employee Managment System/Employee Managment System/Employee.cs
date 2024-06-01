using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Employee_Managment_System
{
    public partial class Employee : Form
    {
        private readonly Dictionary<string, string> columnNameMap = new Dictionary<string, string>()
        {
            { "ID", "EmployeeID" },
            { "Name", "EmployeeName" },
            { "Position", "EmployeePosition" },
            { "Gender", "EmployeeGender" },
            { "Education", "EmployeeEducation" },
            { "Address", "EmployeeAddress" },
            { "Phone", "EmployeePhone" },
            { "DoB", "EmployeeDoB" },
        };

        public Employee()
        {
            InitializeComponent();
            LoadEmployeeData();
            
        }
        public class EmployeeDetails
        {
            public static Dictionary<string, object> save = new Dictionary<string, object>();
            public int EmployeeID { get; private set; }
            public string EmployeeName { get; private set; }
            public string EmployeePosition { get; private set; }
            public string EmployeeGender { get; private set; }
            public string EmployeeEducation { get; private set; }
            public string EmployeeAddress { get; private set; }
            public string EmployeePhone { get; private set; }
            public DateTime EmployeeDoB { get; private set; }

            
            public EmployeeDetails(int ID, string Name, string Position, string Gender, string Education, string Address, string Phone, DateTime DoB)
            {
                EmployeeID = ID;
                EmployeeName = Name;
                EmployeePosition = Position;
                EmployeeGender = Gender;
                EmployeeEducation = Education;
                EmployeeAddress = Address;
                EmployeePhone = Phone;
                EmployeeDoB = DoB;
            }
        }

      
        private void LoadEmployeeData()
        {
            Dictionary<string, object> employeeData = StateManager.LoadState();
            if (employeeData != null)
            {
                foreach (KeyValuePair<string, object> kvp in employeeData)
                {
                    Dictionary<string, object> employeeInfo = (Dictionary<string, object>)kvp.Value;
                    dataGridView2.Rows.Add(
                        employeeInfo["EmployeeID"],
                        employeeInfo["EmployeeName"],
                        employeeInfo["EmployeePosition"],
                        employeeInfo["EmployeeGender"],
                        employeeInfo["EmployeeEducation"],
                        employeeInfo["EmployeeAddress"],
                        employeeInfo["EmployeePhone"],
                        employeeInfo["EmployeeDoB"]
                    );
                }
            }
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EmployeeDetails employee = new EmployeeDetails(
           int.Parse(EmployeeID.Text),
           EmployeeName.Text,
           EmployeePosition.SelectedItem.ToString(),
           EmployeeGender.SelectedItem.ToString(),
           EmployeeEducation.SelectedItem.ToString(),
           EmployeeAddress.Text,
           EmployeePhone.Text,
           DateTime.Parse(EmployeeDoB.Value.ToString("yyyy-MM-dd"))
           );


            dataGridView2.Rows.Add(
                employee.EmployeeID,
                employee.EmployeeName,
                employee.EmployeePosition,
                employee.EmployeeGender,
                employee.EmployeeEducation,
                employee.EmployeeAddress,
                employee.EmployeePhone,
                employee.EmployeeDoB
            );

            Dictionary<string, object> employeeData = new Dictionary<string, object>()
            {
                { "EmployeeID", employee.EmployeeID },
                { "EmployeeName", employee.EmployeeName },
                { "EmployeePosition", employee.EmployeePosition },
                { "EmployeeGender", employee.EmployeeGender },
                { "EmployeeEducation", employee.EmployeeEducation },
                { "EmployeeAddress", employee.EmployeeAddress },
                { "EmployeePhone", employee.EmployeePhone },
                { "EmployeeDoB", employee.EmployeeDoB }
            };

            Dictionary<string, object> existingData = StateManager.LoadState();
            existingData[employee.EmployeeID.ToString()] = employeeData;
            StateManager.SaveState(existingData);
        }

        private bool ValidateFields()
        {
            return !(
                string.IsNullOrWhiteSpace(EmployeeID.Text) ||
                string.IsNullOrWhiteSpace(EmployeeName.Text) ||
                EmployeePosition.SelectedItem == null ||
                EmployeeGender.SelectedItem == null ||
                EmployeeEducation.SelectedItem == null ||
                string.IsNullOrWhiteSpace(EmployeeAddress.Text) ||
                string.IsNullOrWhiteSpace(EmployeePhone.Text)
            );
        }

       

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home();
            homeForm.Show();
            this.Hide();
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                string employeeID = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                DialogResult result = MessageBox.Show($"Ви дійсно бажаєте видалити працівника з ID {employeeID}?", "Підтвердження видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dataGridView2.Rows.Remove(dataGridView2.SelectedRows[0]);
                    DeleteEmployeeData(employeeID);
                    MessageBox.Show("Працівника успішно видалено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть працівника для видалення.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteEmployeeData(string employeeID)
        {
            string filePath = Path.Combine(Application.StartupPath, "saves", "data.json");
            Dictionary<string, object> existingData = StateManager.LoadState();

            if (existingData.ContainsKey(employeeID))
            {
                existingData.Remove(employeeID);
            }
            else
            {
                MessageBox.Show($"Дані працівника з ID {employeeID} не знайдено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StateManager.SaveState(existingData);

            filePath = Path.Combine(Application.StartupPath, "saves", $"{employeeID}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                    string employeeID = selectedRow.Cells["ID"].Value?.ToString();

                    if (ValidateRow(selectedRow))
                    {
                        int parsedID;
                        DateTime parsedDoB;

                        if (!int.TryParse(employeeID, out parsedID))
                        {
                            MessageBox.Show("Неправильний формат ID.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!DateTime.TryParse(selectedRow.Cells["DoB"].Value?.ToString(), out parsedDoB))
                        {
                            MessageBox.Show("Неправильний формат дати народження.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                       
                        EmployeeDetails employee = new EmployeeDetails(
                            parsedID,
                            selectedRow.Cells["Name"].Value?.ToString(),
                            selectedRow.Cells["Position"].Value?.ToString(),
                            selectedRow.Cells["Gender"].Value?.ToString(),
                            selectedRow.Cells["Education"].Value?.ToString(),
                            selectedRow.Cells["Address"].Value?.ToString(),
                            selectedRow.Cells["Phone"].Value?.ToString(),
                            parsedDoB
                        );

                        Dictionary<string, object> employeeData = new Dictionary<string, object>()
                {
                    { "EmployeeID", employee.EmployeeID },
                    { "EmployeeName", employee.EmployeeName },
                    { "EmployeePosition", employee.EmployeePosition },
                    { "EmployeeGender", employee.EmployeeGender },
                    { "EmployeeEducation", employee.EmployeeEducation },
                    { "EmployeeAddress", employee.EmployeeAddress },
                    { "EmployeePhone", employee.EmployeePhone },
                    { "EmployeeDoB", employee.EmployeeDoB }
                };

                        Dictionary<string, object> existingData = StateManager.LoadState();
                        if (existingData.ContainsKey(employeeID))
                        {
                            existingData[employeeID] = employeeData;
                            StateManager.SaveState(existingData);
                            MessageBox.Show("Дані працівника успішно оновлені.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Дані працівника не знайдено у словнику.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Переконайтеся, що всі поля обраного рядка заповнені.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Будь ласка, оберіть рядок для оновлення.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Виникла помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool ValidateRow(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            EmployeeID.Text = "";
            EmployeePosition.SelectedIndex = -1;
            EmployeeEducation.SelectedIndex = -1;
            EmployeeGender.SelectedIndex = -1;
            EmployeeName.Text = "";
            EmployeeAddress.Text = "";
            EmployeePhone.Text = "";
            EmployeeDoB.Value = DateTime.Now;
        }
    }
    public class StateManager : Form
    {
        public static void SaveState(Dictionary<string, object> state)
        {
            string json = JsonConvert.SerializeObject(state, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            string directoryPath = Path.Combine(Application.StartupPath, "saves");
            Directory.CreateDirectory(directoryPath);
            string filePath = Path.Combine(directoryPath, "data.json");
            File.WriteAllText(filePath, json);

        }

        public static Dictionary<string, object> LoadState()
        {
            string filePath = Path.Combine(Application.StartupPath, "saves", "data.json");
            string json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
        }
    }
}
