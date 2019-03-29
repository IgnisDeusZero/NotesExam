namespace Notes.Models.Users
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [BsonElement("Login")]
        public string Login { get; set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        [BsonElement("RegisteredAt")]
        public DateTime RegisteredAt { get; set; }
    }
}
