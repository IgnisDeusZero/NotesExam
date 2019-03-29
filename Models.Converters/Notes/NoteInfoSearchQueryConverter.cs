namespace Notes.Models.Converters.Notes
{
    using System;
    using System.Linq;
    using Client = global::Notes.Client.Notes;
    using Model = global::Notes.Models.Notes;

    /// <summary>
    /// Предоставляет методы конвертирования запроса за заметками между клиентской и серверной моделями
    /// </summary>
    public static class NoteInfoSearchQueryConverter
    {
        /// <summary>
        /// Переводит запрос за заметками из клиентсокой модели в серверную
        /// </summary>
        /// <param name="clientQuery">Запрос за заметками в клиентской модели</param>
        /// <returns>Запрос за заметками в серверной модели</returns>
        public static Model.NoteInfoSearchQuery Convert(Client.NoteInfoSearchQuery clientQuery)
        {
            if (clientQuery == null)
            {
                throw new ArgumentNullException(nameof(clientQuery));
            }

            var modelUserId = (Guid?)null;

            if (clientQuery.UserId != null)
            {
                if (!Guid.TryParse(clientQuery.UserId, out var userId))
                {
                    throw new ArgumentException($"The user id \"{clientQuery.UserId}\" is invalid.", nameof(clientQuery));
                }

                modelUserId = userId;
            }


            var modelSort = clientQuery.Sort.HasValue ?
                SortTypeConverter.Convert(clientQuery.Sort.Value) :
                (Models.SortType?)null;

            var modelSortBy = clientQuery.SortBy.HasValue ?
                NoteSortByConverter.Convert(clientQuery.SortBy.Value) :
                (Model.NoteSortBy?)null;

            var modelQuery = new Model.NoteInfoSearchQuery
            {
                CreatedFrom = clientQuery.CreatedFrom,
                CreatedTo = clientQuery.CreatedTo,
                UserId = modelUserId,
                Favorite = clientQuery.Favorite,
                Limit = clientQuery.Limit,
                Offset = clientQuery.Offset,
                Sort = modelSort,
                SortBy = modelSortBy,
                Tags = clientQuery.Tags?.ToList()
            };

            return modelQuery;
        }
    }
}
