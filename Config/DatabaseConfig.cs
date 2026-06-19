namespace TestApp.Config
{
    /// <summary>
    /// Класс конфигурации базы данных.
    /// Хранит строку подключения, загружаемую из appsettings.json.
    /// </summary>
    public class DatabaseConfig
    {
        /// <summary>
        /// Строка подключения к базе данных MySQL.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;
    }
}
