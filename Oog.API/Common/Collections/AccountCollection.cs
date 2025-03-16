using API.Common.DTOs;

namespace API.Common.Collections;

public record AccountCollection(
    IReadOnlyCollection<AccountDto> Data
);