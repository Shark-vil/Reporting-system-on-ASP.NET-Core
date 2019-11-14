using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTING_WEB_API_ASP.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Models.DocumentsModel.Documents> Documents { get; set; }
        //public DbSet<Models.DocumentsFilesModel.DocumentsFiles> DocumentsFiles { get; set; }
    }
}
