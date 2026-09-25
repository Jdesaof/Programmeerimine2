namespace KooliProjekt.WindowsForms.Api;
public interface IApiClient
{
    Task<PagedResult<Olu>> List(int page, int pageSize, CancellationToken token = default);
    Task<Olu> Save(Olu item, CancellationToken token = default);
    Task Delete(int id, CancellationToken token = default);
}

