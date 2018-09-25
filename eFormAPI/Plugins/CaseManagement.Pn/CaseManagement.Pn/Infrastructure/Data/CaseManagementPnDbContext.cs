using System.Data.Entity;
using CaseManagement.Pn.Infrastructure.Data.Entities;

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

        public DbSet<CaseManagementSetting> CaseManagementSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<CaseManagementSetting>()
            //    .Property(e => e.SelectedTemplateId)
            //    .HasColumnAnnotation(
            //        IndexAnnotation.AnnotationName,
            //        new IndexAnnotation(new IndexAttribute { IsUnique = false }));
        }
    }
}
