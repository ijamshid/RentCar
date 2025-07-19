﻿using Microsoft.EntityFrameworkCore;
using RentCar.DataAccess.Persistence;

public static class AutomatedMigration
{
    public static async Task MigrateAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<DatabaseContext>();

        if (context.Database.IsNpgsql())
            await context.Database.MigrateAsync();
    }
}
