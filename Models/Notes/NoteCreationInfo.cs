namespace Notes.Models.Notes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Информация для создания заметки
    /// </summary>
    public class NoteCreationInfo
    {
        /// <summary>
        /// Создает заметку
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, который хочет создать заметку</param>
        /// <param name="title">Заголовок заметки</param>
        /// <param name="text">Текст заметки</param>
        /// <param name="tags">Теги заметки</param>
        public NoteCreationInfo(Guid userId, string title, string text, IEnumerable<string> tags = null)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.UserId = userId;
            this.Title = title;
            this.Text = text;
            this.Tags = tags?.ToArray() ?? new string[] { };
        }

        /// <summary>
        /// Идентификатор пользователя, который хочет создать заметку
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Заголовок заметки
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Текст заметки
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Теги заметки
        /// </summary>
        public IReadOnlyList<string> Tags { get; }
    }
}
