namespace Notes.API.Controllers
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using API.Errors;
    using Models.Converters.Notes;
    using Models.Notes.Repositories;
    using Notes.API.Auth;
    using Microsoft.AspNetCore.Http;

    [Route("v1/notes")]
    public sealed class NotesController : Controller
    {
        private readonly INoteRepository repository;
        private readonly IAuthenticator authenticator;

        public NotesController(INoteRepository repository, IAuthenticator authenticator)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (authenticator == null)
            {
                throw new ArgumentNullException(nameof(authenticator));
            }

            this.repository = repository;
            this.authenticator = authenticator;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateNoteAsync(
            [FromBody]Client.Notes.NoteBuildInfo clientBuildInfo, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!this.TryGetSessionState(this.HttpContext.Request.Cookies, out var state))
            {
                return this.Unauthorized();
            }
            if (!this.ModelState.IsValid)
            {
                var error = ServiceErrorResponses.BodyIsMissing(nameof(Client.Notes.NoteBuildInfo));
                return this.BadRequest(error);
            }

            var modelBuildInfo = NoteBuildInfoConverter.Convert(state.UserId.ToString(), clientBuildInfo);
            var modelNoteInfo = await this.repository.CreateAsync(modelBuildInfo, cancellationToken).ConfigureAwait(false);
            var clientNoteInfo = NoteInfoConverter.Convert(modelNoteInfo);
            var routeParams = new Dictionary<string, object>
            {
                { "noteId", clientNoteInfo.Id }
            };

            return this.CreatedAtRoute("GetNoteRoute", routeParams, clientNoteInfo);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> SearchNotesAsync(
            [FromQuery]Client.Notes.NoteInfoSearchQuery clientQuery, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!this.TryGetSessionState(this.HttpContext.Request.Cookies, out var state))
            {
                return this.Unauthorized();
            }

            var modelQuery = NoteInfoSearchQueryConverter.Convert(clientQuery ?? new Client.Notes.NoteInfoSearchQuery());
            modelQuery.UserId = state.UserId;
            var modelNotes = await this.repository.SearchAsync(modelQuery, cancellationToken).ConfigureAwait(false);
            var clientNotesList = modelNotes.Select(note => NoteInfoConverter.Convert(note)).ToList();
            return this.Ok(clientNotesList);
        }

        [HttpGet]
        [Route("{noteId}", Name = "GetNoteRoute")]
        public async Task<IActionResult> GetNoteAsync(
            [FromRoute]string noteId, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!this.TryGetSessionState(this.HttpContext.Request.Cookies, out var state))
            {
                return this.Unauthorized();
            }
            if (!Guid.TryParse(noteId, out var noteIdGuid))
            {
                var error = ServiceErrorResponses.NoteNotFound(noteId);
                return this.NotFound(error);
            }

            Models.Notes.Note modelNote = null;
            try
            {
                modelNote = await this.repository.GetAsync(noteIdGuid, cancellationToken).ConfigureAwait(false);
            }
            catch (Models.Notes.NoteNotFoundExcepction)
            {
                var error = ServiceErrorResponses.NoteNotFound(noteId);
                return this.NotFound(error);
            }
            if (!state.UserId.Equals(modelNote.UserId))
            {
                return this.Forbid();
            }

            var clientNote = NoteConverter.Convert(modelNote);
            return this.Ok(clientNote);
        }

        [HttpPatch]
        [Route("{noteId}")]
        public async Task<IActionResult> PatchNoteAsync(
            [FromRoute]string noteId, 
            [FromBody]Client.Notes.NotePatchInfo patchInfo, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!this.TryGetSessionState(this.HttpContext.Request.Cookies, out var state))
            {
                return this.Unauthorized();
            }
            if (patchInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("NotePatchInfo");
                return this.BadRequest(error);
            }
            if (!Guid.TryParse(noteId, out var noteIdGuid))
            {
                var error = ServiceErrorResponses.NoteNotFound(noteId);
                return this.NotFound(error);
            }
            if (!this.IsNoteBelongsToUserAsync(state, noteIdGuid, cancellationToken).Result)
            {
                return this.Forbid();
            }

            Models.Notes.Note modelNote = null;
            var modelPathInfo = NotePathcInfoConverter.Convert(noteIdGuid, patchInfo);
            try
            {
                modelNote = await this.repository.PatchAsync(modelPathInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (Models.Notes.NoteNotFoundExcepction)
            {
                var error = ServiceErrorResponses.NoteNotFound(noteId);
                return this.NotFound(error);
            }

            var clientNote = NoteConverter.Convert(modelNote);
            return this.Ok(clientNote);
        }

        [HttpDelete]
        [Route("{noteId}")]
        public async Task<IActionResult> DeleteNoteAsync(
            [FromRoute]string noteId, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!this.TryGetSessionState(this.HttpContext.Request.Cookies, out var state))
            {
                return this.Unauthorized();
            }
            if (!Guid.TryParse(noteId, out var noteIdGuid))
            {
                var error = ServiceErrorResponses.NoteNotFound(noteId);
                return this.NotFound(error);
            }
            if (!this.IsNoteBelongsToUserAsync(state, noteIdGuid, cancellationToken).Result)
            {
                return this.Forbid();
            }

            try
            {
                await this.repository.RemoveAsync(noteIdGuid, cancellationToken).ConfigureAwait(false);
            }
            catch (Models.Notes.NoteNotFoundExcepction)
            {
                var error = ServiceErrorResponses.NoteNotFound(noteId);
                return this.NotFound(error);
            }
            return this.NoContent();
        }

        private bool TryGetSessionState(IRequestCookieCollection cookie, out SessionState state) 
        {
            state = null;
            if (!cookie.TryGetValue("session_id", out var sessionId)
                || !cookie.TryGetValue("pass_hash", out var passHash)
                || !cookie.TryGetValue("user_id", out var userId))
            {
                return false;
            }
            try
            {
                state = this.authenticator.GetSessionAsync(sessionId, new CancellationToken()).Result;
            }
            catch (AuthenticationException)
            {
                return false;
            }
            return state.PasswordHash.Equals(passHash)
                && state.UserId.Equals(new Guid(userId));
        }

        private async Task<bool> IsNoteBelongsToUserAsync(
            SessionState state, Guid noteId, 
            CancellationToken cancellationToken)
        {
            Models.Notes.Note modelNote = null;
            try
            {
                modelNote = await this.repository.GetAsync(noteId, cancellationToken).ConfigureAwait(false);
            }
            catch (Models.Notes.NoteNotFoundExcepction)
            {
                return false;
            }
            return modelNote.UserId == state.UserId;
        }
    }
}
