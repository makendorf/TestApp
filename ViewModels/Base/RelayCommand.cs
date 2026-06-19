using System;
using System.Windows.Input;

namespace TestApp.ViewModels.Base
{
    /// <summary>
    /// Реализация ICommand для привязки команд в MVVM.
    /// Позволяет привязать Action (выполнение) и опциональный Func (проверка доступности).
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Делегат выполнения команды (принимает параметр command).
        /// </summary>
        private readonly Action<object?> _execute;

        /// <summary>
        /// Делегат проверки доступности команды (возвращает true, если команда доступна).
        /// </summary>
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Создать команду с делегатом, принимающим параметр.
        /// </summary>
        /// <param name="execute">Действие при выполнении команды.</param>
        /// <param name="canExecute">Проверка доступности команды (null = всегда доступна).</param>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Создать команду без параметра (удобная обёртка).
        /// </summary>
        /// <param name="execute">Действие при выполнении команды.</param>
        /// <param name="canExecute">Проверка доступности команды (null = всегда доступна).</param>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
            : this(_ => execute(), canExecute is not null ? _ => canExecute() : null)
        {
        }

        /// <summary>
        /// Событие изменения доступности команды.
        /// Привязано к CommandManager.RequerySuggested WPF для автоматического
        /// пересмотра доступности команд при изменении UI.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Проверить, доступна ли команда для выполнения.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>True, если команда доступна.</returns>
        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object? parameter) => _execute(parameter);
    }
}
