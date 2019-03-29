namespace Notes.Models.Converters.Notes
{
    using System;
    using Client = global::Notes.Client.Notes;
    using Model = global::Notes.Models.Notes;

    /// <summary>
    /// Предоставляет методы конвертирования запроса на изменение заметки между клиентской и серверной моделями
    /// </summary>
    public static class NotePathcInfoConverter
    {
        /// <summary>
        /// Переводит запрос на изменение заметки из клиентсокой модели в серверную
        /// </summary>
        /// <param name="noteId">Идентификатор заметки</param>
        /// <param name="clientPatchInfo">Запрос на изменение заметки в клиентской модели</param>
        /// <returns>Запрос на изменение заметки в серверной модели</returns>
        public static Model.NotePatchInfo Convert(Guid noteId, Client.NotePatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }
            
            var modelPatchInfo = new Model.NotePatchInfo(noteId)
            {
                Favorite = clientPatchInfo.Favorite,
                Text = clientPatchInfo.Text,
                Title = clientPatchInfo.Title
            };

            return modelPatchInfo;
        }
    }
}
