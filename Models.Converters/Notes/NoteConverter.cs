namespace Notes.Models.Converters.Notes
{
    using System;
    using System.Linq;
    using Client = global::Notes.Client.Notes;
    using Model = global::Notes.Models.Notes;

    /// <summary>
    /// Предоставляет методы конвертирования заметки между серверной и клиентской моделями
    /// </summary>
    public static class NoteConverter
    {
        /// <summary>
        /// Переводит заметку из серверной модели в клиентскую
        /// </summary>
        /// <param name="modelNote">Заметка в серверной модели</param>
        /// <returns>Заметка в клиентской модели</returns>
        public static Client.Note Convert(Model.Note modelNote)
        {
            if (modelNote == null)
            {
                throw new ArgumentNullException(nameof(modelNote));
            }

            var clientNote = new Client.Note
            {
                Id = modelNote.Id.ToString(),
                UserId = modelNote.UserId.ToString(),
                CreatedAt = modelNote.CreatedAt,
                LastUpdatedAt = modelNote.LastUpdatedAt,
                Favorite = modelNote.Favorite,
                Title = modelNote.Title,
                Text = modelNote.Text,
                Tags = modelNote.Tags.ToList()
            };

            return clientNote;
        }
    }
}
