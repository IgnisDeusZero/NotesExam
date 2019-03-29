namespace Notes.Models.Notes
{
    /// <summary>
    /// Аспект заметки для сортировки
    /// </summary>
    public enum NoteSortBy
    {
        /// <summary>
        /// Сортировкаи по дате создания
        /// </summary>
        Creation = 0,

        /// <summary>
        /// Сортировка по дате последнего изменения
        /// </summary>
        LastUpdate
    }
}
