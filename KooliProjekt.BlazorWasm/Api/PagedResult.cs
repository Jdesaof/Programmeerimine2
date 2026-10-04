namespace KooliProjekt.BlazorWasm;

public class PagedResult<T> : PagedResultBase
{
    public List<T> Results { get; set; } = new();
}

