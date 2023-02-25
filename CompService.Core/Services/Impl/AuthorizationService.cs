using CompService.Core.Models;
using CompService.Core.Repositories;

namespace CompService.Core.Services.Impl
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService _userService;
        private readonly IVerificationRepository _verificationRepository;
        private readonly IEmailService _emailService;

        public AuthorizationService(IUserService userService, IVerificationRepository verificationRepository,
            IEmailService emailService)
        {
            _userService = userService;
            _verificationRepository = verificationRepository;
            _emailService = emailService;
        }

        public async Task<bool> RegistrateAsync(string name, string email, string? phoneNumber)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            };
            await _userService.CreateUserAsync(user);
            return true;
        }

        public async Task<bool> CreateAuthorizeCodeAsync(string? email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user is null)
            {
                return false;
            }

            var rnd = new Random();
            var verification = new UserVerification
            {
                IsActual = true,
                Code = rnd.Next(0, 1000000).ToString("D6"),
                ExpyreTime = DateTime.UtcNow.AddHours(2),
                UserId = user.UserId
            };
            await _verificationRepository.CreateVerification(verification);
            await _emailService.SendEmailAsync(email, "Код подтверждения", $"Код подтверждения для" +
            $"авторизации в приложении - {verification.Code}. Если вы ничего такого не делали, то проигнорируйте письмо");
            return true;
        }

        public async Task<User?> AuthorizeAsync(string email, string code)
        {
            var res = await _verificationRepository.VerificateUser(email);
            if (!Equals(res?.Code, code))
            {
                return null;
            }

            if (!(res.ExpyreTime < DateTime.UtcNow))
            {
                return null;
            }

            await _verificationRepository.ChangeVerification(res.Id);
            return await _userService.GetUserByEmailAsync(email);
        }
    }
}