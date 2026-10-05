namespace KooliProjekt.BlazorWasm.Api;
public interface IApiClient
{
    Task<OperationResult<Olu>> Get(int id, CancellationToken token = default);
    Task<OperationResult<PagedResult<Olu>>> List(int page, int pageSize, CancellationToken token = default);
    Task<OperationResult<Olu>> Save(Olu item, CancellationToken token = default);
    Task<OperationResult> Delete(int id, CancellationToken token = default);
}

