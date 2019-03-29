namespace Notes.Models.Converters
{
    using System;
    using Client = global::Notes.Client;
    using Model = global::Notes.Models;

    /// <summary>
    /// Предоставляет методы конвертирования типа сортировки между клиентской и серверной моделями
    /// </summary>
    public static class SortTypeConverter
    {
        /// <summary>
        /// Переводит тип сортировки из серверной модели в клиентскую
        /// </summary>
        /// <param name="sortType">Тип сортировки в сервеной модели</param>
        /// <returns>Тип сортировки в клиентской модели</returns>
        public static Client.SortType Convert(Model.SortType sortType)
        {
            switch (sortType)
            {
                case SortType.Ascending:
                    return Client.SortType.Ascending;

                case SortType.Descending:
                    return Client.SortType.Descending;

                default:
                    throw new ArgumentException($"Unknown sort type \"{sortType}\".", nameof(sortType));
            }
        }

        /// <summary>
        /// Переводит тип сортировки из клиентской модели в серверную
        /// </summary>
        /// <param name="sortType">Тип сортировки в клиентской модели</param>
        /// <returns>Тип сортировки в серверной модели</returns>
        public static Model.SortType Convert(Client.SortType sortType)
        {
            switch (sortType)
            {
                case Client.SortType.Ascending:
                    return SortType.Ascending;

                case Client.SortType.Descending:
                    return SortType.Descending;

                default:
                    throw new ArgumentException($"Unknown sort type \"{sortType}\".", nameof(sortType));
            }
        }
    }
}
