using HackAPIs.Db.Model;
using System.Collections.Generic;

namespace HackAPIs.Model.Db.Repository
{
    public interface IDataRepositoy<TEntity, TDto>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(long id, ExtendedDataType extendedData);

        TEntity GetByObject(TEntity entity);

        TEntity GetByColumn(long id, string columnName, string columnValue);
        TDto GetDto(long id);
        void Add(TEntity entity);
        void Update(TEntity entityToUpdate, TEntity entity, ExtendedDataType extendedData);
        void Delete(TEntity entity);
        
    }
}
