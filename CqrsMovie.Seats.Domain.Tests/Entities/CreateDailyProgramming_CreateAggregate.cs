﻿using System;
using System.Collections.Generic;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Dtos;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Muflone.Messages.Commands;
using CqrsMovie.Muflone.Messages.Events;
using CqrsMovie.Seats.Domain.CommandHandlers;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.Extensions.Logging;
using Moq;

namespace CqrsMovie.Seats.Domain.Tests.Entities
{
  public class CreateDailyProgramming_CreateAggregate : CommandSpecification<CreateDailyProgramming>
  {
    private readonly DailyProgrammingId aggregateId = new DailyProgrammingId(Guid.NewGuid());
    private readonly MovieId movieId = new MovieId(Guid.NewGuid());
    private readonly ScreenId screenId = new ScreenId(Guid.NewGuid());
    private readonly DateTime dailyDate = DateTime.Today;
    private readonly string movieTitle = "rambo";
    private readonly string screenName = "screen 99";
    private readonly IEnumerable<Seat> seats;
    private readonly Mock<ILoggerFactory> loggerFactory;

    public CreateDailyProgramming_CreateAggregate()
    {
      seats = new List<Seat>() { new Seat() { Number = 1, Row = "A" } };
      loggerFactory = new Mock<ILoggerFactory>();
    }

    protected override IEnumerable<DomainEvent> Given()
    {
      return new List<DomainEvent>();
    }

    protected override CreateDailyProgramming When()
    {
      return new CreateDailyProgramming(aggregateId, movieId, screenId, dailyDate, seats, movieTitle, screenName);
    }

    protected override ICommandHandler<CreateDailyProgramming> OnHandler()
    {
      return new CreateDailyProgrammingCommandHandler(Repository, loggerFactory.Object);
    }

    protected override IEnumerable<DomainEvent> Expect()
    {
      yield return new DailyProgrammingCreated(aggregateId, movieId, screenId, dailyDate, seats, movieTitle, screenName);
    }
  }
}
