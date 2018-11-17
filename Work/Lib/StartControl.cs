﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Work.Properties;

namespace Work.Lib
{
    public partial class StartControl : UserControl, IActivControl
    {
        public event EventHandler<WorkEventArgs> ActivControl;
        public event EventHandler AdminClick;
        public event EventHandler UserClick; 

        public StartControl()
        {
            InitializeComponent();
        }

        private void StartControl_Load(object sender, EventArgs e)
        {
            //picPB.Size = picPB.Image.Size;        // Размер PictureBox в размер картинки
                             // Выравнивание относительно границ (StartControl)
            picPB.Location = picPB.Align(HorizontalAlign.Centre, VerticalAlign.Bottom);

            //pointPB.Size = pointPB.Image.Size;    // Размер PictureBox в размер картинки
                                        // Выравнивание относительно границ (StartControl)
            pointPB.Location = pointPB.Align(HorizontalAlign.Centre, VerticalAlign.Top);

            // Обработчик события
            picPB.Click += new EventHandler(picPB_Click);
        }

        void picPB_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)// Проверяем нажата ли клавиша Ctrl
            {
                AdminClick?.Invoke(this, EventArgs.Empty);// если да - вызываем событие AdminClick
            }
            else                        // иначе - UserClick
                UserClick?.Invoke(this, EventArgs.Empty);    
        }

        public void Start(object sender, EventArgs e)
        {   // Запускаем событие (ActivControl), в качестве параметра цвет фона
            ActivControl?.Invoke(this, new WorkEventArgs(MainForm.Settings.BackgroundColor));
        }

        private void StartControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            picPB.Click -= new EventHandler(picPB_Click);
        }
    }
}
