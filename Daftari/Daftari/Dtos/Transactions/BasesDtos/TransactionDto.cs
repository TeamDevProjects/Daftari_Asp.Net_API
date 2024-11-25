using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Daftari.Dtos.Transactions.BasesDtos
{
    public class TransactionDto
    {

        public decimal Amount { get; set; }

        public string? Notes { get; set; }

        public byte[]? ImageData { get; set; }

        public string? ImageType { get; set; }

        [NotMapped]
        public IFormFile? FormImage { get; set; }  // New property for image upload
    }
}
