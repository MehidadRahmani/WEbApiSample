using Entities.Owin;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Contract.Account
{
    public interface IAccountService
    {

        #region [-Jwt-]
        public Task<string> GenerateTokenAsync(User user);
       

      
     
        #endregion

        #region [-Register-]
        public  Task<bool> UserNameExistAysnc(string username);
       
        public Task<IdentityResult> Register(User user, string password, CancellationToken cancellationToken);
       
        #endregion

        #region [-Login-]
        public  Task<SignInResult> CheckUserAndPassword(string userName, string password, bool RememberMe);
       
        
        #endregion

        #region [-UserManager-]
        public Task<List<User>> GetUsers(CancellationToken cancellationToken);
        

        public Task<User> GetUser(int id, CancellationToken cancellationToken);
    
        #endregion

    }
}
