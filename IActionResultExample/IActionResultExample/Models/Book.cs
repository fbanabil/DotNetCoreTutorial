using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Models
{
    public class Book
    {
        [FromRoute]
        public int? BookId { get; set; }
        [FromQuery]
        public string? Author { get; set; }

        public override string ToString()
        {
            return $"Book object - Book ID: {BookId}, Author: {Author}";
        }
    }
}
