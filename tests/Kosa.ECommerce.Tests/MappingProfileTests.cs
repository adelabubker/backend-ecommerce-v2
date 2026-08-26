using AutoMapper;
using Kosa.ECommerce.Application.Mapping;
using Microsoft.Extensions.Logging.Abstractions;


namespace Kosa.ECommerce.Tests;

public sealed class MappingProfileTests
{
    [Fact]
    public void MappingConfiguration_IsValid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance);
        config.AssertConfigurationIsValid();
    }
}
