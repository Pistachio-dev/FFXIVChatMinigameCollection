//using AutoMapper;
//using PersistentModel.Model.PlayerManagement;

//namespace PersistentModel.Model
//{
//    public class Mapper
//    {
//        internal static readonly IMapper Mapper;

//        static Mapper()
//        {
//            var mapperConfiguration = new MapperConfiguration(configuration =>
//            {
//                configuration.CreateMap<PlayerOOGData, PlayerOOGData>()
//                .ForMember(d => d.Id, opt => opt.Ignore());
//            });

//            mapperConfiguration.AssertConfigurationIsValid();
//            Mapper = mapperConfiguration.CreateMapper();
//        }
//    }
//}
