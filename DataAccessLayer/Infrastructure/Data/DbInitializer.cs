using DomainLayer.Models.DataModels.AuthenticationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder _modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Seed()
        {
 
        }
    }

}
