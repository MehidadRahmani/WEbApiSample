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



        #region [-Register-]
        public  Task<bool> UserNameExistAysnc(string username);
       
        public Task<IdentityResult> Register(User user, string password);
       
        #endregion

        #region [-Login-]
        public  Task<string> CheckUserAndPassword(string userName, string password, bool RememberMe);
       
        
        #endregion

        #region [-UserManager-]
        public Task<List<User>> GetUsers(CancellationToken cancellationToken);
        

        public Task<User> GetUser(int id, CancellationToken cancellationToken);
        public  Task UpdateUser(User user, CancellationToken cancellationToken);
        public Task DeleteUser(User user, CancellationToken cancellationToken);

        #endregion

    }
}
