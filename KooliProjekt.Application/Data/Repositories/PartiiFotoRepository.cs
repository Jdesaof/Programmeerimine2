namespace KooliProjekt.Application.Data.Repositories
{
    public class PartiiFotoRepository : BaseRepository<PartiiFoto>, IPartiiFotoRepository
    {
        public PartiiFotoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
