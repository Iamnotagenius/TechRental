﻿using TechRental.Application.Dto.Orders;

namespace TechRental.Application.Dto.Users;

public record UserDto(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    byte[] Image,
    DateTime BirthDate,
    string Number,
    IEnumerable<UserOrderDto> Orders);