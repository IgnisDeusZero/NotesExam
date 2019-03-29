namespace Notes.Models.Users.Repositories
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс, описывающий хранилище пользователей
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="creationInfo">Данные для создания нового пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданный пользователь</returns>
        Task<User> CreateAsync(UserCreationInfo creationInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Пользователь</returns>
        Task<User> GetAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получить пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Пользователь</returns>
        Task<User> GetAsync(string login, CancellationToken cancellationToken);
    }
}
