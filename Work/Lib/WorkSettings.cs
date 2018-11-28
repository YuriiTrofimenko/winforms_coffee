using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Drawing;

namespace Work.Lib
{
    // Serializable класс
    [Serializable]
    public class Product
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string PathToPic { get; set; }
    }

    // Вспомогательный класс объединяющий вводимые данные о продукте (контрола Settings)
    public class Settings_Box
    {
        public Label Number { get; set; }       // Номер
        public TextBox Name { get; set; }       // Наименование
        public TextBox Price { get; set; }      // Цена
        public TextBox PathToPic { get; set; }  // Путь к файлу картинки
        
        // Кнопка запуска OpenFileDialog, выбора файла картинки
        public Button BtnPath { get; set; }     
    }


    public class WorkSettings
    {
        // Событие удачного завершения чтения настроек
        public event EventHandler WorkSettingsIsRead = delegate { };
        List<Product> product;  // Список продуктов типа Product
        int timeDeadMouse;      // Допустимое время отсутствия активности в мс.
        Color backgroundColor;  // Цвет фона формы MainForm

        public List<Product> Product
        {
            set { product = value; }
            get { return product; }
        }
        public int TimeDeadMouse 
        {
            set { timeDeadMouse = value; }
            get { return timeDeadMouse; }
        }
        public Color BackgroundColor
        {
            set { backgroundColor = value; }
            get { return backgroundColor; }
        }

        // Конструктор без параметров
        public WorkSettings()
        {
            product = new List<Product>();
        }

        // Конструктор принимающий в качестве параметра путь к файлу с данными
        public WorkSettings(string path)
        {
            Read(path);
        }

        /// <summary>
        /// Добавляет в конец элемент типа Product преобразуя данные из Settings_Box
        /// </summary>
        /// <param name="box">Данные типа Settings_Box</param>
        public void AddSettings_Box(Settings_Box box)
        {
            product.Add(new Product()
            {
                Number = int.Parse(box.Number.Text),
                Name = box.Name.Text,
                Price = double.Parse(box.Price.Text),
                PathToPic = box.PathToPic.Text
            });
        }

        /// <summary>
        /// Читает из файла, с использованием BinaryFormatter, данные WorkSettings
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void Read(string path)
        {
            BinaryFormatter binFormat = new BinaryFormatter();// Используем BinaryFormatter
            try
            {
                using (Stream fStream = new FileStream(path,    // Открытие файла
                        FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // Чтение List<Product> product
                    product = (List<Product>)binFormat.Deserialize(fStream);
                    // Чтение int timeDeadMouse
                    timeDeadMouse = (int)binFormat.Deserialize(fStream);
                    // Чтение Color backgroundColor
                    backgroundColor = (Color)binFormat.Deserialize(fStream);
                }
                // Вызов события удачного завершения чтения
                WorkSettingsIsRead(this, EventArgs.Empty);
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Ошибка чтения данных из файла!", "Чтение настроек");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Пишет в файл, с использованием BinaryFormatter, данные WorkSettings
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void Write(string path)
        {
            BinaryFormatter binFormat = new BinaryFormatter();// Используем BinaryFormatter
            try
            {
                using (Stream fStream = new FileStream(path,    // Открытие файла
                        FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    binFormat.Serialize(fStream, product);// Запись List<Product> product
                    binFormat.Serialize(fStream, timeDeadMouse);// Запись int timeDeadMouse
                    // Запись Color backgroundColor
                    binFormat.Serialize(fStream, backgroundColor);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка записи данных в файл!", "Запись настроек");
            }
        }
    }
}
