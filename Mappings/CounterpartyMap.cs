using FluentNHibernate.Mapping;
using TestApp.Models;

namespace TestApp.Mappings
{
    /// <summary>
    /// Маппинг NHibernate для модели Counterparty.
    /// Определяет отображение свойств контрагента на таблицу «Counterparties».
    /// </summary>
    public class CounterpartyMap : ClassMap<Counterparty>
    {
        public CounterpartyMap()
        {
            Table("Counterparties");

            // Первичный ключ с автогенерацией через Identity
            Id(x => x.Id).GeneratedBy.Identity();

            // Наименование контрагента — строка до 255 символов, обязательное
            Map(x => x.Name).Length(255).Not.Nullable();

            // ИНН — строка до 12 символов, обязательное поле
            Map(x => x.Inn).Length(12).Not.Nullable();

            // Внешний ключ на куратора (сотрудника)
            References(x => x.Curator).Column("CuratorId").Not.Nullable();
        }
    }
}
