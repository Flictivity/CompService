using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompService.Core.Models;
using CompService.Core.Repositories;
using CompService.Core.Services;

namespace CompService.Core.Services.Impl
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerificationRepository _verificationRepository;

        public AuthorizationService(IUserRepository userRepository, IVerificationRepository verificationRepository)
        {
            _userRepository = userRepository;
            _verificationRepository = verificationRepository;
        }

        public async Task<bool> RegistrateAsync(string name, string email, string? phoneNumber)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            };
            await _userRepository.CreateUser(user);
            return true;
        }

        public async Task<bool> CreateAuthorizeCodeAsync(string? email)
        {
            User? user = await _userRepository.GetUserByEmail(email);
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
            return true;
        }

        public async Task<User?> AuthorizeAsync(string email, string code)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user is null)
            {
                return null;
            }

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
            return user;
        }
    }
}