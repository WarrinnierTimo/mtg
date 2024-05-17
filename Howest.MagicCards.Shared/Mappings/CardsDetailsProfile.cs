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
    public class CardsDetailsProfile : Profile
    {
        public CardsDetailsProfile() {
            CreateMap<Card, CardDetailReadDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.OriginalImageUrl));
        }
    }
}
