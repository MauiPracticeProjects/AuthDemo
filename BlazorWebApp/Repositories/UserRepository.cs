using BlazorWebApp.Interface;
using BlazorWebApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        private string connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");

        }

        public async Task<ApiResponse> UserRegistration(UserModel model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = "INSERT INTO dbo.Users (Username, Email, PasswordHash, CreatedAt) VALUES (@Username, @Email, @PasswordHash, @CreatedAt)";
                var result = await connection.ExecuteAsync(query, model);
                if (result > 0)
                {
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        Message = "User registered successfully",
                        StatusCode = StatusCodes.Status201Created
                    };
                }
                else
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "User registration failed",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
            }


        }

        public async Task<ApiResponse> LoginUser(UserLoginModel model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT PasswordHash FROM dbo.Users WHERE Email = @Email";
                var result = await connection.QueryFirstOrDefaultAsync<UserModel>(query, new { Email = model.Email });
                if (result != null)
                {
                    var token = GenerateJwtToken(model);
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        Message = "User login successful",
                        StatusCode = StatusCodes.Status200OK,
                        Result = new { Token = token }
                    };
                }
                else
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "User login failed",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
            }
        }

        private string GenerateJwtToken(UserLoginModel user)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, user.Email)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = jwtSettings["Issuer"],
                    Audience = jwtSettings["Audience"], // 🔴 Ensure Audience is set here
                    SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
