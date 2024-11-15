using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Buffers.Binary;

namespace Roaster.Controllers.Abstractions
{
    public abstract class CustomApiController : ControllerBase
    {
        private readonly IDataProtector _protector;

        protected CustomApiController(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector(nameof(RoastsController));
        }

        protected string ProtectId(int id)
        {
            var bytes = BitConverter.GetBytes(id);
            var protectedId = _protector.Protect(bytes);

            return WebEncoders.Base64UrlEncode(protectedId);
        }

        protected int UnprotectId(string protectedId)
        {
            var decoded = WebEncoders.Base64UrlDecode(protectedId);
            var bytes = _protector.Unprotect(decoded);

            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
