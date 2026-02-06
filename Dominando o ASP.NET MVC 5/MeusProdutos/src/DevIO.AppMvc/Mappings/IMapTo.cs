namespace DevIO.AppMvc.Mappings
{
    public interface IMapTo<TEntity>
    {
        TEntity ToEntity();
    }
}
