using CompService.Core.Enums;
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
        private readonly UserInfoHolder _userInfoHolder;

        public AuthorizationService(IUserService userService, IVerificationRepository verificationRepository,
            IEmailService emailService, UserInfoHolder userInfoHolder)
        {
            _userService = userService;
            _verificationRepository = verificationRepository;
            _emailService = emailService;
            _userInfoHolder = userInfoHolder;
        }

        public async Task<BaseResult> RegistrateAsync(string name, string surname, string patronymic,
            string email, string? phoneNumber, Roles roles)
        {
            var user = new User
            {
                Surname = surname,
                Patronymic = patronymic,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Roles = roles
            };

            var dbUser = await _userService.GetUserByEmailAsync(user.Email);

            if (dbUser is not null)
            {
                return new BaseResult {Success = false, Message = UserMessages.UserAlreadyExisting};
            }
            
            await _userService.CreateUserAsync(user);
            return new BaseResult {Success = true};
        }

        /// <summary>
        /// Create a verify code for user, and send to user mailbox
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
            var sendResult = await _emailService.SendEmailAsync(email, MailSubjects.AuthMailSubject,
                $"{MailMessages.AuthMailMessage}{verification.Code}");
            return sendResult.Success
                ? new BaseResult {Success = true} 
                : new BaseResult {Success = false, Message = sendResult.Message};
        }

        public async Task<AuthorizationResult> AuthorizeWithCodeAsync(string email, string code)
        {
            var res = await _verificationRepository.VerifyUser(email);
            if (!Equals(res?.Code, code))
            {
                return new AuthorizationResult {Success = false, Message = UserMessages.WrongUserVerifyCode};
                ;
            }

            if (!(res.ExpyreTime < DateTime.UtcNow))
            {
                await _verificationRepository.ChangeVerification(res.VerificationId);
                return new AuthorizationResult {Success = false, Message = UserMessages.UserVerifyCodeExpire};
            }

            await _verificationRepository.ChangeVerification(res.VerificationId);
            _userInfoHolder.CurrentUser = res.User;

            return new AuthorizationResult {User = await _userService.GetUserByEmailAsync(email), Success = true};
        }

        public async Task<AuthorizationResult> AuthorizeWithPassword(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user is null)
                return new AuthorizationResult {Success = false, Message = UserMessages.UserNotFound};

            if (!Equals(user.Password, password))
                return new AuthorizationResult {Success = false, Message = UserMessages.WrongUserPassword};

            _userInfoHolder.CurrentUser = user;
            return new AuthorizationResult {User = user, Success = true};
        }
    }
}