using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WinSite.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string city { get; set; }
    }
}