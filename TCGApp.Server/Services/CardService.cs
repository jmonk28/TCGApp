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

public class CardService
{

    public readonly TCGAppContext dbContext;
    public CardService(TCGAppContext context)
    {
        dbContext = context;
    }

    public int GenerateCollectionCardID()
    {
        return (dbContext.CollectionCard.Any() ? dbContext.CollectionCard.Max(c => c.CollectionCardID) : 0) + 1;
    }

    public async void AddCollectionCard(CollectionCard newCollectionCard)
    {
        await dbContext.CollectionCard.AddAsync(newCollectionCard);
        dbContext.SaveChanges();
    }

    public async Task<List<CollectionCard>> GetCardsInCollection(int collectionID)
    {
        var collectionCards = await dbContext.CollectionCard.Where(cc => cc.CollectionCardID == collectionID)
        .Select(cc => new CollectionCard
        {
            CollectionCardID = cc.CollectionCardID,
            CollectionID = cc.CollectionID,
            CardID = cc.CardID
        })
        .ToListAsync();
        return collectionCards;
    }
}

