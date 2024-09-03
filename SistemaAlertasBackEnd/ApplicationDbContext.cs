using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaAlertasBackEnd.Entidades;

namespace SistemaAlertasBackEnd
{
    public class ApplicationDbContext : IdentityDbContext //Aquí agregas automáticamente las tablas y todo lo necesario para los usuarios
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //ApiFluente:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SensorEntidad
            modelBuilder.Entity<SensorEntidad>().HasKey(x => x.SensorId);
            modelBuilder.Entity<SensorEntidad>().Property(x => x.NombreSensor)
                .IsRequired()
                .HasMaxLength(250);
            modelBuilder.Entity<SensorEntidad>().Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(250);
            modelBuilder.Entity<SensorEntidad>().Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(150);

            // LecturaEntidad
            modelBuilder.Entity<LecturaEntidad>().HasKey(x => x.ReadId);
            modelBuilder.Entity<LecturaEntidad>().Property(x => x.RegisterDate)
                .IsRequired();
            modelBuilder.Entity<LecturaEntidad>().Property(x => x.Unity)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<LecturaEntidad>().Property(x => x.ph_parameter)
                .HasPrecision(5, 2);
            modelBuilder.Entity<LecturaEntidad>().Property(x => x.orp_parameter)
                .HasPrecision(5, 2);
            modelBuilder.Entity<LecturaEntidad>().Property(x => x.turbidez_parameter)
                .HasPrecision(5, 2);

            // Configuración explícita de la relación uno a muchos
            modelBuilder.Entity<LecturaEntidad>()
                .HasOne<SensorEntidad>()  // Configura la relación
                .WithMany(s => s.LecturaEntidades)  // Relación uno a muchos
                .HasForeignKey(l => l.SensorId);  // Especifica la clave foránea

            // AlertaEntidad
            modelBuilder.Entity<AlertaEntidad>().HasKey(x => x.AlertaId);
            modelBuilder.Entity<AlertaEntidad>().Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<AlertaEntidad>().Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<AlertaEntidad>().Property(x => x.RegisterDate)
                .IsRequired();
            modelBuilder.Entity<AlertaEntidad>().Property(x => x.Level)
                .IsRequired()
                .HasMaxLength(50);

            // Configuración de las tablas de Identity
            modelBuilder.Entity<IdentityUser>().ToTable("Usuarios");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsuariosClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsuariosLogins");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsuariosRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsuariosTokens");
        }


        public DbSet<SensorEntidad> SensorEntitys { get; set; }
        public DbSet<LecturaEntidad> LecturaEntitys { get; set; }
        public DbSet<AlertaEntidad> AlertaEntitys { get; set; }
    }
}
