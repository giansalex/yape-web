using System.ComponentModel.DataAnnotations;

namespace Yape.Api.Models
{
    public class OrderIntent
    {
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
    }
}
