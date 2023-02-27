using CompService.Core.Messages;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Results;

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

        public async Task<BaseResult> RegistrateAsync(string name, string email, string? phoneNumber)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            };
            await _userService.CreateUserAsync(user);
            return new BaseResult {Success = true};
        }

        public async Task<BaseResult> CreateAuthorizeCodeAsync(string? email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user is null)
            {
                return new BaseResult {Success = false, Message = UserMessages.UserNotFound};
            }

            var rnd = new Random();
            var verification = new UserVerification
            {
                IsActual = true,
                Code = rnd.Next(0, 1000000).ToString("D6"),
                ExpyreTime = DateTime.UtcNow.AddHours(2),
                User = user
            };
            await _verificationRepository.CreateVerification(verification);
            await _emailService.SendEmailAsync(email, MailSubjects.AuthMailSubject,
                $"{MailMessages.AuthMailMessage}{verification.Code}");
            return new BaseResult{Success = true};
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
                await _verificationRepository.ChangeVerification(res.VerificationId);
                return null;
            }

            await _verificationRepository.ChangeVerification(res.VerificationId);
            return await _userService.GetUserByEmailAsync(email);
        }
    }
}