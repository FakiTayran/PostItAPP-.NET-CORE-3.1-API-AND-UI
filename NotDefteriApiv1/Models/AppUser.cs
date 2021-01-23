using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NotDefteriApiv1.Models
{
    public class AppUser : IdentityUser
    {
        public string Token { get; set; }

        public ICollection<NotDefteri> NotDefteris { get; set; }
    }
}
