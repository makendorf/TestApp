using System;

namespace TestApp.Models
{
    public class Order
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual Employee Employee { get; set; } = null!;
        public virtual Counterparty Counterparty { get; set; } = null!;
    }
}
