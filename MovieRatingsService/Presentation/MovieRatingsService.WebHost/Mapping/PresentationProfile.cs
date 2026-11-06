using AutoMapper;
using MovieRatingsService.Application.Models.User;
using MovieRatingsService.WebHost.Requests.User;
using MovieRatingsService.WebHost.Responses.User;

namespace MovieRatingsService.WebHost.Mapping
{
    public class PresentationProfile : Profile
    {
        public PresentationProfile()
        {
            CreateMap<UserModel, UserResponse>();
            CreateMap<CreateUserRequest, CreateUserModel>();
        }
    }
}
