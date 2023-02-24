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
            await _verificationRepository.CreateVerification(user);
            return true;
        }

        public async Task<User?> AuthorizeAsync(string email, string code)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if(user is null)
            {
                return null;
            }

            return await _verificationRepository.VerificateUser(email, code) ? user : null;
        }
    }
}
