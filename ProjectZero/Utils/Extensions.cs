using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZero.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Method to determin if request is local so that we can ignore
        /// using ssl for local requests to WebAPI.  Eases development wihtout
        /// risking production security
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsLocal(this HttpRequestMessage request)
        {
            var localFlag = request.Properties["MS_IsLocal"] as Lazy<bool>;
            return localFlag != null && localFlag.Value;
        }
    }
}
