namespace Notes.Client.Notes
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Информация для создания заметки
    /// </summary>
    [DataContract]
    public class NoteBuildInfo
    {
        /// <summary>
        /// Заголовок заметки
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Текст заметки
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Text { get; set; }

        /// <summary>
        /// Теги заметки
        /// </summary>
        [DataMember(IsRequired = false)]
        public IReadOnlyList<string> Tags { get; set; }
    }
}
