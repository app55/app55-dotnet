using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace App55 {

    public class ApiException : Exception {
        private string message;
        private long? code;
        private object body;

        internal ApiException(string message, long? code, object body) {
            this.message = message;
            this.code = code;
            this.body = body;
        }

        public override string Message {
            get {
                return message;
            }
        }

        public long? Code {
            get {
                return code;
            }
        }

        public object Body {
            get {
                return body;
            }
        }

        public override string ToString() {
            return this.message;
        }

        internal static ApiException CreateException(Hashtable error) {
            if((string)error["type"] == "request-error")
                return new RequestException((string)error["message"], error["code"] == null ? null : (long?)long.Parse(error["code"].ToString()), error["body"]);
            if((string)error["type"] == "resource-error")
                return new ResourceException((string)error["message"], error["code"] == null ? null : (long?)long.Parse(error["code"].ToString()), error["body"]);
            if((string)error["type"] == "authentication-error")
                return new AuthenticationException((string)error["message"], error["code"] == null ? null : (long?)long.Parse(error["code"].ToString()), error["body"]);
            if((string)error["type"] == "server-error")
                return new ServerException((string)error["message"], error["code"] == null ? null : (long?)long.Parse(error["code"].ToString()), error["body"]);
            if((string)error["type"] == "validation-error")
                return new ValidationException((string)error["message"], error["code"] == null ? null : (long?)long.Parse(error["code"].ToString()), error["body"]);
            if((string)error["type"] == "card-error")
                return new CardException((string)error["message"], error["code"] == null ? null : (long?)long.Parse(error["code"].ToString()), error["body"]);

            return new ApiException((string)error["message"], error["code"] == null ? null : (long?)long.Parse(error["code"].ToString()), error["body"]);
        }
    }

    public sealed class InvalidSignatureException : ApiException {
        internal InvalidSignatureException() : base("The response contained an invalid signature.", null, null) {

        }
    }

    public sealed class RequestException : ApiException {
        internal RequestException(string message, long? code, object body) : base(message, code, body) {

        }
    }

    public sealed class ResourceException : ApiException {
        internal ResourceException(string message, long? code, object body) : base(message, code, body) {

        }
    }

    public sealed class AuthenticationException : ApiException {
        internal AuthenticationException(string message, long? code, object body) : base(message, code, body) {

        }
    }

    public sealed class ServerException : ApiException {
        internal ServerException(string message, long? code, object body) : base(message, code, body) {

        }
    }

    public sealed class ValidationException : ApiException {
        internal ValidationException(string message, long? code, object body) : base(message, code, body) {

        }
    }

    public sealed class CardException : ApiException {
        internal CardException(string message, long? code, object body) : base(message, code, body) {

        }
    }
}
