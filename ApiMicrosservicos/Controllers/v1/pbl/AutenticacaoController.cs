using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Livraria.Api.ObjectModel;
using Livraria.Api.ObjectModel.Swagger.Examples;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Examples;

namespace Livraria.Api.Controllers.v1.pbl
{
    [Route("v1/public/[controller]")]
    public class AutenticacaoController : Controller
    {
        private IConfiguration _config;

        public AutenticacaoController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [SwaggerRequestExample(typeof(Login), typeof(LoginExample))]
        public IActionResult CriarToken([FromBody]Login login)
        {
            IActionResult response = Unauthorized();
            var user = AutenticarUsuario(login);

            if (user != null)
            {
                var tokenString = ConstruirToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string ConstruirToken(UsuarioLogin user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Nome),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.AuthTime, user.AuthTime.ToString(CultureInfo.InvariantCulture))
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                null,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UsuarioLogin AutenticarUsuario(Login login)
        {
            UsuarioLogin usuarioLogin = null;

            if (login.Usuario == "otavio" && login.Senha == "12345")
            {
                usuarioLogin = new UsuarioLogin { Nome = "Otávio Augusto", Email = "otavioteste@teste.com", AuthTime = DateTime.Now};
            }
            if (login.Usuario == "matheus" && login.Senha == "12345")
            {
                usuarioLogin = new UsuarioLogin { Nome = "Matheus", Email = "matheusteste@teste.com", AuthTime = DateTime.Now };
            }
            return usuarioLogin;
        }
    }
}