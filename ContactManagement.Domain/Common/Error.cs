using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Common
{

    public class Error : IEquatable<Error>
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.", ErrorType.Validation);

        public Error(string code, string message, ErrorType type)
        {
            Code = code;
            Message = message;
            Type = type;
        }

        public string Code { get; }
        public string Message { get; }
        public ErrorType Type { get; }

        public static Error NotFound(string code = "NotFound", string message = "Record not found")
            => new(code, message, ErrorType.NotFound);

        public static Error Validation(string code = "ValidationError", string message = "Validation error occurred")
            => new(code, message, ErrorType.Validation);

        public static Error Conflict(string code = "Conflict", string message = "Conflict error occurred")
            => new(code, message, ErrorType.Conflict);

        public static Error Unauthorized(string code = "Unauthorized", string message = "Unauthorized access")
            => new(code, message, ErrorType.Unauthorized);

        public static Error Forbidden(string code = "Forbidden", string message = "Forbidden access")
            => new(code, message, ErrorType.Forbidden);

        public static Error Internal(string code = "InternalError", string message = "Internal error occurred")
            => new(code, message, ErrorType.Internal);

        public static implicit operator string(Error error) => error?.Message ?? string.Empty;

        public bool Equals(Error? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Code == other.Code && Message == other.Message && Type == other.Type;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Error)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Message, Type);
        }

        public override string ToString()
        {
            return $"{Code}: {Message} ({Type})";
        }

        public static bool operator ==(Error? left, Error? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Error? left, Error? right)
        {
            return !(left == right);
        }
    }

    public enum ErrorType
    {
        None = 0,
        NotFound = 1,
        Validation = 2,
        Conflict = 3,
        Unauthorized = 4,
        Forbidden = 5,
        Internal = 6
    }

    public static class ErrorExtensions
    {
        public static Error WithMessage(this Error error, string message)
        {
            return new Error(error.Code, message, error.Type);
        }

        public static Error WithCode(this Error error, string code)
        {
            return new Error(code, error.Message, error.Type);
        }
    }
}
