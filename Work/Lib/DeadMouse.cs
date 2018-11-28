using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Work;

namespace Work.Lib
{
    /// <summary>
    /// Таймер неактивной мыши
    /// </summary>
    public class DeadMouseTimer : IDisposable
    {
        event EventHandler DeadMouse;       // Генерируемое событие
        private Control ctrl;               // Ссылка на контрол запрашивающий событие
        private EventHandler eventHandler;  // Ссылка на генерируемое событие контрола
        Timer timer = new Timer();          // Таймер

        /// <summary>
        /// Таймер неактивной мыши
        /// </summary>
        /// <param name="ctrl">Контрол запрашивающий событие неактивной мыши</param>
        /// <param name="evt">Ссылка на генерируемое событие контрола</param>
        /// <param name="interval">Интервал времени срабатывания в мс.</param>
        public DeadMouseTimer(Control ctrl, EventHandler evt, int interval)
        {
            this.ctrl = ctrl;
            eventHandler = evt;
            // Если интервал нулевой, задаем его равным 1 мс.
            timer.Interval = (interval == 0)? 1: interval;  
            // Подписка на события
            timer.Tick += new EventHandler(timer_Tick); 
            DeadMouse += new EventHandler(evt);     
            ctrl.MouseClick += new MouseEventHandler(ctrl_MouseClick);
            // Запуск таймера
            timer.Start();
        }

        void ctrl_MouseClick(object sender, MouseEventArgs e)
        {   // Если за время работы таймера будет клик мышкой,
            timer.Enabled = false;  // отсчет времени
            timer.Enabled = true;   // начнется с начала.
        }

        void timer_Tick(object sender, EventArgs e)
        {   // Если есть подписка - генерируем событие DeadMouse
            // В аргументы добавляется ссылка на тот контрол, который вызвал событие первого таймаута
            if (DeadMouse != null) DeadMouse(this, new DeadMouseEventArgs() { Sender = this });
            this.Stop();            // Остановка таймера неактивной мыши  
        }

        public void Stop()
        {   // Остановка таймера и отписка от всех событий
            timer.Stop();           
            timer.Tick -= new EventHandler(timer_Tick);
            DeadMouse -= new EventHandler(eventHandler);
            ctrl.MouseClick -= new MouseEventHandler(ctrl_MouseClick); 
        }

        public void Restart() {

            timer.Enabled = false;
            timer.Enabled = true;
        }

        // Реализация интерфейса IDisposable (освобождение ресурсов)
        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    timer.Dispose();
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты)
                // и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing)
        // выше включает код для освобождения неуправляемых ресурсов.
        // ~DeadMouseTimer() {
        //   // Не изменяйте этот код. Разместите код очистки выше, 
        // в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, 
            // в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, 
            // если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
