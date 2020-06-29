using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Yape.Api.Models
{
    public class PaymentIntent
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Phone to pay
        /// </summary>
        [Required]
        [RegularExpression(@"^[9]\d{8}$", ErrorMessage = "Invalid peruvian phone.")]
        public string Phone { get; set; }
        /// <summary>
        /// Amount to pay
        /// </summary>
        [Range(1.0, 10.0)]
        public decimal Amount { get; set; }
        [BindNever]
        public string Id { get; set; }
        [BindNever]
        public string State { get; set; }
        [BindNever]
        public DateTime Create { get; set; }
        [BindNever]
        public DateTime? CompleteDate { get; set; }
    }
}
