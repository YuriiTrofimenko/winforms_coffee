using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Work.Lib
{
    /// <summary>
    /// Предоставляет возможность автоматической подписки на событие ActivControl,
    /// которое обеспечивает активирование контрола на главной форме
    /// </summary>
    public interface IActivControl
    {
        event EventHandler<WorkEventArgs> ActivControl;

        /// <summary>
        /// Настраивает контрол перед запуском и
        /// вызывает событие ActivControl
        /// </summary>
        void Start(object sender, EventArgs e);
    }

    /// <summary>
    /// Предоставляет возможность автоматической подписки на событие DeadMouse,
    /// и определяет работу таймера DeadMouse на реализующем контроле
    /// </summary>
    public interface IDeadMouse
    {
        event EventHandler DeadMouse;

        /// <summary>
        /// Запускает таймер DeadMouse
        /// </summary>
        /// <param name="ctrl">Контрол запрашивающий событие 
        /// неактивной мыши DeadMouse</param>
        /// <param name="evt">Генерируемое событие контрола</param>
        /// <param name="interval">Время до наступления события в мс.</param>
        void StartDeadMouseTimer(Control ctrl, EventHandler evt, int interval);
    }

    /// <summary>
    /// Предоставляет возможность автоматической подписки контрола на событие
    /// WorkSettingsIsRead, для выполнения метода SettingsIsRead 
    /// при наступлении этого события
    /// </summary>
    public interface ISettingsIsRead
    {
        /// <summary>
        /// Обработчик события WorkSettingsIsRead
        /// </summary>
        void SettingsIsRead(object sender, EventArgs e);
    }
}
