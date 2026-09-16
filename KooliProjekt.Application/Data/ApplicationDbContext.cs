using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Olu> Olud { get; set; }
        public DbSet<Partii> Partiid { get; set; }
        public DbSet<Maitsmine> Maitsmised { get; set; }
        public DbSet<Koostisosa> Koostisosad { get; set; }
        public DbSet<PruulimisLogi> PruulimisLogid { get; set; }
        public DbSet<PartiiFoto> PartiiFotod { get; set; }
    }
}