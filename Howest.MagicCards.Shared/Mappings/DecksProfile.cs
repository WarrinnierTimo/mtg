using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Mappings
{
    public class DecksProfile : AutoMapper.Profile
    {
        public DecksProfile()
        {
            CreateMap<Deck, DeckDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CardDecks, opt => opt.MapFrom(src => 
                    src.CardDecks.Select(cd => new CardDeckDTO
                    {
                        Id = cd.Id,
                        Amount = cd.Amount
                    }).ToList()));

            CreateMap<DeckDTO, Deck>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CardDecks, opt => opt.MapFrom(src => 
                    src.CardDecks.Select(cd => new CardDeck
                    {
                        Id = cd.Id,
                        Amount = cd.Amount
                    }).ToList()));

            CreateMap<CardDeck, CardDeckDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            CreateMap<CardDeckDTO, CardDeck>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

        }
    }
}
