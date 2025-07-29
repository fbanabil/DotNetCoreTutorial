namespace PartialViewsExample.Models
{
    public class ListModel
    {
        public string? ListTitle { get; set; }
        public List<string>? Cities { get; set; } = new List<string>();
    }
}
