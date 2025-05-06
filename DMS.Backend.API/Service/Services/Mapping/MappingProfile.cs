using AutoMapper.Execution;
using AutoMapper;
using System.Diagnostics.Metrics;
using System;
using DMS.Backend.API.Service.Dtos.Comments;
using DMS.Backend.API.Service.Dtos.Documents;
using DMS.Backend.API.Service.Dtos.DocumentShares;
using DMS.Backend.API.Service.Dtos.DocumentTags;
using DMS.Backend.API.Service.Dtos.ExternalStorages;
using DMS.Backend.API.Service.Dtos.FriendRequests;
using DMS.Backend.API.Service.Dtos.Friends;
using DMS.Backend.API.Service.Dtos.Likes;
using DMS.Backend.API.Service.Dtos.Notifications;
using DMS.Backend.API.Service.Dtos.Users;
using DMS.Backend.Models.Entities;

namespace DMS.Backend.API.Service.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();

            // Comment mappings
            CreateMap<Comment, CommentDto>().ReverseMap();

            // Document mappings
            CreateMap<DocumentViewModel, DocumentDto>().ReverseMap();
            CreateMap<DocumentViewModel, UpdateDocumentDto>().ReverseMap();

            // DocumentShare mappings
            CreateMap<DocumentShare, DocumentShareDto>().ReverseMap();
            CreateMap<DocumentShare, UpdateDocumentShareDto>().ReverseMap();

            //// DocumentTag mappings
            //CreateMap<DocumentTag, DocumentTagDto>().ReverseMap();
            //CreateMap<DocumentTag, UpdateDocumentTagDto>().ReverseMap();

            // ExternalStorage mappings
            CreateMap<ExternalStorage, ExternalStorageDto>().ReverseMap();
            CreateMap<ExternalStorage, UpdateExternalStorageDto>().ReverseMap();

            // Friend mappings
            CreateMap<Friend, FriendDto>().ReverseMap();
            CreateMap<Friend, UpdateFriendDto>().ReverseMap();

            // FriendRequest mappings
            CreateMap<FriendRequest, FriendRequestDto>().ReverseMap();
            CreateMap<FriendRequest, UpdateFriendRequestDto>().ReverseMap();

            // Like mappings
            CreateMap<Like, LikeDto>().ReverseMap();

            // Notification mappings
            CreateMap<Notification, NotificationDto>().ReverseMap();


            // Parameters DTO mappings
            CreateMap<UserParametersDto, User>().ReverseMap();
            CreateMap<CommentParametersDto, Comment>().ReverseMap();
            CreateMap<DocumentParametersDto, DocumentViewModel>().ReverseMap();
            CreateMap<DocumentShareParametersDto, DocumentShare>().ReverseMap();
            //CreateMap<DocumentTagParametersDto, DocumentTag>().ReverseMap();
            CreateMap<ExternalStorageParametersDto, ExternalStorage>().ReverseMap();
            CreateMap<FriendParametersDto, Friend>().ReverseMap();
            CreateMap<FriendRequestParametersDto, FriendRequest>().ReverseMap();
            CreateMap<LikeParametersDto, Like>().ReverseMap();
            CreateMap<NotificationParametersDto, Notification>().ReverseMap();
        }
    }
}
