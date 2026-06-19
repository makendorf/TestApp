namespace TestApp.Models
{
    public class Counterparty
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; } = string.Empty;
        public virtual string Inn { get; set; } = string.Empty;
        public virtual Employee Curator { get; set; } = null!;
    }
}
