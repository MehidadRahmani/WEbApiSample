using Common;
using Data.Contract.Owin;
using Entities.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.Account
{
    public class AccountService
    {

        private readonly SiteSettings _siteSetting;
        private readonly SignInManager<User> signInManager;
        public IUserRepository _userRepository { get; }
        private readonly UserManager<User> userManager;
        public AccountService(IOptionsSnapshot<SiteSettings> settings, UserManager<User> userManager
            , SignInManager<User> signInManager, IUserRepository userRepository
            
            )
        {
            _siteSetting = settings.Value;
            this.signInManager = signInManager;
            _userRepository = userRepository;
            this.userManager = userManager;
        }


        #region [-Jwt-]
        public async Task<string> GenerateTokenAsync(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.Encryptkey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await _getClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.JwtSettings.Issuer,
                Audience = _siteSetting.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };



            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);

            var jwt = tokenHandler.WriteToken(securityToken);

            return jwt;
        }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
        {
            var result = await signInManager.ClaimsFactory.CreateAsync(user);
            //add custom claims
            var list = new List<Claim>(result.Claims);
            list.Add(new Claim(ClaimTypes.MobilePhone, "09121480884"));
            return list;


        }
        #endregion

        #region [-Register-]
        public async Task<bool> UserNameExistAysnc(string username)
        {
            var user= await userManager.FindByNameAsync(username);
            if (user != null) return true; else return false ;
        }
        public async Task<IdentityResult> Register(User user,string password, CancellationToken cancellationToken)
        {

            var result = await userManager.CreateAsync(user, password);
            result = await userManager.AddToRoleAsync(user, "Customer");
           

           
            return result;
        }
        #endregion

        #region [-Login-]
        public async Task<SignInResult> CheckUserAndPassword(string userName,string password, bool RememberMe)
        {
            return await signInManager.PasswordSignInAsync(
                 userName, password, RememberMe, true);

        }
        #endregion

        #region [-UserManager-]
        public async Task<List<User>> GetUsers(CancellationToken cancellationToken)
        {
            return await _userRepository.TableNoTracking.ToListAsync(cancellationToken);
        }

        public async Task<User> GetUser(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetByIdAsync(cancellationToken,id);
        }
        #endregion

    }
}
