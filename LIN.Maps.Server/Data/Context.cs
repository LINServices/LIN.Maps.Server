namespace LIN.Maps.Server.Data;


public class Context : DbContext
{


    /// <summary>
    /// Tabla de cuentas
    /// </summary>
    public DbSet<LIN.Types.Maps.Models.ProfileModel> Profiles { get; set; }


    /// <summary>
    /// Puntos
    /// </summary>
    public DbSet<LIN.Types.Maps.Models.PlacePoint> Points { get; set; }



    /// <summary>
    /// Nuevo contexto a la base de datos
    /// </summary>
    public Context(DbContextOptions<Context> options) : base(options) { }



    /// <summary>
    /// Naming DB
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Indices y identidad
        modelBuilder.Entity<LIN.Types.Maps.Models.ProfileModel>()
           .HasKey(e => e.ID);

        // Unico
         modelBuilder.Entity<LIN.Types.Maps.Models.ProfileModel>()
           .HasIndex(e => e.AccountID)
           .IsUnique();

        // Nombre de la tablas
        modelBuilder.Entity<LIN.Types.Maps.Models.ProfileModel>().ToTable("PROFILES");
        modelBuilder.Entity<LIN.Types.Maps.Models.PlacePoint>().ToTable("POINTS");

    }


}