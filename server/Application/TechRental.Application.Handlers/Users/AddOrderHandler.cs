﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using TechRental.Application.Common.Exceptions;
using TechRental.DataAccess.Abstractions;
using TechRental.Domain.Core.Orders;
using TechRental.Domain.Core.Users;
using static TechRental.Application.Contracts.Users.Commands.AddOrder;

namespace TechRental.Application.Handlers.Users;

internal class AddOrderHandler : IRequestHandler<Command>
{
    private readonly IDatabaseContext _context;

    public AddOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.UserId), cancellationToken);
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id.Equals(request.OrderId), cancellationToken);

        if (user is null)
            throw EntityNotFoundException.For<User>(request.UserId);

        if (order is null)
            throw EntityNotFoundException.For<Order>(request.OrderId);

        user.AddOrder(order);

        await _context.SaveChangesAsync(cancellationToken);
    }
}