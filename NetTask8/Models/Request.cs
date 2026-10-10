using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NetTask8.Data;
using NetTask8.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetTask8.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public string FileNumber { get; set; }
        public string Subject { get; set; }

        public DateTime Date { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; } = StaticDetails.Initiated;

        public bool? Approval1 { get; set; } = null;
        public bool? Approval2 { get; set; } = null;


        public string CreatorId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser? User { get; set; }

        public string? Approval1UserId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(Approval1UserId))]
        public ApplicationUser? Approval1User { get; set; }


        public string? Approval2UserId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(Approval2UserId))]
        public ApplicationUser? Approval2User { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

    }
}
