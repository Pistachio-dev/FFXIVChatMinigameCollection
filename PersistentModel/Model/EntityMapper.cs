using AutoMapper;
using Microsoft.Extensions.Logging;
using Model.Banking;
using Model.Banking.Transactions;
using Model.PlayerManagement;
using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;

namespace PersistentModel.Model
{
    public class EntityMapper
    {
        internal static readonly IMapper Mapper;
        internal static ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddFile());

        // Note to self: Always .Ignore() EF Navigation properties. If any object links back to their parent,
        // even with one way .Ignore(), it destroys the reference when mapping.
        static EntityMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                var playerOOgDataMap = configuration.CreateMap<PlayerOOGData, PlayerOOGDataEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore());
                playerOOgDataMap.ForMember(d => d.CashRecord, opts => opts.Ignore());
                playerOOgDataMap.ReverseMap();

                var crMap = configuration.CreateMap<PlayerCashRecord, PlayerCashRecordEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.PlayerOOGDataId, opt => opt.Ignore())
                .ForMember(d => d.PlayerOOGData, opt => opt.Ignore());
                crMap.ReverseMap();

                var playerIdentifierMap = configuration.CreateMap<PlayerIdentifier, PlayerIdentifierEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.PlayerOOGDataId, opt => opt.Ignore())
                .ForMember(d => d.PlayerOOGData, opt => opt.Ignore());                
                playerIdentifierMap.ReverseMap();



                var gtMap = configuration.CreateMap<GilTransaction, GilTransactionEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.HostPlayerId, opt => opt.Ignore())
                .ForMember(d => d.PatronPlayerId, opt => opt.Ignore());
                gtMap.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                gtMap.ReverseMap();

            }, loggerFactory);

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }
    }
}
