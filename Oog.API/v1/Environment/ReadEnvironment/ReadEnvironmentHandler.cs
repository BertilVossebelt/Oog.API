using API.Common.DTOs;
using API.v1.Environment.ReadEnvironment.Interfaces;
using AutoMapper;

namespace API.v1.Environment.ReadEnvironment;

public class ReadEnvironmentHandler(IReadEnvironmentRepository readEnvironmentRepository, IMapper mapper) : IReadEnvironmentHandler
{
    public async Task<IEnumerable<EnvironmentDto>?> Read(long ownerId)
    {
        var environmentCollection = await readEnvironmentRepository.Read(ownerId);
        return environmentCollection;
    }

}