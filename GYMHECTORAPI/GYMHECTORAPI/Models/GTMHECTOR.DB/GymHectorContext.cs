using Microsoft.EntityFrameworkCore;

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
        public DbSet<ModuloRolUsuarioObtener_Result> MpSp_ModuloRolUsuarioObtener { get; set; }
        public DbSet<MaestroDatosUsuario_Result> MpSp_MaestroDatosUsuario { get; set; }
        public DbSet<HorarioRegistrosUsuario_Result> MpSp_HorarioRegistrosUsuario { get; set; }
        public DbSet<ListaHorariosGeneral_Result> MpSp_HorariosGenerales { get; set; }
        public DbSet<RegistrarReserva_Result> MpSp_RegistrarReserva { get; set; }
        public DbSet<ListarPosiblesAsistencias_Result> MpSp_PosiblesAsistencias { get; set; }
        public DbSet<ListarAsistencias_Result> MpSp_Asistencias { get; set; }
        public DbSet<ListarReservas_Result> MpSp_ReservasIA { get; set; }
        public DbSet<EditarReserva_Result> MpSp_EditarReserva { get; set; }
        public DbSet<CancelarReserva_Result> MpSp_CancelarReserva { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SP_PRUEBA_TOKEN_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<ModuloRolUsuarioObtener_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<MaestroDatosUsuario_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<HorarioRegistrosUsuario_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<ListaHorariosGeneral_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<RegistrarReserva_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<ListarPosiblesAsistencias_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<ListarAsistencias_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<ListarReservas_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<EditarReserva_Result>().HasNoKey().ToView(null);
            modelBuilder.Entity<CancelarReserva_Result>().HasNoKey().ToView(null);
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
