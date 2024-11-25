using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;

namespace Daftari.Repositories
{
	public class PaymentDateRepository : Repository<PaymentDate>, IPaymentDateRepository
	{
		public PaymentDateRepository(DaftariContext context) : base(context) { }
	}
}
