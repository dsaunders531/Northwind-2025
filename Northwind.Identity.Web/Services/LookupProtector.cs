using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;

namespace Northwind.Identity.Web.Services
{
    /// <summary>
    /// Create a lookup protector which uses the data protection api (configured in startup)
    /// </summary>
    public class LookupProtector : ILookupProtector
    {
        public LookupProtector(IDataProtectionProvider dataProtectionProvider)
        {
            DataProtector = dataProtectionProvider.CreateProtector("LookupProtector");
        }

        private IDataProtector DataProtector { get; set; }

        public string Protect(string keyId, string? data)
        {
            return string.IsNullOrEmpty(data) ? string.Empty : DataProtector.Protect(data);
        }

        public string Unprotect(string keyId, string? data)
        {

            return string.IsNullOrEmpty(data) ? string.Empty : DataProtector.Unprotect(data);
        }
    }
}
