namespace Notes.API.Register
{
    using System;

    public class RegistrationException : Exception
    {
        public RegistrationException()
            : base("Username duplicate.")
        {
        }

        public RegistrationException(string message) :
            base()
        {
        }
    }
}
