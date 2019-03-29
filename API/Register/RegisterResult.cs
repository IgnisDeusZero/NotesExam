namespace Notes.API.Register
{
    using System;

    public class RegisterResult
    {
        public RegisterResult(string login, DateTime registeredAt)
        {
            this.Login = login;
            this.RegisteredAt = registeredAt;
        }

        public string Login { get; }

        public DateTime RegisteredAt { get; }
    }
}
