using Daftari.Data;
using Daftari.Dtos.PaymentDates;
using Daftari.Entities;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services.PaymentDateServices
{
	public class SupplierPaymentDateService:PaymentDateService
	{
		public SupplierPaymentDateService(DaftariContext context) : base(context) { }

		public async Task<SupplierPaymentDate> CreateSupplierPaymentDateAsync(SupplierPaymentDateCreateDto supplierPaymentDateCreateDto)
		{
			if (supplierPaymentDateCreateDto == null)
			{
				throw new ArgumentNullException(nameof(supplierPaymentDateCreateDto));
			}

			try
			{

				// add PaymentDate
				var paymentDate = await CreatePaymentDateService(new PaymentDate
				{
					DateOfPayment = supplierPaymentDateCreateDto.DateOfPayment,
					TotalAmount = supplierPaymentDateCreateDto.TotalAmount,  // calcu
					PaymentMethodId = supplierPaymentDateCreateDto.PaymentMethodId,
					Notes = supplierPaymentDateCreateDto.Notes,
				});


				// add SupplierPaymentDates
				var supplierPaymentDate = new SupplierPaymentDate
				{
					PaymentDateId = paymentDate.PaymentDateId,
					UserId = supplierPaymentDateCreateDto.UserId,
					SupplierId = supplierPaymentDateCreateDto.SupplierId,

				};
				await _context.SupplierPaymentDates.AddAsync(supplierPaymentDate);
				await _context.SaveChangesAsync();


				return supplierPaymentDate;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<SupplierPaymentDate> GetSupplierPaymentDateAsync(int supplierId)
		{
			try
			{
				var SupplierPaymentDate = await _context.SupplierPaymentDates.FirstOrDefaultAsync(x => x.SupplierId == supplierId);

				if (SupplierPaymentDate == null)
				{
					throw new ArgumentNullException(nameof(SupplierPaymentDate));
				}

				return SupplierPaymentDate;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

	}
}
