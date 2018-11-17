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
    public partial class CustomSelect : UserControl, IActivControl, 
                                        IDeadMouse, ISettingsIsRead
    {
        public event EventHandler<WorkEventArgs> ActivControl = delegate { };
        public event EventHandler DeadMouse = delegate { };
        public event EventHandler ProductSelected = delegate { };
        DeadMouseTimer dmTimer;

        public CustomSelect()
        {
            InitializeComponent();     
        }

        // Настройка и запуск контрола
        public void Start(object sender, EventArgs e)
        {
            MainForm.SelectedNumber = 0;                // Обнуляем выбор продукта

            foreach (Object ctrl in this.Controls)
            {   // Для всех PictureBox
                PictureBox pb = ctrl as PictureBox;
                if(pb != null)
                    pb.BorderStyle = BorderStyle.None;  // Убираем границы
            }

            // Запуск таймера DeadMouse
            StartDeadMouseTimer(this, DeadMouse, MainForm.Settings.TimeDeadMouse);
            // Запускаем событие (ActivControl)
            ActivControl(this, new WorkEventArgs(MainForm.Settings.BackgroundColor));
        }

        // Выполняется после удачного окончания чтения файла настроек
        public void SettingsIsRead(object sender, EventArgs e)
        {
            // Удаляем все имеющиеся элементы контрола (очистка)
            this.Controls.Clear();

            // Количество элементов в коллекции продуктов
            int n = MainForm.Settings.Product.Count;
            // Резервируем память под (n) элементов PictureBox
            PictureBox[] picBox = new PictureBox[n];
            // Резервируем память под (n) элементов Label   
            Label[] lbBox = new Label[n];

            int nx = 2;     // Количество PictureBox в одной строке
            int row = 0;    // Строка размещения PictureBox
            int xs = 220;   // Реальный размер PictureBox по оси X
            int ys = 220;   // Реальный размер PictureBox по оси Y
            int picSize = 260;  // Размер стороны PictureBox с учетом отступов
            int indent = 10;// Отступ PictureBox от кнопок управления контрола
            int bH = 60;  // Высота кнопок управления

            foreach (Product p in MainForm.Settings.Product)
            {
                int i = p.Number - 1;   // Индекс
                row = i / 2;            // Строка размещения PictureBox
                int col = i - row * nx; // Столбец размещения PictureBox
                picBox[i] = new PictureBox()
                {
                    Size = new Size(xs, ys),
                    Location = new Point(col * picSize, row * picSize),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    Image = Image.FromFile(MainForm.Settings.Product[i].PathToPic),
                    Tag = p.Number      // Номер отображаемого продукта
                };

                lbBox[i] = new Label()
                {
                    Size = new Size(picSize, picSize - ys),
                    Location = new Point(col * picSize, row * picSize + ys),
                    Font = new Font("Arial", 16, FontStyle.Italic),
                    Text = string.Format("{0}   {1}грн.", 
                                            MainForm.Settings.Product[i].Name, 
                                            MainForm.Settings.Product[i].Price)
                };
                this.Controls.Add(lbBox[i]);

                // Перемещение мыши по элементу PictureBox
                picBox[i].MouseMove += new MouseEventHandler(Mouse_Move);
                // - // - по контролу  
                this.MouseMove += new MouseEventHandler(Mouse_Move);
                // - // - по родительской форме MainForm   
                this.Parent.MouseMove += new MouseEventHandler(Mouse_Move);
                // Клик мыши по PictureBox
                picBox[i].MouseClick += new MouseEventHandler(Mouse_Click); 

                this.Controls.Add(picBox[i]);   // Добавляем PictureBox на контрол

            }

            // Создаем кнопку для получения фокуса
            Button btnFocus = new Button()                
            { Size = new Size(0, 0), Location = new Point(0, 0)};
            this.Controls.Add(btnFocus);

            // Создаем кнопку (Продолжить)
            Button btnGo = new Button()                     
            {
                // Положение кнопки
                Location = new Point(picSize * nx / 2, picSize * (row + 1) + indent),  
                Size = new Size(picSize * nx / 2, bH),// Размер
                BackColor = MainForm.Settings.BackgroundColor,// Цвет кнопки == цвет фона
                Text = "Оплата"
            };
            this.Controls.Add(btnGo);                       // Добавляем кнопку на контрол
            // Задаем обработчик события btnGo.Click
            btnGo.Click += new EventHandler(btnGo_Click);   

            Button btnCancel = new Button()                 // Создаем кнопку (Отмена)
            {
                Location = new Point(0, picSize * (row + 1) + indent),// Положение кнопки
                Size = new Size(picSize * nx / 2, bH),      // Размер
                BackColor = MainForm.Settings.BackgroundColor,// Цвет кнопки == цвет фона
                Text = "Отмена"                             // Надпись          
            };
            this.Controls.Add(btnCancel);                   // Добавляем кнопку на контрол
            // Клик по кнопке (Отмена) - обработчик
            btnCancel.Click += new EventHandler(btnCancel_Click); 

            // Устанавливаем размер контрола в зависимости от содержимого
            this.Size = new Size(picSize * nx, picSize * (row + 1) + indent + bH);
            // Выравниваем контрол по центру
            this.Location = this.Align(HorizontalAlign.Centre, VerticalAlign.Centre);
        }

        /// <summary>
        /// Запуск таймера DeadMouse
        /// </summary>
        /// <param name="ctrl">Контрол запрашивающий событие неактивной мыши</param>
        /// <param name="evt">Ссылка на генерируемое событие контрола</param>
        /// <param name="interval">Время до наступления события в мс.</param>
        public void StartDeadMouseTimer(Control ctrl, EventHandler evt, int interval)
        {
            dmTimer = new DeadMouseTimer(ctrl, evt, interval);
        }


        // Контроль перемещения мыши
        private void Mouse_Move(object sender, MouseEventArgs e)
        {
            foreach (Object ctrl in this.Controls)
            {
                PictureBox pb = ctrl as PictureBox;
                if (pb != null) // Если мышь перемещается на PictureBox
                {
                    if (pb == sender && pb.BorderStyle != BorderStyle.Fixed3D)
                        pb.BorderStyle = BorderStyle.FixedSingle;
                    else if(pb != sender && pb.BorderStyle != BorderStyle.Fixed3D)
                        pb.BorderStyle = BorderStyle.None;

                }
            }
        }


        private void Mouse_Click(object sender, MouseEventArgs e)
        {
            foreach (Object ctrl in this.Controls)
            {
                PictureBox pb = ctrl as PictureBox;
                if (pb != null) // Если клик на PictureBox
                {
                    // Вызов события контрола, MouseClick для перезапуска таймера DeadMouse
                    if (pb == sender) OnMouseClick(new MouseEventArgs(e.Button, e.Clicks, 
                                                                e.X, e.Y, e.Delta));

                    if (pb == sender && pb.BorderStyle != BorderStyle.Fixed3D)

                        // Визуальная имитация нажатия
                        pb.BorderStyle = BorderStyle.Fixed3D;   
                    else if (pb != sender)
                        pb.BorderStyle = BorderStyle.None;
                }
            }
        }

        // Обработчик клика кнопки (Отмена)
        private void btnCancel_Click(object sender, EventArgs e)
        {
            dmTimer.Stop();     // Прекращение работы таймера DeadMouse
            dmTimer.Dispose();  // Освобождение ресурсов
            DeadMouse(this, new EventArgs());   // Возврат на стартовый контрол,
                                                // путем вызова события DeadMouse
        }

        // Обработчик клика кнопки (Оплата)
        private void btnGo_Click(object sender, EventArgs e)
        {
            dmTimer.Stop();     // Прекращение работы таймера DeadMouse
            dmTimer.Dispose();  // Освобождение ресурсов

            foreach (Object ctrl in this.Controls)
            {
                PictureBox pb = ctrl as PictureBox;
                if (pb != null) // Если PictureBox
                    if (pb.BorderStyle == BorderStyle.Fixed3D)  // Если выбор был сделан
                        // Сохраняем номер выбранного продукта
                        MainForm.SelectedNumber = (int)pb.Tag;  
            }

            if (MainForm.SelectedNumber != 0) // Если номер выбранного продукта сохранен
                // вызываем событие ProductSelected
                ProductSelected(this, new EventArgs());
            else
                MessageBox.Show("Сделайте свой выбор", "Выбор");
        }
    }
}
