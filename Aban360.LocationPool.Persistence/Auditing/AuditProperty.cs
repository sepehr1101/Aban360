using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Aban360.LocationPool.Persistence.Auditing
{
    public class AuditProperty
    {
        public string Name { set; get; }
        public object Value { set; get; }

        public bool IsTemporary { set; get; }
        public PropertyEntry PropertyEntry { set; get; }

        public AuditProperty() { }

        public AuditProperty(string name, object value, bool isTemporary, PropertyEntry property)
        {
            Name = name;
            Value = value;
            IsTemporary = isTemporary;
            PropertyEntry = property;
        }
    }
}
