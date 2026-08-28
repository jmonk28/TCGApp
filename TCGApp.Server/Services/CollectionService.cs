namespace TCGApp.Server.Service;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using TCGApp.Server.Data;
using TCGApp.Server.Models;
using TCGApp.Server.Utilities;

public class CollectionService
{

    public readonly TCGAppContext dbContext;
    public CollectionService(TCGAppContext context)
    {
        dbContext = context;
    }

    public int GenerateCollectionID()
    {
        return (dbContext.Collection.Any() ? dbContext.Collection.Max(c => c.CollectionID) : 0) + 1;
    }

    public async void AddCollection(Collection newCollection)
    {
        await dbContext.Collection.AddAsync(newCollection);
        dbContext.SaveChanges();
    }

    public async Task<bool> CollectionNameExists(string collectionName, int userID)
    {
        return await dbContext.Collection.AnyAsync(c => c.CollectionName == collectionName && c.TCGUserID == userID);
    }

    public async Task<List<CollectionWithCounts>> GetCollections(TCGUser user)
    {
        var collections = await dbContext.CollectionWithCounts.Where(c => c.TCGUserID == user.UserID)
        .Select(c => new CollectionWithCounts
        {
            CollectionID = c.CollectionID,
            CollectionName = c.CollectionName,
            CollectionType = c.CollectionType,
            CardCount = c.CardCount,
            IsBase = c.IsBase,
            TCGUserID = c.TCGUserID
        })
        .ToListAsync();
        return collections;
    }
}

    