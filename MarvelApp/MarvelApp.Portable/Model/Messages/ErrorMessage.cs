using System;

namespace MarvelApp.Portable.Model.Messages
{
    /// <summary>
    /// Un mensaje de error a mostrar al usuario
    /// </summary>
    public class ErrorMessage
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public ErrorMessage(string title, string message)
            : this(title, message, null)
        {
        }
        public ErrorMessage(string title, string message, Exception exception)
        {
            this.Title = title;
            this.Message = message;
            this.Exception = exception;
        }
    }
}
