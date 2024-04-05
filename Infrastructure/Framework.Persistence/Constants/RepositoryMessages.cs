using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Persistence.Constants;

public static class RepositoryMessages
{
    public static string OneToOneRelationAttentionWarning = "Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again by same foreign key.";
    public static string IQueryProviderNotFoundThis = "CreateQuery<TElement> method is not found in IQueryProvider.";
}
