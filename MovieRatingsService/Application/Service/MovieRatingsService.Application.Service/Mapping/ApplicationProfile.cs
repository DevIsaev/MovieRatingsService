using AutoMapper;
using MovieRatingsService.Application.Models.Admin;
using MovieRatingsService.Application.Models.Movie;
using MovieRatingsService.Application.Models.Rating;
using MovieRatingsService.Application.Models.Review;
using MovieRatingsService.Application.Models.User;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Application.Service.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username.Value));

            CreateMap<Admin, AdminModel>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username.Value))
                .ForMember(d => d.Permissions, o => o.MapFrom(s => s.Permissions.Value));

            CreateMap<Movie, MovieModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title.Value))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description == null ? null : s.Description.Value))
                .ForMember(d => d.ReleaseYear, o => o.MapFrom(s => s.ReleaseYear == null ? (int?)null : s.ReleaseYear.Value))
                .ForMember(d => d.Genre, o => o.MapFrom(s => s.Genre == null ? null : s.Genre.Value))
                .ForMember(d => d.PosterUrl, o => o.MapFrom(s => s.PosterUrl == null ? null : s.PosterUrl.Value))
                .ForMember(d => d.CreatedByAdminId, o => o.MapFrom(s => s.CreatedByAdmin.Id))
                .ForMember(d => d.AverageScore, o => o.MapFrom(s => s.GetAverageScore()))
                .ForMember(d => d.RatingsCount, o => o.MapFrom(s => s.GetRatingsCount()));

            CreateMap<Rating, RatingModel>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
                .ForMember(d => d.MovieId, o => o.MapFrom(s => s.Movie.Id))
                .ForMember(d => d.Score, o => o.MapFrom(s => s.Score.Value));

            CreateMap<Review, ReviewModel>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
                .ForMember(d => d.MovieId, o => o.MapFrom(s => s.Movie.Id))
                .ForMember(d => d.Content, o => o.MapFrom(s => s.Content.Value))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        }
    }
}