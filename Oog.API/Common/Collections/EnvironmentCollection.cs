using API.Common.DTOs;

namespace API.Common.Collections;

public record EnvironmentCollection(
    IReadOnlyCollection<EnvironmentDto> Data
);