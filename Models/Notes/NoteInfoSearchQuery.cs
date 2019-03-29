namespace Notes.Models.Notes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Параметры поиска заметок
    /// </summary>
    public class NoteInfoSearchQuery
    {
        /// <summary>
        /// Позиция, начиная с которой нужно производить поиск
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Максимальеное количество заметок, которое нужно вернуть
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Пользователь, которому принадлежит заметка
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Минимальная дата создания заметки
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Максимальная дата создания заметки
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// Искать по параметру "в избранном"
        /// </summary>
        public bool? Favorite { get; set; }

        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortType? Sort { get; set; }

        /// <summary>
        /// Аспект заметки, по которому нужно искать
        /// </summary>
        public NoteSortBy? SortBy { get; set; }

        /// <summary>
        /// Теги заметок
        /// </summary>
        public IReadOnlyList<string> Tags { get; set; }
    }
}
