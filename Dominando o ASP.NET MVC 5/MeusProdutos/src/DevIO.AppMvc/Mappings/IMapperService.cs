using DevIO.Business.Core.Models;

namespace DevIO.AppMvc.Mappings
{
    public interface IMapperService
    {
        TEntity ToEntity<TEntity>(IMapTo<TEntity> source) where TEntity : Entity;
        TViewModel ToViewModel<TEntity, TViewModel>(TEntity entity)
            where TEntity : Entity
            where TViewModel : IMapFrom<TEntity>, new();
    }
}