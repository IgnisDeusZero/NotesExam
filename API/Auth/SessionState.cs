namespace Notes.API.Auth
{
    using System;

    public class SessionState
    {
        public SessionState(string sessionId, Guid userId, string passwordHash)
        {
            if (sessionId == null)
            {
                throw new ArgumentNullException(nameof(sessionId));
            }

            this.SessionId = sessionId;
            this.UserId = userId;
            this.PasswordHash = passwordHash;
        }

        public string SessionId { get; }

        public Guid UserId { get; }

        public string PasswordHash { get; }
    }
}
