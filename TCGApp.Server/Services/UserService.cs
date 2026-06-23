namespace TCGApp.Server.Service;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TCGApp.Server.Data;
using TCGApp.Server.Models;
using TCGApp.Server.Utilities;

public class UserService
{

    public readonly TCGAppContext dbContext;
    public UserService(TCGAppContext context)
    {
        dbContext = context;
    }

    public async void CreateUser(TCGUser newUser)
    {
        await dbContext.TCGUser.AddAsync(newUser);
        dbContext.SaveChanges();
    }

    public async Task<TCGUser> GetUserByUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return null;
        return await dbContext.TCGUser.FirstOrDefaultAsync(u => u.Username == username);

    }

    public int GenerateUserID()
    {
        return (dbContext.TCGUser.Any() ? dbContext.TCGUser.Max(u => u.UserID) : 0) + 1;
    }

    public async Task<TCGUser> PerformLogin(string email, string passwordHash)
    {
        var user = await dbContext.TCGUser.FirstOrDefaultAsync(u => (u.Email == email && u.PasswordHash == passwordHash));
        if (user == null) return null;

        user.LastLogin = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        return user;

    }

    public async Task SaveRefreshTokenAsync(int userID, string refreshToken)
    {
        var user = await dbContext.TCGUser.FindAsync(userID);

        user.RefreshTokenHash = new SHA256Hash().ComputeSHA256Hash(refreshToken);
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await dbContext.SaveChangesAsync();
    }

    public async Task<TCGUser> GetUserByRefreshToken(string refreshToken)
    {
        //Hash the refresh token to ensure it matches the hash in the database
        string refreshTokenHash = new SHA256Hash().ComputeSHA256Hash(refreshToken);
        var user = await dbContext.TCGUser.FirstOrDefaultAsync(u => u.RefreshTokenHash == refreshTokenHash);
        return user;
    }

    public async Task<bool> UserLogout(TCGUser user)
    {
        if (user == null) return false;
        user.RefreshTokenHash = null;
        user.RefreshTokenExpiry = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        return true;
    }

}