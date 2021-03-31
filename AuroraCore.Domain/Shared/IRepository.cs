using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Shared
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T FindByID(Guid id);

        void Store(T entity);

        void Update(T entity);

        int Delete(Guid id);
    }
}
