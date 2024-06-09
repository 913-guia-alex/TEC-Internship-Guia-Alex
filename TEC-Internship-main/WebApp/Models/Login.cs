using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace WebApp.Models
{
    public class Login
    {
        public  string username { get; set; }
        public string password { get; set; }
    }
}
