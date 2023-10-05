using Moneyboard.Core.Resources;
using System;
using System.Runtime.Serialization;

namespace Moneyboard.Core.Exeptions.FileExceptions
{
    [Serializable]
    class FileIsEmptyException : FileException
    {
        public FileIsEmptyException()
            : base(ErrorMessages.FileIsEmpty) { }

        public FileIsEmptyException(Exception innerException)
            : base(ErrorMessages.FileIsEmpty, innerException) { }

        public FileIsEmptyException(string path)
            : base(ErrorMessages.FileIsEmpty, path) { }

        protected FileIsEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
