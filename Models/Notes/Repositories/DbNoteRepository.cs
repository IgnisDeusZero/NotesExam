using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Notes.Models.Notes.Repositories
{
    public sealed class DbNoteRepository : INoteRepository
    {
        private readonly IMongoCollection<Note> notes;

        public DbNoteRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("NotesDb");
            notes = database.GetCollection<Note>("Notes");
        }

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
            notes.InsertOne(note);

            return Task.FromResult<NoteInfo>(note);
        }

        public Task<Note> GetAsync(Guid noteId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var searchResult = notes.Find(x => x.Id == noteId);
            if (!searchResult.Any())
            {
                throw new NoteNotFoundExcepction(noteId);
            }
            return Task.FromResult(searchResult.First());
        }

        public Task<Note> PatchAsync(NotePatchInfo patchInfo, CancellationToken cancelltionToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancelltionToken.ThrowIfCancellationRequested();

            var searchResult = notes.Find(x => x.Id == patchInfo.NoteId);
            if (!searchResult.Any())
            {
                throw new NoteNotFoundExcepction(patchInfo.NoteId);
            }
            
            var updated = false;
            var note = searchResult.First();
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
                notes.ReplaceOne(x => x.Id == note.Id, note);
            }

            return Task.FromResult(note);
        }

        public Task RemoveAsync(Guid noteId, CancellationToken cancelltionToken)
        {
            cancelltionToken.ThrowIfCancellationRequested();

            var searchResult = notes.Find(x => x.Id == noteId);
            if (!searchResult.Any())
            {
                throw new NoteNotFoundExcepction(noteId);
            }

            notes.DeleteOne(x => x.Id == noteId);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<NoteInfo>> SearchAsync(NoteInfoSearchQuery query, CancellationToken cancelltionToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancelltionToken.ThrowIfCancellationRequested();
            var builder = Builders<Note>.Filter;
            var filter = builder.Empty;
            if (query.CreatedFrom != null)
            {
                filter |= builder.Gte("CreatedAt", query.CreatedFrom.Value);
            }

            if (query.CreatedTo != null)
            {
                filter |= builder.Lte("CreatedAt", query.CreatedTo.Value);
            }

            if (query.UserId != null)
            {
                filter |= builder.Eq("UserId", query.UserId.Value);
            }

            if (query.Favorite != null)
            {
                filter |= builder.Eq("Favorite", query.Favorite.Value);
            }

            var result = notes.Find(filter);

            if (query.Offset != null)
            {
                result = result.Skip(query.Offset.Value);
            }

            if (query.Limit != null)
            {
                result = result.Limit(query.Offset.Value);
            }
            var sort = query.Sort ?? SortType.Ascending;
            var sortBy = query.SortBy ?? NoteSortBy.Creation;

            if (sort != SortType.Ascending || sortBy != NoteSortBy.Creation)
            {
                string sortField = "";
                switch (sortBy)
                {
                    case NoteSortBy.LastUpdate:
                        sortField = "LastUpdatedAt";
                        break;
                    case NoteSortBy.Creation:
                        sortField = "CreatedAt";
                        break;
                    default:
                        throw new ArgumentException($"Unknown note sort by value \"{sortBy}\".", nameof(query));
                }
                var sortDefinition = sort == SortType.Ascending
                    ? new SortDefinitionBuilder<Note>().Ascending(sortField)
                    : new SortDefinitionBuilder<Note>().Ascending(sortField);
                result = result.Sort(sortDefinition);
            }

            var listResult = result.ToList();
            if (query.Tags != null)
            {
                listResult = listResult
                    .Where(note => query.Tags.All(tag => note.Tags.Contains(tag)))
                    .ToList();
            }

            return Task.FromResult<IReadOnlyList<NoteInfo>>(listResult);
        }
    }
}
