using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EmpApplication.Models
{
    public class EmpDBContext : DbContext
    {
        public EmpDBContext() : base("name=EmpConnectionString")
        {
        }
        public virtual DbSet<Employee> Employees { get; set; }

    }
}
