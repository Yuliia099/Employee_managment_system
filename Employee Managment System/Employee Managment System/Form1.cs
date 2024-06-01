using Microsoft.Win32;
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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Username.Text) || string.IsNullOrEmpty(Password.Text))
                {
                    throw new Exception("Відсутня інформація");
                }

                if (Username.Text == "My login" && Password.Text == "123456")
                {
                    Home H = new Home();
                    H.Show();
                    this.Hide();
                }
                else
                {
                    throw new Exception("Введіть правильний логін та пароль");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
    }
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            Username.Text = "";
            Password.Text = "";
        }

    }
}
