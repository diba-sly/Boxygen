﻿using MassTransit;
using Profile.Domain.Entities;
using Profile.Domain.Repositories;
using Shared.Commands;
using Shared.Events;

namespace Profile.Application.Consumers;

public class CreateUserProfileConsumer : IConsumer<CreateProfile>
{
    private readonly IBus _bus;
    private readonly IUserProfileRepository _userProfileRepository;

    public CreateUserProfileConsumer(IBus bus, IUserProfileRepository userProfileRepository)
    {
        _bus = bus;
        _userProfileRepository = userProfileRepository;
    }

    public async Task Consume(ConsumeContext<CreateProfile> context)
    {
        var userProfile = await _userProfileRepository.Create(new UserProfile
        {
            Id = context.Message.CorrelationId,
            CreatedAt = DateTime.Now
        });

        await context.RespondAsync<ProfileCreated>(new
        {
            CorrelationId = userProfile.Id,
            userProfile.CreatedAt
        });
    }
}