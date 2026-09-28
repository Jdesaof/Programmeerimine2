namespace KooliProjekt.WpfApplication;

public class PagedResult<T> : PagedResultBase
{
    public List<T> Results { get; set; } = new();
}

