using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.Entity;

namespace DataAccess
{
 public   class AcmeContext: DbContext, IDisposable
    {

        public DbSet<Employee> Employees { get; set; }

        public AcmeContext(): base("Name=AcmeContext"){}
 
    
    }
}
