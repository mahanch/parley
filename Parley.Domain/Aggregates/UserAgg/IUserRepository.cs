using Parley.Domain._Shared;
using Parley.Domain.Aggregates.UserAgg.Entities;

namespace Parley.Domain.Aggregates.UserAgg;

public interface IUserRepository:IRepository<User,Guid>
{
    
}