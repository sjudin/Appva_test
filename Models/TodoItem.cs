using System.ComponentModel.DataAnnotations;

namespace Appva_test.Models
{
    public class TodoItem
    {
        public int TodoItemId { get; set; }

        // Model validations
        [Required]
        public string Name { get; set; }

        public string Desc { get; set; }

        public bool IsDone { get; set; }
    }
}
