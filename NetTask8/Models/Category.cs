using System.ComponentModel.DataAnnotations;

namespace NetTask8.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
