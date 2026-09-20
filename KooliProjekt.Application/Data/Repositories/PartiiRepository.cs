namespace KooliProjekt.Application.Data.Repositories
{
    public class PartiiRepository : BaseRepository<Partii>, IPartiiRepository
    {
        public PartiiRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
