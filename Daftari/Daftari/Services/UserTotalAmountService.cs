using Daftari.Data;
using Daftari.Entities;
using Daftari.Interfaces;
using Daftari.Repositories;
using Daftari.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Services
{
    public class UserTotalAmountService : IUserTotalAmountService
	{
        private readonly IUserTotalAmountRepository _userTotalAmountRepository;
        public UserTotalAmountService(IUserTotalAmountRepository userTotalAmountRepository)
        {
            _userTotalAmountRepository = userTotalAmountRepository;
        }

        public async Task<decimal> AddAsync(decimal totalAmount, int userId)
        {
            var newUserTotalAmount = new UserTotalAmount
            {
                UpdateAt = DateTime.UtcNow,
                UserId = userId,
                TotalAmount = totalAmount
            };

            var userTotalAmount = await _userTotalAmountRepository.AddAsync(newUserTotalAmount);
            if (userTotalAmount == null) throw new InvalidOperationException("Unable to add userTotalAmount");

            return userTotalAmount.TotalAmount;
        }

        public async Task<decimal> UpdateAsync(UserTotalAmount existUserTotalAmount, decimal Amount, byte TransactionTypeId)
        {
            decimal totalAmount = Amount;

            if (TransactionTypeId == 1)
            {
                totalAmount = existUserTotalAmount.TotalAmount - Amount;

                existUserTotalAmount.TotalAmount = totalAmount;
            }
            else if (TransactionTypeId == 2)
            {
                totalAmount = existUserTotalAmount.TotalAmount + Amount;

                existUserTotalAmount.TotalAmount = totalAmount;
            }

            existUserTotalAmount.UpdateAt = DateTime.UtcNow; // Update timestamp

            var isUpdated = await _userTotalAmountRepository.UpdateAsync(existUserTotalAmount);

            if (!isUpdated) throw new InvalidOperationException("Unable to updated user total amount");

            return totalAmount;
        }


        public async Task<decimal> SaveAsync(byte TransactionTypeId, decimal Amount, int userId)
        {
            decimal totalAmount = Amount;

            var existUserTotalAmount = await _userTotalAmountRepository.GetByUserIdAsync(userId);

            if (existUserTotalAmount == null)
            {
                var userTotalAmountAdded = await AddAsync(totalAmount, userId);

                totalAmount = userTotalAmountAdded;
            }
            else
            {
                totalAmount = await UpdateAsync(existUserTotalAmount, Amount, TransactionTypeId);
            }

            return totalAmount;
        }

        public async Task<UserTotalAmount> GetTotalAmountByUserId(int userId)
        {
            var userTotalAmount = await _userTotalAmountRepository.GetByUserIdAsync(userId);
            if (userTotalAmount == null) throw new KeyNotFoundException("Unable to find userTotalAmount");

            return userTotalAmount;
        }
    }
}
