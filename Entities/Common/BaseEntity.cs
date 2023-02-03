using System;

namespace Entities
{
    public interface IEntity
    {
        public string Created { get; set; }
        public string CreatedByUserId { get; set; }
        public string Modified { get; set; }
        public string ModifiedByUserId { get; set; }



    }

    public abstract class BaseEntity<TKey> : IEntity
    {
        public TKey Id { get; set; }
        public string Created { get; set; } = " ";

        public string CreatedByUserId { get; set; } = " ";

        public string Modified { get; set; } = "";

        public string ModifiedByUserId { get; set; } = " "; 

    }

    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
