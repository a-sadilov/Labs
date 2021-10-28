using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAplication.Models;
//using System.Data.Entity;


namespace WebApiAplication
{
    public class AppDataBaseContext : DbContext
    {
        public AppDataBaseContext(DbContextOptions<AppDataBaseContext> options) 
            : base(options) { }

        public DbSet<LibraryRecord> LibraryRecords { get; set; } 
        public DbSet<LastRecordTime> LastRecordTimes { get; set; } 
    }
}
