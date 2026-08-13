using Microsoft.AspNetCore.Identity;

namespace IdentityMail.Web.CustomValidation
{
    public class CustomErrorDescriber: IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresDigit",
                Description = "Şifre en az bir rakam içermelidir! (0-9)"
            };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresLower",
                Description = "Şifre en az bir küçük harf içermelidir! (a-z)"
            };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresUpper",
                Description = "Şifre en az bir büyük harf içermelidir! (A-Z)"
            };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Şifre en az bir özel karakter (*,/,!,.,? vs.) içermelidir!"
            };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "PasswordTooShort",
                Description = $"Şifre en az {length} karakter içermelidir!"
            };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = "DuplicateUserName",
                Description = $"{userName} kullanıcı adı daha önce alınmış!"
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "DuplicateEmail",
                Description = $"'{email}' e-posta adresi zaten kayıtlıdır. Lütfen farklı bir e-posta adresi kullanın."
            };
        }
    }
}
