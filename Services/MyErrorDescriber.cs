using Microsoft.AspNetCore.Identity;

namespace Wallaboo.Services
{
    public class MyErrorDescriber: IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            //return base.DuplicateEmail(email);
            return new IdentityError()
            {
                Code = nameof(DuplicateEmail),
                Description = "El mail ya se encuentra registrado"
            };
        }
    }
}
