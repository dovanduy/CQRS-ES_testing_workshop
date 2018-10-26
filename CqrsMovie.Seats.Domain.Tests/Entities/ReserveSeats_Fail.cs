﻿using System;
using System.Collections.Generic;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Dtos;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Muflone.Messages.Commands;
using CqrsMovie.Muflone.Messages.Events;
using CqrsMovie.Seats.Domain.CommandHandlers;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.Extensions.Logging.Abstractions;

namespace CqrsMovie.Seats.Domain.Tests.Entities
{
  public class ReserveSeats_Fail : CommandSpecification<ReserveSeat>
  {
    private readonly DailyProgrammingId aggregateId = new DailyProgrammingId(Guid.NewGuid());
    private readonly MovieId movieId = new MovieId(Guid.NewGuid());
    private readonly ScreenId screenId = new ScreenId(Guid.NewGuid());
    private readonly DateTime dailyDate = DateTime.Today;
    private readonly string movieTitle = "rambo";
    private readonly string screenName = "screen 99";
    private readonly IEnumerable<Seat> seats;
    private readonly IEnumerable<Seat> seatsToBook;
    private readonly IEnumerable<Seat> seatsToReserve;
    
    public ReserveSeats_Fail()
    {
      seats = new List<Seat>
            {
                new Seat { Number = 1, Row = "A" },
                new Seat { Number = 2, Row = "A" },
                new Seat { Number = 3, Row = "A" },
                new Seat { Number = 4, Row = "A" },
                new Seat { Number = 1, Row = "B" },
                new Seat { Number = 2, Row = "B" },
                new Seat { Number = 3, Row = "B" },
                new Seat { Number = 4, Row = "B" }
            };

      seatsToBook = new List<Seat>
            {
                new Seat { Number = 1, Row = "B" },
                new Seat { Number = 2, Row = "B" },
                new Seat { Number = 3, Row = "B" }
            };

      seatsToReserve = new List<Seat>
            {
                new Seat { Number = 1, Row = "C" },
                new Seat { Number = 2, Row = "C" },
                new Seat { Number = 3, Row = "C" }
            };

      ExpectedException = new Exception("Unable to reserve seats. Already taken");
    }

    protected override IEnumerable<DomainEvent> Given()
    {
      yield return new DailyProgrammingCreated(aggregateId, movieId, screenId, dailyDate, seats, movieTitle, screenName);
      yield return new SeatsBooked(aggregateId, seatsToBook);
    }

    protected override ReserveSeat When()
    {
      return new ReserveSeat(aggregateId, seatsToReserve);
    }

    protected override ICommandHandler<ReserveSeat> OnHandler()
    {
      return new ReserveSeatCommandHandler(Repository, new NullLoggerFactory()); 
    }

    protected override IEnumerable<DomainEvent> Expect()
    {
      return new List<DomainEvent>();
    }
  }
}