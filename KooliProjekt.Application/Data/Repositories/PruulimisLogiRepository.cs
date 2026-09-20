namespace KooliProjekt.Application.Data.Repositories
{
    public class PruulimisLogiRepository : BaseRepository<PruulimisLogi>, IPruulimisLogiRepository
    {
        public PruulimisLogiRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
