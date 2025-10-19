using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class UsersRepository
{
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;

    public UsersRepository(AppDbContext context, PasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    public async Task<User?> CreateUserAsync(string username, string email, string password)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
        {
            return null;
        }

        var hashedPassword = _passwordService.HashPassword(password);

        var newUser = new User
        {
            Username = username,
            Email = email,
            Password = hashedPassword,

        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    public async Task<User?> GetUserAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<User?> GetUserInfoAsync(int id)
    {
        return await _context.Users
        .Include(u => u.UserBloodPanel)
        .Include(u => u.UserLipidPanel)
        .Include(u => u.MetabolicPanel)
        .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> UpdateUserVerificationStatusAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;


        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> UpdateUserAsync(
        int userId,
        string? username = null,
        string? email = null,
        string? password = null

    )
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(username))
                user.Username = username;

            if (!string.IsNullOrWhiteSpace(email))
                user.Email = email;

            if (!string.IsNullOrWhiteSpace(password))
                user.Password = _passwordService.HashPassword(password);



            await _context.SaveChangesAsync();
            return user;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> ClearPasswordResetTokenAsync(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (existingUser == null)
        {
            return false;
        }


        await _context.SaveChangesAsync();
        return true;
    }
}
