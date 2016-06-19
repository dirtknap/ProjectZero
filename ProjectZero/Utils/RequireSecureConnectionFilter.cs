using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProjectZero.Utils
{
    public class RequireSecureConnectionFilter : RequireHttpsAttribute
    {
        /// <summary>
        /// Filter out local requests so SSL is not enforced for
        /// local connections without risking production security
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentException("filterContext");
            }

            if (filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }
}
