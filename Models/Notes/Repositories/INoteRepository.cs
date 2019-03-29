namespace Notes.Models.Notes.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс хранилища заметок
    /// </summary>
    public interface INoteRepository
    {
        /// <summary>
        /// Создать новую заметку
        /// </summary>
        /// <param name="creationInfo">Информация для создания заметки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронное создание заметки. Результат выполнения операции - информация о созданной заметке</returns>
        Task<NoteInfo> CreateAsync(NoteCreationInfo creationInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Найти заметки
        /// </summary>
        /// <param name="query">Поисковый запрос</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный поиск заметок. Результат выполнения - список найденных заметок</returns>
        Task<IReadOnlyList<NoteInfo>> SearchAsync(NoteInfoSearchQuery query, CancellationToken cancelltionToken);

        /// <summary>
        /// Запросить заметку
        /// </summary>
        /// <param name="noteId">Идентификатор заметки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представлящая асинхронный запрос заметки. Результат выполнения - заметка</returns>
        Task<Note> GetAsync(Guid noteId, CancellationToken cancellationToken);

        /// <summary>
        /// Изменить заметку
        /// </summary>
        /// <param name="patchInfo">Описание изменений заметки</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на изменение заметки. Результат выполнения - актуальное состояние заметки</returns>
        Task<Note> PatchAsync(NotePatchInfo patchInfo, CancellationToken cancelltionToken);

        /// <summary>
        /// Удалить заметку
        /// </summary>
        /// <param name="noteId">Идентификатор заметки</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на удаление заметки</returns>
        Task RemoveAsync(Guid noteId, CancellationToken cancelltionToken);
    }
}
