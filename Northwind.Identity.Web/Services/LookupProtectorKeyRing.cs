using Microsoft.AspNetCore.Identity;

namespace Northwind.Identity.Web.Services
{
    /// <summary>
    /// This key ring does nothing as the lookup protector uses the data protection api.
    /// </summary>
    public class LookupProtectorKeyRing : ILookupProtectorKeyRing
    {
        public string this[string keyId] => Guid.NewGuid().ToString();

        public string CurrentKeyId => Guid.NewGuid().ToString();

        public IEnumerable<string> GetAllKeyIds()
        {
            return new string[] { Guid.NewGuid().ToString() };
        }
    }
}
