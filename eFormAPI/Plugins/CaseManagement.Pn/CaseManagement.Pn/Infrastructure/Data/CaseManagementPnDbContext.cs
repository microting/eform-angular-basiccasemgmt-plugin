using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagement.Pn.Infrastructure.Data
{
    public class CaseManagementPnDbContext : DbContext
    {
        public CaseManagementPnDbContext()
            : base("eFormCaseManagementPnConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<CaseManagementPnDbContext>(null);
        }

        public CaseManagementPnDbContext(string connectionString)
            : base(connectionString)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<CaseManagementPnDbContext>(null);
        }

        public static CaseManagementPnDbContext Create()
        {
            return new CaseManagementPnDbContext();
        }

     //   public DbSet<Customer> Customers { get; set; }
    //    public DbSet<Field> Fields { get; set; }
     //   public DbSet<CustomerField> CustomerFields { get; set; }
    //    public DbSet<CustomerSettings> CustomerSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Customer>()
            //    .Property(e => e.RelatedEntityId)
            //    .HasColumnAnnotation(
            //        IndexAnnotation.AnnotationName,
            //        new IndexAnnotation(new IndexAttribute { IsUnique = true }));
        }
    }
}
