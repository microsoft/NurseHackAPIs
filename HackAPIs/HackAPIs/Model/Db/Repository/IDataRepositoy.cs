using HackAPIs.Services.Db.Model;
using System.Collections.Generic;

namespace HackAPIs.Model.Db.Repository
{
    public interface IDataRepositoy<TEntity, TDto>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(long id, int type);

        TEntity GetByObject(TEntity entity);

        TEntity GetByColumn(long id, string columnName, string colunmValue);
        TDto GetDto(long id);
        void Add(TEntity entity);
        void Update(TEntity entityToUpdate, TEntity entity, int type);
        void Delete(TEntity entity);
        
    }
}
