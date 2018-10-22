using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using SmartHomeWebControl.Models;

namespace SmartHomeWebControl.Controllers
{
    public class WebControlDbContext : DbContext
    {
        public DbSet<Token> Tokens { get; set; }
    }
}
