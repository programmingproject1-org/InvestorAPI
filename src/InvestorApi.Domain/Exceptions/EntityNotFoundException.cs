using System;

namespace InvestorApi.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, Guid entityId)
            : base($"{entityName} with id '{entityId}' does not exists.")
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }
    }
}
