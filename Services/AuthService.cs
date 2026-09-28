using gerenciador_de_tarefas.Data;
using gerenciador_de_tarefas.DTOs.UsuarioDTOs;
using gerenciador_de_tarefas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace gerenciador_de_tarefas.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            if (await _context.Usuarios.AnyAsync(u => u.Username == request.Username))
                return new LoginResponse { Sucesso = false, Mensagem = "Username já existe" };

            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
                return new LoginResponse { Sucesso = false, Mensagem = "Email já existe" };

            if (request.Password != request.ConfirmarPassword)
                return new LoginResponse { Sucesso = false, Mensagem = "Senhas não conferem" };

            var usuario = new Usuario
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(usuario);

            return new LoginResponse
            {
                Sucesso = true,
                Mensagem = "Registro realizado com sucesso!",
                Token = token,
                Usuario = new UsuarioDto { Id = usuario.Id, Username = usuario.Username, Email = usuario.Email }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao registrar: {ex.Message}");
            return new LoginResponse { Sucesso = false, Mensagem = "Erro ao registrar" };
        }
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (usuario == null || !VerifyPassword(request.Password, usuario.PasswordHash))
                return new LoginResponse { Sucesso = false, Mensagem = "Username ou senha inválidos" };

            if (!usuario.Ativo)
                return new LoginResponse { Sucesso = false, Mensagem = "Usuário inativo" };

            var token = GenerateJwtToken(usuario);

            return new LoginResponse
            {
                Sucesso = true,
                Mensagem = "Login realizado com sucesso!",
                Token = token,
                Usuario = new UsuarioDto { Id = usuario.Id, Username = usuario.Username, Email = usuario.Email }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao fazer login: {ex.Message}");
            return new LoginResponse { Sucesso = false, Mensagem = "Erro ao fazer login" };
        }
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var secretKey = _configuration["Jwt:SecretKey"] ?? "";
        var issuer = _configuration["Jwt:Issuer"] ?? "";
        var audience = _configuration["Jwt:Audience"] ?? "";
        var expirationMinutesStr = _configuration["Jwt:ExpirationMinutes"] ?? "60";

        int.TryParse(expirationMinutesStr, out int expirationMinutes);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, usuario.Username),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, usuario.Email)
            };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}
