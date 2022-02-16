using Microsoft.AspNetCore.Identity;

namespace AgenciaCronosApi
{
    public class ApplicationUser : IdentityUser
    {
        public string Cpf { get; set; }
    }
}
