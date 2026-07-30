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

    public async void AddCollection(Collection newCollection)
    {
        await dbContext.Collection.AddAsync(newCollection);
        dbContext.SaveChanges();
    }
}

    