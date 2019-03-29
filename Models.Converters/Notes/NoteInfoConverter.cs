namespace Notes.Models.Converters.Notes
{
    using System;
    using System.Linq;
    using Client = global::Notes.Client.Notes;
    using Model = global::Notes.Models.Notes;

    /// <summary>
    /// Предоставляет методы конвертирования информации о заметке между серверной и клиентской моделями
    /// </summary>
    public static class NoteInfoConverter
    {
        /// <summary>
        /// Переводит информацию о заметке из серверной модели в клиентскую
        /// </summary>
        /// <param name="modelNoteInfo">Информация о заметке в серверной модели</param>
        /// <returns>Информация о заметке в клиентской модели</returns>
        public static Client.NoteInfo Convert(Model.NoteInfo modelNoteInfo)
        {
            if (modelNoteInfo == null)
            {
                throw new ArgumentNullException(nameof(modelNoteInfo));
            }

            var clientNoteInfo = new Client.NoteInfo
            {
                Id = modelNoteInfo.Id.ToString(),
                UserId = modelNoteInfo.UserId.ToString(),
                CreatedAt = modelNoteInfo.CreatedAt,
                LastUpdatedAt = modelNoteInfo.LastUpdatedAt,
                Favorite = modelNoteInfo.Favorite,
                Title = modelNoteInfo.Title,
                Tags = modelNoteInfo.Tags.ToList()
            };

            return clientNoteInfo;
        }
    }
}
