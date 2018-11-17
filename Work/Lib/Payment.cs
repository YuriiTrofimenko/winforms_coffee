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
    public partial class Payment : UserControl, IActivControl, IDeadMouse
    {
        public event EventHandler<WorkEventArgs> ActivControl = delegate { };
        public event EventHandler DeadMouse = delegate { };
        public event EventHandler ProductIsPaid = delegate { };
        DeadMouseTimer dmTimer;

        public Payment()
        {
            InitializeComponent();
        }

        public void Start(object sender, EventArgs e)
        {
            if(MainForm.SelectedNumber != 0)               // Если выбор был сделан
            {
                // Находим индекс выбранного продукта
                int index = MainForm.SelectedNumber - 1;
                // Название - в Label  lbName.Text
                lbName.Text = MainForm.Settings.Product[index].Name; 
                // Цена - в Label lbPrice.Text
                lbPrice.Text = MainForm.Settings.Product[index].Price.ToString();   
            }

            // Запуск таймера DeadMouse
            StartDeadMouseTimer(this, DeadMouse, MainForm.Settings.TimeDeadMouse);

            // Запуск события (ActivControl)
            ActivControl(this, new WorkEventArgs(MainForm.Settings.BackgroundColor));
        }

        /// <summary>
        /// Запуск таймера DeadMouse
        /// </summary>
        /// <param name="ctrl">Контрол запрашивающий событие неактивной миши</param>
        /// <param name="evt">Ссылка на генерируемое событие контрола</param>
        /// <param name="interval">Время до наступления события в мс.</param>
        public void StartDeadMouseTimer(Control ctrl, EventHandler evt, int interval)
        {
            dmTimer = new DeadMouseTimer(ctrl, evt, interval);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dmTimer.Stop();     // Прекращение работы таймера DeadMouse
            dmTimer.Dispose();  // Освобождение ресурсов
            DeadMouse(this, new EventArgs());   // Возврат на старновый контрол,
                                                // путем вызова события DeadMouse
        }

        private void btnSimulation_Click(object sender, EventArgs e)
        {
            dmTimer.Stop();     // Прекращение работы таймера DeadMouse
            dmTimer.Dispose();  // Освобождение ресурсов
            ProductIsPaid(this, new EventArgs());// Вызываем событие ProductIsPaid
        }
    }
}
