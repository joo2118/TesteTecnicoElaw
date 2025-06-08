using AutoMapper;

namespace Infra.Mapper
{
  public static class MappingExtensions
  {
    /// <summary>
    /// Creates mapping from <typeparamref name="TDestination" /> to <typeparamref name="TSource"/>,
    /// enabling validation so that <see cref="MapperConfiguration.AssertConfigurationIsValid"/>
    /// correctly identifies any missed reverse mappings.
    /// This is the expected behavior, as described by the developer (https://github.com/AutoMapper/AutoMapper/issues/2327#issuecomment-332209154)
    /// </summary>
    public static IMappingExpression<TDestination, TSource> CreateValidatedReverseMap<TSource, TDestination>(
      this IMappingExpression<TSource, TDestination> mappingExpression)
    {
      return mappingExpression.ReverseMap().ValidateMemberList(MemberList.Source);
    }

    public static IMappingExpression CreateValidatedReverseMap(
      this IMappingExpression mappingExpression)
    {
      return mappingExpression.ReverseMap().ValidateMemberList(MemberList.Source);
    }
  }
}
