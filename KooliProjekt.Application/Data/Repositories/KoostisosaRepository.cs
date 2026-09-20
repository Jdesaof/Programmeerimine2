namespace KooliProjekt.Application.Data.Repositories
{
    public class KoostisosaRepository : BaseRepository<Koostisosa>, IKoostisosaRepository
    {
        public KoostisosaRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
