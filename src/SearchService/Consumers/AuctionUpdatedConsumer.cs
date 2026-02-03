using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine("--> Consuming auction updated: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        await DB.Update<Item>()
            .MatchID(item.ID)
            .Modify(x => x.Make, item.Make)
            .Modify(x => x.Model, item.Model)
            .Modify(x => x.Year, item.Year)
            .Modify(x => x.Color, item.Color)
            .Modify(x => x.Mileage, item.Mileage)
            .ExecuteAsync();
    }
}