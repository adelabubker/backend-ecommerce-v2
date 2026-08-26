using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

using Kosa.ECommerce.Application.Mapping;

namespace Kosa.ECommerce.Tests;

internal static class TestHelpers
{
    public static IMapper CreateMapper()
    {
        return new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance).CreateMapper();
    }
}
