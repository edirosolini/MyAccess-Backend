// <copyright file="MappingProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.DependencyInjection
{
    using AutoMapper;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Requests;
    using MyAccess.Domains.Responses;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<UserEntity, AuthenticateResponse>()
                .ForMember(d => d.BusinessName, o => o.MapFrom(s => $"{s.LastName} {s.FirstName}"));

            this.CreateMap<UserRequest, UserEntity>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.EmailAddress));

            this.CreateMap<UserEntity, UserRequest>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.EmailAddress));
        }
    }
}
