using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proyecto.Server.Services.Base;
using Proyecto.Shared.Exceptions;
using Proyecto.Shared.Models.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Server.Services.Auth.Command
{
    public class LoginCommand : IRequest<ResponseLogin>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ResponseLogin 
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }

    public class LoginHanlder : IRequestHandler<LoginCommand, ResponseLogin>
    {
        private readonly IRepository<User, int> _repository;
        
        public LoginHanlder(IRepository<User, int> repository)
        {
            this._repository = repository;
        }
        public async Task<ResponseLogin> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.Query().Where(x => x.UserName == request.UserName).FirstOrDefaultAsync();

            if (user == null) throw new BadRequestException("No se encuentra el usuario");

            if (!VerifyPasswordHash(request.Password, user.PasswordHast, user.PasswordSalt)) throw new BadRequestException("Las credenciales no coinciden");

            ResponseLogin response = CreateToken(user);

            return response;
        }

        private ResponseLogin CreateToken(User user) 
        {
            List<Claim> claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var secret = "ñlkjhgfdsaikmujnyhbtgvqazwsxedcrfv";

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var expired = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expired,
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new ResponseLogin { Token = jwt, Expiration = expired };
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) 
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

    }
}
