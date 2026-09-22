namespace KooliProjekt.WindowsForms;

public class PagedResult<T> : PagedResultBase
{
    public List<T> Results { get; set; } = new();
}

