using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;

namespace Work.Lib
{
    public enum HorizontalAlign { Left, Centre, Right }
    public enum VerticalAlign { Top, Centre, Bottom }

    public static class Extension
    {
        /// <summary>
        /// Выравнивание контрола, внутри родительского окна или контрола, 
        /// по заданным параметрам
        /// </summary>
        /// <param name="control"></param>
        /// <param name="h">Горизонтальное выравнивание</param>
        /// <param name="v">Вертикальное выравнивание</param>
        /// <returns></returns>
        public static Point Align(this Control control, HorizontalAlign h, VerticalAlign v)
        {
            int x;                              // Координата верхнего угла по оси (x)
            int y;                              // Координата верхнего угла по оси (y)
            Control parent = control.Parent;    // Владелец дочернего контрола

            switch (h)                          // Выбор значения для координаты (x)
            { 
                case HorizontalAlign.Left:      // выравнивание по левому краю
                    x = 0;
                    break;
                case HorizontalAlign.Right:     // выравнивание по правому краю
                    x = parent.ClientSize.Width - control.Size.Width;
                    break;
                default:                           // выравнивание по центру
                    x = parent.ClientSize.Width / 2 - control.Size.Width / 2;
                    break;
            }


            switch (v)                          // Выбор значения для координаты (y)
            {
                case VerticalAlign.Top:         // выравнивание по верхнему краю
                    y = 0;
                    break;
                case VerticalAlign.Bottom:      // выравнивание по нижнему краю
                    y = parent.ClientSize.Height - control.Size.Height;
                    break;
                default:                        // выравнивание по центру
                    y = parent.ClientSize.Height / 2 - control.Size.Height / 2;
                    break;
            }

            return new Point(x, y);
        }
    }
}
