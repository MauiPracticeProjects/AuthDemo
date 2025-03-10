using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHybrid.Shared
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Don't send PasswordHash here, let the API handle it.
    }
}
