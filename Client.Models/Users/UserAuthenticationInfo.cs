using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Notes.Client.Users
{
    /// <summary>
    /// Информация для аутентификации пользователя
    /// </summary>
    public class UserAuthenticationInfo
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
    }
}
