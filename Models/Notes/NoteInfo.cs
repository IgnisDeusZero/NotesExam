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
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит заметка
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Дата создания заметки
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Флаг, указывающий, находится заметка в избранном или нет
        /// </summary>
        public bool Favorite { get; set; }

        /// <summary>
        /// Название заметки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Теги заметки
        /// </summary>
        public IReadOnlyList<string> Tags { get; set; }
    }
}
