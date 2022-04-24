using MediatR;
using Proyecto.Shared.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Proyecto.Server.Services.Base;

namespace Proyecto.Server.Services.Auth.Command
{
    public class UserCreateCommand : IRequest<User>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }


    public class UserCreateHanlder : IRequestHandler<UserCreateCommand, User>
    {
        private readonly IRepository<User, int> _repository;
        public UserCreateHanlder(IRepository<User, int> repository)
        {
            this._repository = repository;
        }
        public async Task<User> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User 
            {
                UserName = request.UserName,
                PasswordHast = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


        }
    }
}
