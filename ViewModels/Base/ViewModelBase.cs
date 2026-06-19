using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestApp.ViewModels.Base
{
    /// <summary>
    /// Абстрактный базовый класс ViewModel для паттерна MVVM.
    /// Реализует INotifyPropertyChanged для уведомления UI об изменениях свойств.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие, возникающее при изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Установить значение свойства с автоматическим уведомлением UI.
        /// Если значение не изменилось — уведомление не отправляется.
        /// </summary>
        /// <typeparam name="T">Тип свойства.</typeparam>
        /// <param name="field">Ссылка на backing-поле.</param>
        /// <param name="value">Новое значение.</param>
        /// <param name="propertyName">Имя свойства (определяется автоматически).</param>
        /// <returns>True, если значение было изменено; иначе false.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Вызвать событие PropertyChanged для указанного свойства.
        /// </summary>
        /// <param name="propertyName">Имя изменившегося свойства.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
