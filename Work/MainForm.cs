using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Work.Lib;

namespace Work
{
    public partial class MainForm : Form
    {
        StartControl startControl = new StartControl();
        Login login = new Login();
        CustomSelect customSelect = new CustomSelect();
        Settings settings = new Settings();
        Payment payment = new Payment();
        Finish finish = new Finish();

        private Control activeControl;       // Поле ссылки на активный Control
        public static WorkSettings Settings { get; set; }   // Настройки

        // Номер выбранного продукта (0 - продукт не выбран)
        public static int SelectedNumber { get; set; }     

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Settings = new WorkSettings();  // Создание экземпляра настроек
    
            // Добавление созданных контролов на главную форму
            this.Controls.Add(startControl);
            this.Controls.Add(login);
            this.Controls.Add(customSelect);
            this.Controls.Add(settings);
            this.Controls.Add(payment);
            this.Controls.Add(finish);

            // Настройка контролов и подписка на события от них
            foreach (Control ctrl in this.Controls)
            {
                var ac = ctrl as IActivControl; // Если контрол
                if (ac != null)       // реализует интерфейс IActivControl
                {
                    ctrl.Visible = false;
                    ctrl.Location = ctrl.Align(HorizontalAlign.Centre, 
                                                VerticalAlign.Centre);
                    // Подписка на событие ActivControl
                    ac.ActivControl += new EventHandler<WorkEventArgs>
                                                    (Set_ActivControl);
                }

                var dm = ctrl as IDeadMouse;    // Если контрол
                if (dm != null)// реализует интерфейс IDeadMouse, подписываемся
                                                // на событие DeadMouse
                    dm.DeadMouse += new EventHandler(startControl.Start);

                var sr = ctrl as ISettingsIsRead;
                if (sr != null)
                    Settings.WorkSettingsIsRead += new EventHandler
                                                    (sr.SettingsIsRead);
            }

            // Подписка на события контролов
            startControl.AdminClick += new EventHandler(login.Start);
            startControl.UserClick += new EventHandler(customSelect.Start);
            login.btnCancel.Click += new EventHandler(startControl.Start);
            login.LogOK += new EventHandler(settings.Start);
            settings.btnCancel.Click += new EventHandler(startControl.Start);
            customSelect.ProductSelected += new EventHandler(payment.Start);
            payment.ProductIsPaid += new EventHandler(finish.Start);

            // Чтение настроек из файла
            Settings.Read("Settings.dat");

            // Запуск стартового контрола
            startControl.Start(this, new EventArgs());
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                var ac = ctrl as IActivControl; // Если контрол реализует
                if (ac != null) // интерфейс IActivControl - 
                                //изменяем его положение на форме
                    ctrl.Location = ctrl.Align(HorizontalAlign.Centre, 
                                                VerticalAlign.Centre);
            }
        }

        // Обработчик запроса на изменение активного контрола
        private void Set_ActivControl(object sender, WorkEventArgs e)
        {
            if (activeControl != null)// Если активный контрол был установлен -
                activeControl.Visible = false;// делаем его невидимым (неактивным),
             // Делаем активным - контрол отправитель запроса
            activeControl = (Control)sender;
            activeControl.Visible = true;       // и отображаем его.
            this.BackColor = e.BackColor;       // Цвет фона формы
        }

    }
}
