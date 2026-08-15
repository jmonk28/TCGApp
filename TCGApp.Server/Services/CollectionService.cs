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

    public async Task<List<Collection>> GetCollections(TCGUser user)
    {
        var collections = await dbContext.Collection.Where(c => c.TCGUserID == user.UserID)
        .Select(c => new Collection
        {
            CollectionID = c.CollectionID,
            CollectionName = c.CollectionName,
            CardCount = c.CardCount,
            CollectionType = c.CollectionType,
            IsBase = c.IsBase
        })
        .ToListAsync();
        return collections;
    }
}

    