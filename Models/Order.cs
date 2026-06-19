using System;

namespace TestApp.Models
{
    /// <summary>
    /// Модель данных, представляющая заказ.
    /// Связывает сотрудника, контрагента, дату и сумму заказа.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Уникальный идентификатор заказа.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Дата оформления заказа.
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Сумма заказа (до 18 цифр, 2 знака после запятой).
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Сотрудник, оформивший заказ.
        /// </summary>
        public virtual Employee Employee { get; set; } = null!;

        /// <summary>
        /// Контрагент, для которого оформлен заказ.
        /// </summary>
        public virtual Counterparty Counterparty { get; set; } = null!;
    }
}
