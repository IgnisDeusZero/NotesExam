namespace Notes.API.Errors
{
    using System;
    using System.Net;
    using Client.Errors;

    public static class ServiceErrorResponses
    {
        public static ServiceErrorResponse NoteNotFound(string noteId)
        {
            if (noteId == null)
            {
                throw new ArgumentNullException(nameof(noteId));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotFound,
                    Message = $"A note with \"{noteId}\" not found.",
                    Target = "note"
                }
            };

            return error;
        }

        public static ServiceErrorResponse BodyIsMissing(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = "Request body is empty.",
                    Target = target
                }
            };

            return error;
        }
    }
}
