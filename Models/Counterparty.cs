namespace TestApp.Models
{
    /// <summary>
    /// Модель данных, представляющая контрагента (юрлицо или ИП).
    /// Каждый контрагент привязан к одному куратору-сотруднику.
    /// </summary>
    public class Counterparty
    {
        /// <summary>
        /// Уникальный идентификатор контрагента.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Наименование контрагента.
        /// </summary>
        public virtual string Name { get; set; } = string.Empty;

        /// <summary>
        /// ИНН контрагента (строка до 12 символов).
        /// </summary>
        public virtual string Inn { get; set; } = string.Empty;

        /// <summary>
        /// Сотрудник-куратор, ответственный за данного контрагента.
        /// </summary>
        public virtual Employee Curator { get; set; } = null!;
    }
}
