using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;    // Для FileInfo
using System.Threading;

namespace Work.Lib
{
    public partial class Settings : UserControl, IActivControl
    {
        public event EventHandler<WorkEventArgs> ActivControl;

        List<Settings_Box> box = new List<Settings_Box>();  // Номер, Название, Цена
        TextBox tbDeadMouse;                                // Значение DeadMouse
        Button btnColor;

        int n = 4;      // Количество наименований
        int x = 0;      // Координаты по оси X
        int y = 0;      // Координаты по оси Y
        int sp = 40;    // Расстояние между строками

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            y += 60;    // отступ на заголовки

            for (int i = 0; i < n; i++)
            {
                box.Add(new Settings_Box());

                box[i].Number = new Label() 
                { Size = new Size(13, 20), Location = new Point(x, y),
                                Text = (i + 1).ToString() };
                this.Controls.Add(box[i].Number);

                box[i].Name = new TextBox() 
                { Size = new Size(155, 20), Location = new Point(30, y) };
                this.Controls.Add(box[i].Name);

                box[i].Price = new TextBox() 
                { Size = new Size(55, 20), Location = new Point(220, y),
                                    Text = "0"};// Text == значение по умолчанию
                box[i].Price.KeyPress += new KeyPressEventHandler(this.tb_IsDigit);
                this.Controls.Add(box[i].Price);

                box[i].PathToPic = new TextBox()
                { Size = new Size(155, 20), Location = new Point(310, y) };
                this.Controls.Add(box[i].PathToPic);

                box[i].BtnPath = new Button() 
                {
                    Size = new Size(30, 20),
                    Location = new Point(500, y),
                    UseVisualStyleBackColor = true,
                    Text = "...",
                    Tag = i     // Tag == индекс
                };
                // Подписка на обработчик события Click
                box[i].BtnPath.Click += new EventHandler(FileDialog_Click);   
                this.Controls.Add(box[i].BtnPath);

                y += sp;        // Отступ для вывода следующей строки контролов
            }

            Label lbDeadMouse = new Label() 
            { Size = new Size(180, 20), Location = new Point(30, y),
                            Text = "Время ожидания активности мс." };
            this.Controls.Add(lbDeadMouse);

            tbDeadMouse = new TextBox() 
            { Size = new Size(55, 20), Location = new Point(220, y),
                            Text = "15000"};// Text == значение по умолчанию
            tbDeadMouse.KeyPress += new KeyPressEventHandler(this.tb_IsDigit);
            this.Controls.Add(tbDeadMouse);

            Label lbColor = new Label() 
            { Size = new Size(80, 20), Location = new Point(370, y),
                            Text = "Цвет фона:" };
            this.Controls.Add(lbColor);

            btnColor = new Button() 
            { Size = new Size(80, 20), Location = new Point(450, y) };
            // Подписка на обработчик события Click
            btnColor.Click += new EventHandler(btnColor_Click);   
            this.Controls.Add(btnColor);
        }


        public void Start(object sender, EventArgs e)
        {
            if (MainForm.Settings.Product != null && MainForm.Settings.Product.Count != 0)
            {
                int i = 0;
                foreach (Settings_Box b in box)
                {   // Значения продуктов из MainForm.Settings переписываем в поля контрола
                    b.Number.Text = MainForm.Settings.Product[i].Number.ToString();
                    b.Name.Text = MainForm.Settings.Product[i].Name;
                    b.Price.Text = MainForm.Settings.Product[i].Price.ToString();
                    b.PathToPic.Text = MainForm.Settings.Product[i].PathToPic;
                    i++;
                }
                // Инициализация значения поля TimeDeadMouse
                tbDeadMouse.Text = MainForm.Settings.TimeDeadMouse.ToString();
                // Инициализация цвета для кнопки выбора цвета фона
                btnColor.BackColor = MainForm.Settings.BackgroundColor; 
            }
            // Вызываем событие (ActivControl)
            ActivControl(this, new WorkEventArgs(MainForm.Settings.BackgroundColor));
        }

        // Сохранение настроек
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Если информация о продуктах еще не создана
            if (MainForm.Settings.Product.Count == 0)   
            {
                foreach (Settings_Box b in box)// Значения из Settings_Box
                    // добавляем в Settings.Product
                    MainForm.Settings.AddSettings_Box(b);
            }

            else                    // Иначе, заменяем уже существующие значения
            {
                int i = 0;
                foreach (Settings_Box b in box)
                {
                    MainForm.Settings.Product[i].Number = int.Parse(b.Number.Text);
                    MainForm.Settings.Product[i].Name = b.Name.Text;
                    MainForm.Settings.Product[i].Price = double.Parse(b.Price.Text);
                    MainForm.Settings.Product[i].PathToPic = b.PathToPic.Text;
                    ++i;
                }
            }
            // Сохраняем время ожидания активности
            MainForm.Settings.TimeDeadMouse = int.Parse(tbDeadMouse.Text);
            // Сохраняем выбранный цвет фона
            MainForm.Settings.BackgroundColor = btnColor.BackColor;
            //MainForm.Settings = ws;      // Установить ссылку на новый WorkSettings
            MainForm.Settings.Write("Settings.dat");// Сохранить настройки в файл

            // Чтение настроек из файла (для пересоздания контролов)
            MainForm.Settings.Read("Settings.dat");      
            Start(this, new EventArgs());           // Обновить содержимое формы
        }

        // Проверка ввода (цифры, разделитель и Backspace) 8 == Backspace
        private void tb_IsDigit(object sender, KeyPressEventArgs e)
        {
            // Разделитель целой и дробной части берем из региональных настроек
            char sep = Convert.ToChar
                (Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != sep)
            {   // Если (e.Handled == true) событие считается обработанным и 
                e.Handled = true;   // не передается операционной системе!!!
            }
        }

        // Выбор цвета фона
        private void btnColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();// Открываем окно диалога выбора цвета
            colorDlg.Color = btnColor.BackColor;     // Передаем в диалог текущий цвет фона
            if (colorDlg.ShowDialog() == DialogResult.OK)// Если цвет выбран
                btnColor.BackColor = colorDlg.Color; // Красим в выбранный цвет кнопку
        }

        private void FileDialog_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            string workDir = Directory.GetCurrentDirectory();// Рабочая папка программы
            fd.InitialDirectory = workDir;        // Стартовая папка для OpenFileDialog
            fd.Filter = "Файл картинки   *.png|*.png";      // Фильтр
            DialogResult fdResult = fd.ShowDialog();        // Показать OpenFileDialog

            if (fdResult == DialogResult.OK)                // Если файл выбран
            {
                FileInfo fi = new FileInfo(fd.FileName);    // Информация о файле
                string dirName = fi.DirectoryName;// Директория в которой находится файл
                string fName = fi.Name;                     // Имя файла

                // Индекс записи в List<Settings_Box> box
                int index = (int)((Button)sender).Tag; 
                // Если файл в рабочей директории - пишем только имя файла
                // иначе полный путь
                box[index].PathToPic.Text = (dirName == workDir)? fName: fd.FileName;
            }            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {   // Требуем подтверждения закрытия приложения
            DialogResult result = MessageBox.Show("Закрыть приложение?", "Work", 
                                                    MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)                  // Если ответ Yes,
                Application.Exit();                         // закрыть приложение
        }
    }

}
