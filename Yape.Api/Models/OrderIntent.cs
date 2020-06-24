using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Yape.Api.Models
{
    public class OrderIntent
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Range(1, 20)]
        public decimal Amount { get; set; }
        [BindNever]
        public string Id { get; set; }
        [BindNever]
        public string State { get; set; }
    }
}
