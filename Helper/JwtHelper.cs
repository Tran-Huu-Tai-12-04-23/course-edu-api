using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using course_edu_api.Data;
using course_edu_api.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace course_edu_api.Helper;

public class JwtHelper
{
    public static Token GenerateToken(JwtData jwtData, in User user)
    {
        var accessToken = GenerateTokenString(jwtData, user, 15);
        var refreshToken = GenerateTokenString(jwtData, user, 35);
        return new Token(accessToken, refreshToken);
    }

    public static string GenerateTokenString(JwtData jwtData, User user, in int time)
    {
        var key = Encoding.ASCII.GetBytes
            (jwtData.JwtKey);
        
        Console.Write(jwtData.JwtKey);
        
        if (key.Length < 64)
        {
            throw new ArgumentException("Key size must be at least 512 bits for HMACSHA512", nameof(key));
        }
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = jwtData.Issuer,
            Audience = jwtData.Audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
    public static string GenerateHash(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    
    public static bool VerifyPassword(string enteredPassword, string storedPasswordHash)
    {
        byte[] storedBytes = Convert.FromBase64String(storedPasswordHash);
        // Extract the salt from the stored hash
        byte[] salt = storedBytes.Take(128 / 8).ToArray();
        // Hash the entered password with the retrieved salt
        byte[] enteredHash = KeyDerivation.Pbkdf2(
            password: enteredPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);
        // Compare the computed hash with the stored hash
        return enteredHash.SequenceEqual(storedBytes.Skip(128 / 8));
    }
    
    public static string HashPassword(string password)
    {
        // Generate a secure random salt
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        // Hash the password using the generated salt
        byte[] hashed = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);

        // Combine salt and hashed password
        byte[] combined = new byte[salt.Length + hashed.Length];
        salt.CopyTo(combined, 0);
        hashed.CopyTo(combined, salt.Length);

        return Convert.ToBase64String(combined);
    }
    
}
