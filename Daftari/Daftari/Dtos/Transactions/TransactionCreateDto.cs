using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Daftari.Dtos.Transactions
{
    public class TransactionCreateDto
    {

        public byte TransactionTypeId { get; set; } // 1 => Payment , 2 => Withdrawal

       // public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public decimal Amount { get; set; }

        public string? Notes { get; set; }

        public byte[]? ImageData { get; set; } 

        public string? ImageType { get; set; }

        [NotMapped]
        public IFormFile? FormImage { get; set; }  // New property for image upload
    }
}
