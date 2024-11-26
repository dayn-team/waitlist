using Core.Application.Errors;
using Core.Shared;

namespace Core.Domain.Entities {
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            id = Cryptography.CharGenerator.genID(12, Cryptography.CharGenerator.characterSet.HEX_STRING);
        }
        public string id { get; protected set; }

        public void cannotBeNull(params string[] objs)
        {
            foreach (string data in objs)
                if (data is null)
                    throw new InputError("One or more input is missing!");
        }
        public void cannotBeNullOrEmpty(params string[] objs)
        {
            foreach (string data in objs)
                if (string.IsNullOrEmpty(data))
                    throw new InputError("One or more input is missing or empty!");
        }
    }
}
