﻿using Common.Exceptions;
using Common.Utilities;
using Data.Contract.Owin;
using Data.DBContext;
using Data.Repositories.Common;
using Entities.Owin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories.Owin
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            return Table.Where(p => p.UserName == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);
        }

        public Task UpdateSecuirtyStampAsync(User user, CancellationToken cancellationToken)
        {
          
            return UpdateAsync(user, cancellationToken);
        }

    

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            return UpdateAsync(user, cancellationToken);
        }

        public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            var exists = await TableNoTracking.AnyAsync(p => p.UserName == user.UserName);
            if (exists)
                throw new BadRequestException("نام کاربری تکراری است");

            var passwordHash = SecurityHelper.GetSha256Hash(password);
            user.PasswordHash = passwordHash;
            await base.AddAsync(user, cancellationToken);
        }
    }
}
