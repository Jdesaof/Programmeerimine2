namespace KooliProjekt.Application.Data.Repositories
{
    public class OluRepository : BaseRepository<Olu>, IOluRepository
    {
        public OluRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
