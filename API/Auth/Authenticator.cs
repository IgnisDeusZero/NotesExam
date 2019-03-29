namespace Notes.API.Auth
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Notes.Models.Users;
    using Notes.Models.Users.Repositories;

    public class Authenticator : IAuthenticator
    {
        private readonly IUserRepository userRepository;
        private readonly ConcurrentDictionary<string, SessionState> sessions;

        public Authenticator(IUserRepository userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }

            this.userRepository = userRepository;
            this.sessions = new ConcurrentDictionary<string, SessionState>();
        }

        public async Task<SessionState> AuthenticateAsync(string login, string password, CancellationToken cancellationToken)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            cancellationToken.ThrowIfCancellationRequested();

            User user = null;
            try
            {
                user = await this.userRepository.GetAsync(login, cancellationToken).ConfigureAwait(false);
            }
            catch (UserNotFoundException)
            {
                throw new AuthenticationException();
            }

            var currentHash = Models.Helper.Hash(password);
            if (!user.PasswordHash.Equals(currentHash))
            {
                throw new AuthenticationException();
            }
            
            var sessionId = Guid.NewGuid().ToString();
            var sessionState = new SessionState(sessionId, user.Id, currentHash);
            while (!this.sessions.TryAdd(sessionId, sessionState))
            {
            }

            return sessionState;
        }

        public Task<SessionState> GetSessionAsync(string sessionId, CancellationToken cancellationToken)
        {
            if (sessionId == null)
            {
                throw new ArgumentNullException(nameof(sessionId));
            }

            if (!this.sessions.TryGetValue(sessionId, out var sessionState))
            {
                throw new AuthenticationException();
            }

            return Task.FromResult(sessionState);
        }
    }
}
