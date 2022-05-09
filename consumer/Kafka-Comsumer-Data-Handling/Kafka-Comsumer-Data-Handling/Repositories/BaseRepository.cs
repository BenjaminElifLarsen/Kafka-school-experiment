using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kafka_Comsumer_Data_Handling.Repositories;

internal class BaseRepository<TEntity> where TEntity : class
{
    protected readonly HouseContext _context;
    private readonly DbSet<TEntity> _entities;

    public void Add(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public TEntity FetchSingleWithRelations(Expression<Func<TEntity, object>>[] includes, Expression<Func<TEntity, bool>>[] predicates)
    {
        throw new NotImplementedException();
    }

    public void Update(TEntity entity)
    {

    }
}
