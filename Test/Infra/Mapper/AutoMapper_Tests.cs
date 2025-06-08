using System.Reflection;

using AutoMapper;
using Infra.Mapper;
using Xunit;

namespace Test.Infra.Mapper
{
    public class AutoMapper_Tests
    {
        [Fact]
        public void CreateValidMappingConfiguration()
        {
            var configuration = CreateMapperConfiguration();
            configuration.AssertConfigurationIsValid();
        }

        private static MapperConfiguration CreateMapperConfiguration()
        {
            var myAssembly = Assembly.GetAssembly(typeof(AutoMapperSetup));
            return new MapperConfiguration(cfg => cfg.AddMaps(myAssembly));
        }

        internal static IMapper CreateMapper()
        {
            var configuration = CreateMapperConfiguration();
            return configuration.CreateMapper();
        }
    }
}
