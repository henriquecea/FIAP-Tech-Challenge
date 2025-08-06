using FCG.Domain.Entity;
using System.Linq.Expressions;

namespace FCG.Domain.Queries;

public static class UserQueries
{
    // Buscar usuário por ID
    public static Expression<Func<UserEntity, bool>> ById(Guid id)
        => u => u.Id == id;

    // Buscar usuário por e-mail (ignora maiúsculas/minúsculas)
    public static Expression<Func<UserEntity, bool>> ByEmail(string email)
        => u => u.EmailAddress.Address.Equals(email, StringComparison.CurrentCultureIgnoreCase);

    // Buscar usuário ativo
    public static Expression<Func<UserEntity, bool>> IsActive()
        => u => u.IsDeleted;

    // Buscar usuários criados depois de uma certa data
    public static Expression<Func<UserEntity, bool>> CreatedAfter(DateTime date)
        => u => u.CreatedAt > date;
}
