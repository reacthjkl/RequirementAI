using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class OrganizationProfile: Profile
{
    public OrganizationProfile()
    {
        CreateMap<Organization, OrganizationResponseDto>();
        CreateMap<OrganizationForUpdateDto, Organization>();
    }
}