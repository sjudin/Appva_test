using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

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

    public class DateItem
    {
        public DateItem()
        {
            TodoItems = new List<TodoItem>();
        }

        public int DateItemId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        // Foreign key storage
        public List<TodoItem> TodoItems { get; set; }
    }
}
