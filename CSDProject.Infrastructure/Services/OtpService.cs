using CSDProject.Infrastructure.ScaffoldedModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDProject.Infrastructure.Services
{
    public class OtpService
    {
        private readonly DbAbe381CsddbContext _db;

        public OtpService(DbAbe381CsddbContext db)
        {
            _db = db;
        }

        public async Task<bool> VerifyOtpAsync(string email, int otp)
        {
            var record = await _db.CsdEmailValidations
                .FirstOrDefaultAsync(x => x.Email == email && x.Otp == otp && x.OtpStatus == "0");

            if (record == null || record.ExpiryTime < DateTime.UtcNow)
                return false;

            record.OtpStatus = "1"; // mark as used
            await _db.SaveChangesAsync();
            return true;
        }
    }

}
