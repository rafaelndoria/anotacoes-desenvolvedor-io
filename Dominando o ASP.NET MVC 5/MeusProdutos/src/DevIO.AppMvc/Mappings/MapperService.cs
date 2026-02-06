using DevIO.Business.Core.Models;

namespace DevIO.AppMvc.Mappings
{
    public class MapperService : IMapperService
    {
        public TEntity ToEntity<TEntity>(IMapTo<TEntity> source) where TEntity : Entity
        {
            return source.ToEntity();
        }

        public TViewModel ToViewModel<TEntity, TViewModel>(TEntity entity)
            where TEntity : Entity
            where TViewModel : IMapFrom<TEntity>, new()
        {
            var vm = new TViewModel();
            vm.FromEntity(entity);
            return vm;
        }
    }
}