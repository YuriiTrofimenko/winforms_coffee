using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Work.Lib
{
    public partial class Login : UserControl, IActivControl
    {
        public event EventHandler LogOK = delegate { };
        public event EventHandler<WorkEventArgs> ActivControl = delegate { };

        public Login()
        {
            InitializeComponent();
        }

        public void Start(object sender, EventArgs e)
        {
            tbLogin.Text = string.Empty;    // Пустое начальное значение TextBox логина
            tbPass.Text = string.Empty;     // Пустое начальное значение TextBox пароля
            // Запускаем событие (ActivControl)
            ActivControl(this, new WorkEventArgs(MainForm.Settings.BackgroundColor));
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool checkLog = false;

            // Проверка логина и пароля ***********************************************
            if(tbLogin.Text == "1" && tbPass.Text == "1")
                checkLog = true;

            if (checkLog)                   // Если логин и пароль правильные
                LogOK(this, new EventArgs());// запускаем событие LogOK
            else
                MessageBox.Show("Неправильный логин или пароль", "Авторизация");
        }

    }

}
