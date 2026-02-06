using DevIO.Business.Core.Models;

namespace DevIO.AppMvc.Mappings
{
    public interface IMapFrom<TEntity> where TEntity : Entity
    {
        void FromEntity(TEntity entity);
    }
}
