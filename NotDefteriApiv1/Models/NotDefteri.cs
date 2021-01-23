using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NotDefteriApiv1.Models
{
    public class NotDefteri
    {
        public int Id { get; set; }
        public DateTime? Time { get; set; }
        public string Icerik { get; set; }
    }
}
