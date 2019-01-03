#region Using

using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

#endregion

namespace SmartAdminMvc
{
    public class UserManager : UserManager<IdentityUser>
    {
        private static readonly UserStore<IdentityUser> UserStore = new UserStore<IdentityUser>();
        private static readonly UserManager Instance = new UserManager();

        private UserManager()
            : base(UserStore)
        {
        }

        public static UserManager Create()
        {
            // We have to create our own user manager in order to override some default behavior:
            //
            //  - Override default password length requirement (6) with a length of 4
            //  - Override user name requirements to allow spaces and dots
            var passwordValidator = new MinimumLengthValidator(4);
            var userValidator = new UserValidator<IdentityUser, string>(Instance)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };

            Instance.UserValidator = userValidator;
            Instance.PasswordValidator = passwordValidator;

            return Instance;
        }

        public static void Seed()
        {
            // Make sure we always have at least the demo user available to login with
            // this ensures the user does not have to explicitly register upon first use
            var demo = new IdentityUser
            {
                Id = "6bc8cee0-a03e-430b-9711-420ab0d6a596",
                Email = "demo@email.com",
                UserName = "John Doe",
                PasswordHash = "APc6/pVPfTnpG89SRacXjlT+sRz+JQnZROws0WmCA20+axszJnmxbRulHtDXhiYEuQ==",
                SecurityStamp = "18272ba5-bf6a-48a7-8116-3ac34dbb7f38"
            };

            UserStore.Context.Set<IdentityUser>().AddOrUpdate(demo);
            UserStore.Context.SaveChanges();
        }
    }
}