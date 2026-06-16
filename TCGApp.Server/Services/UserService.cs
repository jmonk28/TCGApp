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

    public async Task<bool> PerformLogin(string email, string passwordHash)
    {
        var user = await dbContext.TCGUser.FirstOrDefaultAsync(u => (u.Email == email && u.PasswordHash == passwordHash));
        if (user == null) return false;

        user.LastLogin = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        return true;

    }
}