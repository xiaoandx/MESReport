using System;
using System.Collections.Generic;
using System.Text;

namespace MR.Utility.CustomExceptions
{
    /// <summary>
    /// 业务中可预见得异常
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// BusinessException
        /// </summary>
        public BusinessException() { }

        /// <summary>
        /// BusinessException
        /// </summary>
        /// <param name="message"></param>
        public BusinessException(string message) : base(message) { }

        /// <summary>
        /// BusinessException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public BusinessException(string message, Exception innerException) : base(message, innerException) { }
    }
}
