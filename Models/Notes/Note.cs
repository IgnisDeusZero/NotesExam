using MongoDB.Bson.Serialization.Attributes;

namespace Notes.Models.Notes
{
    /// <summary>
    /// Заметка
    /// </summary>
    public class Note : NoteInfo
    {
        /// <summary>
        /// Текст заметки
        /// </summary>
        [BsonElement("Text")]
        public string Text { get; set; }
    }
}
