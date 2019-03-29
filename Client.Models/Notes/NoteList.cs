namespace Notes.Client.Notes
{
    using System.Collections.Generic;

    /// <summary>
    ///  Список c описанием заметок
    /// </summary>
    public class NoteList
    {
        /// <summary>
        /// Краткая информация о заметках
        /// </summary>
        public IReadOnlyCollection<NoteInfo> Notes { get; set; }
    }
}
