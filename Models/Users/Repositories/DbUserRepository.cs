using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Configuration;

namespace Notes.Models.Users.Repositories
{
    public sealed class DbUserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> users;

        public DbUserRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("NotesDb");
            users = database.GetCollection<User>("Users");
        }

        public Task<User> CreateAsync(UserCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (this.users.Find(x => x.Login == creationInfo.Login).Any())
            {
                throw new UserDuplicationException(creationInfo.Login);
            }

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var user = new User
            {
                Id = id,
                Login = creationInfo.Login,
                PasswordHash = creationInfo.PasswodHash,
                RegisteredAt = now
            };

            users.InsertOne(user);

            return Task.FromResult(user);
        }

        public Task<User> GetAsync(Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var searchResult = this.users.Find(x => x.Id == userId);

            if (!searchResult.Any())
            {
                throw new UserNotFoundException(userId);
            }

            return Task.FromResult(searchResult.First());
        }

        public Task<User> GetAsync(string login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var searchResult = this.users.Find(x => x.Login == login);

            if (!searchResult.Any())
            {
                throw new UserNotFoundException(login);
            }

            return Task.FromResult(searchResult.First());
        }
    }
}
