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
using TCGApp.Server.DTO;
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
            CardID = cc.CardID,
            TCGUserID = cc.TCGUserID
        })
        .ToListAsync();
        return collectionCards;
    }

    public async Task<List<Card>> GetFirstTenDatabaseCards()
    {
        var dbCards = await dbContext.Card.OrderBy(c => c.CardID).Take(10).ToListAsync();
        return dbCards;
    }

    public async Task<List<CardWithCount>> GetDatabaseCardsFromCollectionCards(List<CollectionCard> collectionCardList)
    {
        //First, group all collection cards by respective card ID
        var grouped = collectionCardList.GroupBy(cc => cc.CardID).Select(g => new { CardID = g.Key, Count = g.Count() }).ToList();
        //Make a list of just the card IDs from above
        var cardIDs = grouped.Select(g => g.CardID).ToList();
        //Grab associated database cards
        var cards = await dbContext.Card.Where(c => cardIDs.Contains(c.CardID)).ToListAsync();
        //Merge card information with counts for each card
        var databaseCards = cards.Select(card => new CardWithCount
        {
            CardID = card.CardID,
            CardName = card.CardName,
            CardDescription = card.CardDescription,
            CardGame = card.CardGame,
            Rarity = card.Rarity,
            CardSet = card.CardSet,
            Price = card.Price,
            SpecialProperties = card.SpecialProperties,
            InGameProperties = card.InGameProperties,
            Image = card.Image,
            CardType = card.CardType,
            CardCount = grouped.First(g => g.CardID == card.CardID).Count
        }).ToList();
        return databaseCards;
    }
}

