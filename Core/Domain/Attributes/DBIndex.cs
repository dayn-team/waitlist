namespace Core.Domain.Attributes {
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class DBIndex : Attribute {
        public IndexAttributes indexAttribute { get; private set; }

        public DBIndex(IndexAttributes attribute = IndexAttributes.NONE) {
            indexAttribute = attribute;
        }
    }

    public enum IndexAttributes {
        UNIQUE = 1,
        FULL_TEXT_INDEX = 2,
        NONE = 0,
        UNIQUE_AND_PRIMARY = 3
    }
}