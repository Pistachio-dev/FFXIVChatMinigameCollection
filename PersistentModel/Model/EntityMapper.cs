using AutoMapper;
using Microsoft.Extensions.Logging;
using Model.Banking;
using Model.PlayerManagement;
using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;

namespace PersistentModel.Model
{
    public class EntityMapper
    {
        internal static readonly IMapper Mapper;
        internal static ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddFile());
        static EntityMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<PlayerOOGData, PlayerOOGDataEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ReverseMap();

                configuration.CreateMap<PlayerIdentifier, PlayerIdentifierEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.PlayerOOGDataId, opt => opt.Ignore())
                .ForMember(d => d.PlayerOOGData, opt => opt.Ignore())
                .ReverseMap();

                configuration.CreateMap<PlayerCashRecord, PlayerCashRecordEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.PlayerOOGDataId, opt => opt.Ignore())
                .ForMember(d => d.PlayerOOGData, opt => opt.Ignore())
                .ReverseMap();

                configuration.CreateMap<GilTransaction, GilTransactionEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.HostPlayerId, opt => opt.Ignore())
                .ForMember(d => d.PatronPlayerId, opt => opt.Ignore())
                .ReverseMap();

            }, loggerFactory);

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }
    }
}
