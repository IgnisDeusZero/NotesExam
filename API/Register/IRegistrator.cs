namespace Notes.API.Register
{
    using Models.Users;

    public interface IRegistrator
    {
        RegisterResult Register(string login, string password);
    }
}
