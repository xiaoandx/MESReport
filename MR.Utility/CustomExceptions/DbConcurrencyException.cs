using System;
using System.Collections.Generic;
using System.Text;

namespace MR.Utility.CustomExceptions {

    /// <summary>
    /// DbConcurrencyException
    /// </summary>
    [Serializable]
    public class DbConcurrencyException : Exception {

        /// <summary>
        /// DbConcurrencyException
        /// </summary>
        public DbConcurrencyException() { }

        /// <summary>
        /// DbConcurrencyException
        /// </summary>
        /// <param name="message"></param>
        public DbConcurrencyException(string message) : base(message) { }

        /// <summary>
        /// DbConcurrencyException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DbConcurrencyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
