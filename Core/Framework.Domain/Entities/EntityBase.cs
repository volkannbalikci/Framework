using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Entities;

public class EntityBase<TEntityId> : IEntityTimestamps
{
    public TEntityId Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    public EntityBase()
    {
        CreatedDate = DateTime.UtcNow;
    }
}
