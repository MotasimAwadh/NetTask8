using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NetTask8.Models
{
    public class RequestCategoryViewModel
    {
        public int RequestId { get; set; }

        public string FileNumber { get; set; }

        public string Subject { get; set; }
        public int? CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
