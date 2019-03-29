namespace Notes.Models.Notes.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Хранилище заметок в памяти
    /// 
    /// Потоконебезопасно
    /// </summary>
    public sealed class MemoryNoteRepository : INoteRepository
    {
        private readonly Dictionary<Guid, Note> primaryKeyIndex;
        private readonly List<NoteInfo> sequenceIndex;
        private readonly List<Note> removedNotes;

        /// <summary>
        /// Создает новый экземпляр хранилища заметок в памяти
        /// </summary>
        public MemoryNoteRepository()
        {
            this.primaryKeyIndex = new Dictionary<Guid, Note>();
            this.sequenceIndex = new List<NoteInfo>();
            this.removedNotes = new List<Note>();
        }

        /// <summary>
        /// Создать новую заметку
        /// </summary>
        /// <param name="creationInfo">Информация для создания заметки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронное создание заметки. Результат выполнения операции - информация о созданной заметке</returns>
        public Task<NoteInfo> CreateAsync(NoteCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var note = new Note
            {
                Id = id,
                UserId = creationInfo.UserId,
                CreatedAt = now,
                LastUpdatedAt = now,
                Favorite = false,
                Title = creationInfo.Title,
                Text = creationInfo.Text,
                Tags = creationInfo.Tags
            };

            this.primaryKeyIndex.Add(id, note);
            this.sequenceIndex.Add(note);

            return Task.FromResult<NoteInfo>(note);
        }

        /// <summary>
        /// Запросить заметку
        /// </summary>
        /// <param name="noteId">Идентификатор заметки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представлящая асинхронный запрос заметки. Результат выполнения - заметка</returns>
        public Task<Note> GetAsync(Guid noteId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!this.primaryKeyIndex.TryGetValue(noteId, out var note))
            {
                throw new NoteNotFoundExcepction(noteId);
            }

            return Task.FromResult(note);
        }

        /// <summary>
        /// Изменить заметку
        /// </summary>
        /// <param name="patchInfo">Описание изменений заметки</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на изменение заметки. Результат выполнения - актуальное состояние заметки</returns>
        public Task<Note> PatchAsync(NotePatchInfo patchInfo, CancellationToken cancelltionToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancelltionToken.ThrowIfCancellationRequested();

            if (!this.primaryKeyIndex.TryGetValue(patchInfo.NoteId, out var note))
            {
                throw new NoteNotFoundExcepction(patchInfo.NoteId);
            }

            var updated = false;

            if (patchInfo.Title != null)
            {
                note.Title = patchInfo.Title;
                updated = true;
            }

            if (patchInfo.Text != null)
            {
                note.Text = patchInfo.Text;
                updated = true;
            }

            if (patchInfo.Favorite != null)
            {
                note.Favorite = patchInfo.Favorite.Value;
                updated = true;
            }

            if (updated)
            {
                note.LastUpdatedAt = DateTime.UtcNow;
            }

            return Task.FromResult(note);
        }

        /// <summary>
        /// Удалить заметку
        /// </summary>
        /// <param name="noteId">Идентификатор заметки</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на удаление заметки</returns>
        public Task RemoveAsync(Guid noteId, CancellationToken cancelltionToken)
        {
            cancelltionToken.ThrowIfCancellationRequested();

            if (!this.primaryKeyIndex.TryGetValue(noteId, out var note))
            {
                throw new NoteNotFoundExcepction(noteId);
            }

            this.primaryKeyIndex.Remove(noteId);
            this.sequenceIndex.Remove(note);

            this.removedNotes.Add(note);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Найти заметки
        /// </summary>
        /// <param name="query">Поисковый запрос</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный поиск заметок. Результат выполнения - список найденных заметок</returns>
        public Task<IReadOnlyList<NoteInfo>> SearchAsync(NoteInfoSearchQuery query, CancellationToken cancelltionToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancelltionToken.ThrowIfCancellationRequested();

            var search = this.sequenceIndex.AsEnumerable();

            if (query.CreatedFrom != null)
            {
                search = search.Where(note => note.CreatedAt >= query.CreatedFrom.Value);
            }

            if (query.CreatedTo != null)
            {
                search = search.Where(note => note.CreatedAt <= query.CreatedTo.Value);
            }

            if (query.UserId != null)
            {
                search = search.Where(note => note.UserId == query.UserId.Value);
            }

            if (query.Favorite != null)
            {
                search = search.Where(note => note.Favorite = query.Favorite.Value);
            }

            if (query.Tags != null)
            {
                bool whereTagsPresent(NoteInfo noteInfo)
                {
                    return query.Tags.All(tag => noteInfo.Tags.Contains(tag));
                }

                search = search.Where(whereTagsPresent);
            }

            if (query.Offset != null)
            {
                search = search.Take(query.Offset.Value);
            }

            if (query.Limit != null)
            {
                search = search.Take(query.Limit.Value);
            }

            var sort = query.Sort ?? SortType.Ascending;
            var sortBy = query.SortBy ?? NoteSortBy.Creation;

            if (sort != SortType.Ascending || sortBy != NoteSortBy.Creation)
            {
                DateTime select(NoteInfo note)
                {
                    switch (sortBy)
                    {
                        case NoteSortBy.LastUpdate:
                            return note.LastUpdatedAt;

                        case NoteSortBy.Creation:
                            return note.CreatedAt;

                        default:
                            throw new ArgumentException($"Unknown note sort by value \"{sortBy}\".", nameof(query));
                    }
                }

                search = sort == SortType.Ascending ?
                    search.OrderBy(select) :
                    search.OrderByDescending(select);
            }

            var result = search.ToList();

            return Task.FromResult<IReadOnlyList<NoteInfo>>(result);
        }
    }
}
