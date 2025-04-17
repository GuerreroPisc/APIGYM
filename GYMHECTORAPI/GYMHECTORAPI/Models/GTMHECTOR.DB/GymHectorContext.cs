using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GYMHECTORAPI.Models.GTMHECTOR.DB
{
    public partial class GymHectorContext : DbContext
    {
        public GymHectorContext()
        {
        }
        public GymHectorContext(DbContextOptions<GymHectorContext> options)
                 : base(options)
        {
        }
        public DbSet<SP_PRUEBA_TOKEN_Result> MpSp_AutenticarUsuario { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SP_PRUEBA_TOKEN_Result>().HasNoKey().ToView(null);
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
