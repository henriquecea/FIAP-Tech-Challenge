using FCG.Domain.Entity;
using FCG.Domain.Interface.Repository;
using FCG.Domain.Queries;

namespace FCG.Test.Repository;

public class FakeUserRepository : IUserRepository
{
    private readonly List<UserEntity> _users = [];

    public Task AddAsync(UserEntity entity)
    {
        _users.Add(entity);
        return Task.CompletedTask;
    }

    public void Delete(UserEntity entity)
    {
        _users.Remove(entity);
    }

    public Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<UserEntity>>(_users);
    }

    public Task<UserEntity?> GetAllUser(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<UserEntity?> GetByEmailAsync(string email)
    {
        var predicate = UserQueries.ByEmail(email).Compile();

        var user = _users.FirstOrDefault(predicate);
        return Task.FromResult(user);
    }

    public Task<UserEntity> GetByIdAsync(Guid id)
    {
        var user = _users.First(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task SaveChangesAsync()
        => Task.CompletedTask;

    public void Update(UserEntity entity)
    {
        var existing = _users.FirstOrDefault(u => u.Id == entity.Id);
        if (existing != null)
        {
            _users.Remove(existing);
            _users.Add(entity);
        }
    }
}
