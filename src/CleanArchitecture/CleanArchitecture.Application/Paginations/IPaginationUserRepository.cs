using System.Linq.Expressions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore.Query;

namespace CleanArchitecture.Application.Paginations;
public interface IPaginationUserRepository
{
    Task<PagedResults<User, UserId>> GetPaginationAsync(
        Expression<Func<User, bool>>? predicate,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? includes,
        int page,
        int pageSize,
        string OrderBy,
        bool ascending,
        bool disableTracking = true
    );
}
