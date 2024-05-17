using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Mappings
{
    public class ArtistsProfile : Profile
    {
        public ArtistsProfile() {
            CreateMap<Artist, ArtistReadDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
        }
    }
}
