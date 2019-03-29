namespace Notes.Models.Notes
{
    using System;

    /// <summary>
    /// Информация для изменения заметки
    /// </summary>
    public class NotePatchInfo
    {
        /// <summary>
        /// Создает новый экземпляр объекта, описывающего изменение заметки
        /// </summary>
        /// <param name="noteId">Идентификатор заметки, которую нужно изменить</param>
        /// <param name="title">Новый заголовок заметки</param>
        /// <param name="text">Новый текст заметки</param>
        /// <param name="favorite">Флаг, указывающий, что заявка находится в избранном</param>
        public NotePatchInfo(Guid noteId, bool? favorite = null, string title = null, string text = null)
        {
            this.NoteId = noteId;
            this.Favorite = favorite;
            this.Title = title;
            this.Text = text;
        }

        /// <summary>
        /// Идентификатор заметки, которую нужно изменить
        /// </summary>
        public Guid NoteId { get; }

        /// <summary>
        /// Флаг, указывающий, что заявка находится в избранном
        /// </summary>
        public bool? Favorite { get; set; }

        /// <summary>
        /// Новый заголовок заметки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Новый текст заметки
        /// </summary>
        public string Text { get; set; }
    }
}
