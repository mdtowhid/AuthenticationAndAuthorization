using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AuthAsp.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>
        {
            new Claim("Create Role", "Create Role"),
            new Claim("Edit Role", "Edit Role"),
            new Claim("Delete Role", "Delete Role")
        };
    }
}
