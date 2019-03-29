using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Notes.Models.Notes
{
    /// <summary>
    /// Информация о заметке
    /// </summary>
    public class NoteInfo
    {
        /// <summary>
        /// Идентификатор заметки
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит заметка
        /// </summary>
        [BsonElement("UserId")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Дата создания заметки
        /// </summary>
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        [BsonElement("LastUpdatedAt")]
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Флаг, указывающий, находится заметка в избранном или нет
        /// </summary>
        [BsonElement("Favorite")]
        public bool Favorite { get; set; }

        /// <summary>
        /// Название заметки
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }

        /// <summary>
        /// Теги заметки
        /// </summary>
        [BsonElement("Tags")]
        public IReadOnlyList<string> Tags { get; set; }
    }
}
