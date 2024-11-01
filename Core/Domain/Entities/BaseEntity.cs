using Core.Application.Errors;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Domain.Entities
{
    public abstract class BaseEntity
    {

        public string? id { get; protected set; }

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
