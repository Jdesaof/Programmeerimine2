namespace KooliProjekt.WindowsForms.Api;
public interface IApiClient
{
    Task<OperationResult<PagedResult<Olu>>> List(int page, int pageSize, CancellationToken token = default);
    Task<OperationResult<Olu>> Save(Olu item, CancellationToken token = default);
    Task<OperationResult> Delete(int id, CancellationToken token = default);
}

