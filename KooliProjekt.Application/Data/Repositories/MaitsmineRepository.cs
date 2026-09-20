namespace KooliProjekt.Application.Data.Repositories
{
    public class MaitsmineRepository : BaseRepository<Maitsmine>, IMaitsmineRepository
    {
        public MaitsmineRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
