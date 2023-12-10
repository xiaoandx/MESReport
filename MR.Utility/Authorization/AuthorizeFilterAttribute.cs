using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MR.Utility.Authorization
{
    /// <summary>
    /// AuthorizeFilterAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeFilterAttribute : Attribute {

        /// <summary>
        /// 资源标识名称
        /// </summary>
        public string FilterName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterName"></param>
        public AuthorizeFilterAttribute(string filterName) {
            FilterName = filterName;
        }
    }
}
