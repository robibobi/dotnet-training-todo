using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApplication.Util;

namespace TodoApplication.Respositories
{
    internal interface IRepository<TEntity>
    {
        Task<Result<List<TEntity>>> GetAll();
        Task<Result<Unit>> Add(TEntity entity);
        Task<Result<Unit>> Remove(Guid entityId);
        Task<Result<Unit>> Update(TEntity entity);
    }
}
