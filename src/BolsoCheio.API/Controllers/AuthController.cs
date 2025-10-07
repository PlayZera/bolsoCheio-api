using BolsoCheio.Business.DTOs;
using BolsoCheio.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BolsoCheio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Realizar login no sistema
        /// </summary>
        /// <param name="loginRequest">Dados de login (email e senha)</param>
        /// <returns>Token JWT e dados do usuário</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginRequest);
            
            if (result == null)
                return Unauthorized(new { message = "Email ou senha inválidos" });

            return Ok(result);
        }

        /// <summary>
        /// Registrar novo usuário
        /// </summary>
        /// <param name="registerRequest">Dados para criação da conta</param>
        /// <returns>Token JWT e dados do usuário criado</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<LoginResponseDto>> Register([FromBody] RegisterRequestDto registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerRequest);
            
            if (result == null)
                return Conflict(new { message = "E-mail já cadastrado" });

            return CreatedAtAction(nameof(GetProfile), new { }, result);
        }

        /// <summary>
        /// Obter perfil do usuário autenticado
        /// </summary>
        /// <returns>Dados do perfil do usuário</returns>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var user = await _authService.GetUserProfileAsync(userId.Value);
            
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(user);
        }

        /// <summary>
        /// Atualizar perfil do usuário autenticado
        /// </summary>
        /// <param name="updateProfile">Dados a serem atualizados</param>
        /// <returns>Dados atualizados do usuário</returns>
        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UpdateProfile([FromBody] UpdateProfileDto updateProfile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var user = await _authService.UpdateProfileAsync(userId.Value, updateProfile);
            
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(user);
        }

        /// <summary>
        /// Alterar senha do usuário autenticado
        /// </summary>
        /// <param name="changePassword">Dados para alteração de senha</param>
        /// <returns>Confirmação da alteração</returns>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var success = await _authService.ChangePasswordAsync(userId.Value, changePassword);
            
            if (!success)
                return BadRequest(new { message = "Senha atual incorreta" });

            return Ok(new { message = "Senha alterada com sucesso" });
        }

        /// <summary>
        /// Validar token JWT
        /// </summary>
        /// <returns>Status de validação do token</returns>
        [HttpPost("validate-token")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ValidateToken()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            return Ok(new { message = "Token válido", userId = userId.Value });
        }

        /// <summary>
        /// Logout (invalidar token no lado cliente)
        /// </summary>
        /// <returns>Confirmação de logout</returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            // Como estamos usando JWT stateless, o logout é feito no cliente
            // removendo o token do storage local
            return Ok(new { message = "Logout realizado com sucesso" });
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }
}