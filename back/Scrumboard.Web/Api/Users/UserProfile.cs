using AutoMapper;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Web.Api.Users;

internal sealed class UserProfile : Profile
{
    public UserProfile()
    {
        // Write
        CreateMap<UserDto, string>()
            .ConstructUsing(x => x.Id);
        
        // Read
        CreateMap<IUser, UserDto>()
            .ForMember(dest => dest.HasAvatar, opt => opt.MapFrom(src => src.Avatar.Length > 0));
        
        CreateMap<string, UserDto>()
            .ConstructUsing(userId => new UserDto
            {
                Id = userId
            });
    }
}
