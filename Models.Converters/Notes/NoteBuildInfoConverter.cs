namespace Notes.Models.Converters.Notes
{
    using System;
    using Client = global::Notes.Client.Notes;
    using Model = global::Notes.Models.Notes;

    /// <summary>
    /// Предоставляет методы конвертирования запроса на создание заметки между клиентской и серверной моделями
    /// </summary>
    public static class NoteBuildInfoConverter
    {
        /// <summary>
        /// Переводит запрос на создание заметки из клиентсокой модели в серверную
        /// </summary>
        /// <param name="clientUserId">Идентификатор пользователя в клиентской модели</param>
        /// <param name="clientBuildInfo">Запрос на создание заметки в клиентской модели</param>
        /// <returns>Запрос на создание заметки в серверной модели</returns>
        public static Model.NoteCreationInfo Convert(string clientUserId, Client.NoteBuildInfo clientBuildInfo)
        {
            if (clientUserId == null)
            {
                throw new ArgumentNullException(nameof(clientUserId));
            }

            if (clientBuildInfo == null)
            {
                throw new ArgumentNullException(nameof(clientBuildInfo));
            }

            if (!Guid.TryParse(clientUserId, out var modelUserId))
            {
                throw new ArgumentException($"The client user id \"{clientUserId}\" is invalid.", nameof(clientUserId));
            }

            var modelCreationInfo = new Model.NoteCreationInfo(
                modelUserId,
                clientBuildInfo.Title,
                clientBuildInfo.Text,
                clientBuildInfo.Tags);

            return modelCreationInfo;
        }
    }
}
