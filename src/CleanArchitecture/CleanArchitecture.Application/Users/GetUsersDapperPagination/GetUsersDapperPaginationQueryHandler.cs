using System.Runtime.InteropServices;
using System.Text;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using Dapper;

namespace CleanArchitecture.Application.Users.GetUsersDapperPagination;

internal sealed class GetUsersDapperPaginationQueryHandler
: IQueryHandler<GetUsersDapperPaginationQuery, PagedDapperResults<UserPaginationData>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUsersDapperPaginationQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<PagedDapperResults<UserPaginationData>>> Handle(GetUsersDapperPaginationQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var builder = new StringBuilder(""" 
            SELECT
                usr.email,
                rl.name as role,
                p.name as permiso
            FROM users usr
            LEFT JOIN users_roles usrl ON usr.id = usrl.user_id
            LEFT JOIN roles rl ON usrl.role_id = rl.id
            LEFT JOIN roles_permisos rp ON rl.id = rp.role_id
            LEFT JOIN permisos p ON rp.permission_id = p.id
        """);

        var search = string.Empty;
        var whereStatement = string.Empty;


        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            search = "%" + EncodeFOrLike(request.Search) + "%";
            whereStatement = " WHERE rl.name LIKE @Search ";
            builder.AppendLine(whereStatement);
        }

        var orderBy = request.OrderBy;
        if(!string.IsNullOrWhiteSpace(orderBy))
        {
            var orderStatement = string.Empty;
            var orderAsc = request.OrderAsc ? "ASC" : "DESC";
            orderStatement = orderBy switch
            {
                "email" => $" ORDER BY usr.email {orderAsc}",
                "role" => $" ORDER BY rl.name {orderAsc}",
                _ => $" ORDER BY rl.name {orderAsc}",
            };

            builder.AppendLine(orderStatement);
        }

        builder.AppendLine(" LIMIT @PageSize OFFSET @Offset;");

        builder.AppendLine("""
            SELECT COUNT(*)
            FROM users usr
            LEFT JOIN users_roles usrl ON usr.id = usrl.user_id
            LEFT JOIN roles rl ON usrl.role_id = rl.id
            LEFT JOIN roles_permisos rp ON rl.id = rp.role_id
            LEFT JOIN permisos p ON rp.permission_id = p.id
        """);

        builder.AppendLine(whereStatement);
        builder.AppendLine(";");

        using var multi = await connection.QueryMultipleAsync(
            builder.ToString(), 
            new { 
                PageSize = request.PageSize, 
                Offset = (request.PageNumber - 1) * request.PageSize, 
                Search = search 
            }
        );

        var items = await multi.ReadAsync<UserPaginationData>().ConfigureAwait(false);
        var totalItems = await multi.ReadFirstAsync<int>().ConfigureAwait(false);

        var result = new PagedDapperResults<UserPaginationData>(totalItems, request.PageNumber, request.PageSize)
        {
            Items = items
        };

        return result;

    }

    private string EncodeFOrLike(string search)
    {
        return search.Replace("[", "[]]")
            .Replace("%", "[%]");
    }
}