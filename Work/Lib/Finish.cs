using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Printing;

namespace Work.Lib
{
    public partial class Finish : UserControl, IActivControl, IDeadMouse
    {
        public event EventHandler<WorkEventArgs> ActivControl = delegate { };
        public event EventHandler DeadMouse = delegate { };
        DeadMouseTimer dmTimer;
        bool printed;           // Флаг, показывающий печатался чек или нет

        public Finish()
        {
            InitializeComponent();
        }

        public void Start(object sender, EventArgs e)
        {
            printed = false;    // false - чек не напечатан

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

        private void btnPrintCheck_Click(object sender, EventArgs e)
        {
            if (printed) return;// Прерываем выполнение метода если чек уже напечатан
            string defPrinter = string.Empty;            // Имя принтера по умолчанию

            // Перебираем все имеющиеся в системе принтеры
            foreach (string printerName in PrinterSettings.InstalledPrinters)
                // Если имя установлено по умолчанию
                if (new PrinterSettings() { PrinterName = printerName }.IsDefaultPrinter)
                    defPrinter = printerName;              // Сохраняем имя в defPrinter



            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrinterSettings.PrinterName = defPrinter;

                // Для отладки внешнего вида
                //PrintPreviewDialog ppd = new PrintPreviewDialog();
                //ppd.Document = pd;
                //ppd.ShowDialog();

                pd.Print();
                printed = true;                  // Чек напечатан
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            DateTime dateTime = DateTime.Now;    // Текущая дата и время
            // Текущая дата и время в виде строки
            string dt = dateTime.ToString("dd.MM.yyyy HH:mm");       
            int index = MainForm.SelectedNumber - 1;    // Индекс выбранного продукта
            // Название
            string prName = string.Format("\"{0}\"", 
                                        MainForm.Settings.Product[index].Name);
            double prPrice = MainForm.Settings.Product[index].Price;  // Цена

            // Картинка из ресурсов, координаты 0|0, размер 50|50
            e.Graphics.DrawImage(Work.Properties.Resources.impresszum, 0, 0, 50, 50);

            string printText1 = string.Empty;    // Текст чека 1
            string printText2 = string.Empty;    // Текст чека 2
            string printText3 = string.Empty;    // Текст чека 3

            printText1 += string.Format("\r\n"); // Отступ
            printText1 += string.Format("\r\n"); // Отступ
            printText1 += string.Format("                    кофе машина\r\n");
            printText1 += string.Format("                      № 00001\r\n");
            printText1 += string.Format
                                   ("----------------------------------------------\r\n");
            printText1 += string.Format("\r\n");
            printText1 += string.Format(" Кофе   {0,13}        {1:##0.00}\r\n",
                                                    prName, prPrice);
            printText1 += string.Format("\r\n");
            printText1 += string.Format
                                    ("----------------------------------------------\r\n");

            printText2 += string.Format("\r\n"); // Отступ
            printText2 += string.Format("  Сумма     {0:##0.00}\r\n", 
                                            MainForm.Settings.Product[index].Price);
            printText2 += string.Format("\r\n"); // Отступ

            printText3 += string.Format("Фискальный чек     {0}\r\n", dt);
            printText3 += string.Format("          Спасибо за покупку!\r\n");



            // Формирование изображения
            Font PrintFont1 = new Font("Times New Roman", 3, FontStyle.Regular, 
                                                      GraphicsUnit.Millimeter);// Шрифт1
            e.Graphics.DrawString(printText1, PrintFont1, Brushes.Black, new PointF(0, 0));
            Font PrintFont2 = new Font("Times New Roman", 5, FontStyle.Regular, 
                                                      GraphicsUnit.Millimeter);// Шрифт2
            e.Graphics.DrawString(printText2, PrintFont2, Brushes.Black, 
                                    new PointF(0, 100));
            Font PrintFont3 = new Font("Times New Roman", 3, FontStyle.Regular,
                                                      GraphicsUnit.Millimeter);// Шрифт3
            e.Graphics.DrawString(printText3, PrintFont3, Brushes.Black, 
                                    new PointF(0, 160));         
        }
    }
}
