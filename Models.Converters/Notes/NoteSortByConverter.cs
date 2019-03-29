namespace Notes.Models.Converters.Notes
{
    using System;
    using Client = global::Notes.Client.Notes;
    using Model = global::Notes.Models.Notes;

    /// <summary>
    /// Предоставляет методы конвертирования аспекта сортировки заметок между клиентской и серверной моделями
    /// </summary>
    public static class NoteSortByConverter
    {
        /// <summary>
        /// Переводит аспект сортировки заметок из клиентской модели в серверную
        /// </summary>
        /// <param name="clientSortBy">Аспект сортировки заметок в клиентской модели</param>
        /// <returns>Аспект сортировки заметок в серверной модели</returns>
        public static Model.NoteSortBy Convert(Client.NoteSortBy clientSortBy)
        {
            switch (clientSortBy)
            {
                case Client.NoteSortBy.Creation:
                    return Model.NoteSortBy.Creation;

                case Client.NoteSortBy.LastUpdate:
                    return Model.NoteSortBy.LastUpdate;

                default:
                    throw new ArgumentException($"Unknown sort by value \"{clientSortBy}\".", nameof(clientSortBy));
            }
        }
    }
}
