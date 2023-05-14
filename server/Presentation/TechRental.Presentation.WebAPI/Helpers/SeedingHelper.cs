﻿using MediatR;
using TechRental.Application.Abstractions.Identity;
using TechRental.Application.Contracts.Identity.Commands;
using TechRental.Presentation.Contracts.Configuration;

namespace TechRental.Presentation.WebAPI.Helpers;

internal static class SeedingHelper
{
    internal static async Task SeedAdmins(IServiceProvider provider, IConfiguration configuration)
    {
        var mediator = provider.GetRequiredService<IMediator>();
        var logger = provider.GetRequiredService<ILogger<Program>>();
        IConfigurationSection adminsSection = configuration.GetSection("Identity:DefaultAdmins");
        var admins = adminsSection.Get<AdminModel[]>() ?? Array.Empty<AdminModel>();

        foreach (var admin in admins)
        {
            try
            {
                var registerCommand = new CreateAdmin.Command(admin.Username, admin.Password);
                await mediator.Send(registerCommand);

                var changeRole = new ChangeUserRole.Command(admin.Username, TechRentalIdentityRoleNames.AdminRoleName);
                await mediator.Send(changeRole);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to register admin {Username}", admin.Username);
            }
        }
    }
}