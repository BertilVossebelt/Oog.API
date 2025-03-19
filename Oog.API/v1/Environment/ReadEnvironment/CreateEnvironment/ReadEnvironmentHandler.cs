using API.Common.Collections;
using API.Common.DTOs;
using API.v1.Environment.ReadEnvironment.CreateEnvironment.Interfaces;
using API.v1.Environment.ReadEnvironment.CreateEnvironment.Requests;
using AutoMapper;

namespace API.v1.Environment.ReadEnvironment.CreateEnvironment;

public class ReadEnvironmentHandler(IReadEnvironmentRepository readEnvironmentRepository, IMapper mapper) : IReadEnvironmentHandler
{
    public async Task<IEnumerable<EnvironmentDto>?> Read(long ownerId)
    {
        var environmentCollection = await readEnvironmentRepository.Read(ownerId);
        return environmentCollection;
    }

}