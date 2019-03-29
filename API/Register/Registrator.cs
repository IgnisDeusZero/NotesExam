namespace Notes.API.Register
{
    using System;
    using System.Threading;
    using Models.Users;
    using Models.Users.Repositories;

    public class Registrator : IRegistrator
    {
        private readonly IUserRepository userRepository;

        public Registrator(IUserRepository userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }
            this.userRepository = userRepository;
        }

        public RegisterResult Register(string login, string password)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            var hash = Models.Helper.Hash(password);
            var creationInfo = new UserCreationInfo(login, hash);
            CancellationToken cancellationToken;
            User user = null;
            try
            {
                user = this.userRepository.CreateAsync(creationInfo, cancellationToken).Result;
            }
            catch (UserDuplicationException)
            {
                throw new RegistrationException();
            }
            return new RegisterResult(user.Login, user.RegisteredAt);
        }
    }
}
