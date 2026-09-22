namespace KooliProjekt.WindowsForms;

public class OperationResult
{
    public Dictionary<string, string> PropertyErrors { get; set; }
    public List<string> Errors { get; set; }
    public bool HasErrors => PropertyErrors?.Count > 0 || Errors?.Count > 0;
}

