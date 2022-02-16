using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AgenciaCronosApi.Context;
using AgenciaCronosApi.Models;

namespace AgenciaCronosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly AccountContext _contextAccount;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IOptions<AppSettings> appSettings,
                              DataContext context, AccountContext contextAccount)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _context = context;
            _contextAccount = contextAccount;
        }

        //REGISTRAR O USUÁRIO
        [HttpPost("register")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var user = new ApplicationUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
                Cpf = registerUser.Cpf
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, false);

            return Ok(await GerarJwt(registerUser.Email));
        }

        //AUTENTICAÇÃO
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel viewmodel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var result = await _signInManager.PasswordSignInAsync(viewmodel.Email, viewmodel.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(await GerarJwt(viewmodel.Email));
            }

            return BadRequest("Usuário ou senha inválidos");
        }

        //ALTERAÇÃO DE SENHA
        [HttpPost("changepassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var user = await _userManager.FindByEmailAsync(changePassword.Email);
            if (user == null)
            {
                return NotFound("Usuário não cadastrado");
            }

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, changePassword.Password);

            if (passwordChangeResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return Ok(await GerarJwt(changePassword.Email));
            }
            else
            {
                return BadRequest("Não foi possível completar a solicitação");
            }
        }

        //GERAÇÃO DO TOKEN
        private async Task<string> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(await _userManager.GetClaimsAsync(user));

            // Sucesso na autenticação
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            string Token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            string cpf = user.Cpf;

            RetornoRegister bm = new RetornoRegister();
            bm.Token = Token;
            bm.Email = email;
          
            string json = JsonConvert.SerializeObject(bm);

            return json;
        }
    }
}
