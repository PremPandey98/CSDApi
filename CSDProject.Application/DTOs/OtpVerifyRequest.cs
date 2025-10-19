using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDProject.Application.DTOs
{
    public class OtpVerifyRequest
    {
        public string Email { get; set; } = string.Empty;
        public int OtpCode { get; set; }
    }
}
