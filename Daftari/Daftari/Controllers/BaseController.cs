using Daftari.Data;
using Microsoft.AspNetCore.Mvc;

namespace Daftari.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly DaftariContext _context;

        public BaseController(DaftariContext context)
        {
            _context = context;
        }

        protected int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return -1; // Indicate failure to find or parse user ID.
            }

            if (int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId; // Return parsed user ID.
            }

            return -1; // Indicate failure to parse user ID.
        }



    }
}
