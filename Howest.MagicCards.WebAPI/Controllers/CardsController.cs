using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardsController(ICardRepository cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<PagedResponse<IEnumerable<CardReadDTO>>> Get([FromQuery] CardFilter filter)
        {
            var query = _cardRepository.GetAll()
                .ToFilteredList(filter.Name, filter.Text, filter.Type, filter.SetCode, filter.RarityCode, filter.ArtistId, filter.SortOrder);

            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

            var pagedCards = query
                .ToPagedList(filter.PageNumber, filter.PageSize)
                .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider)
                .ToList();

            return Ok(new PagedResponse<IEnumerable<CardReadDTO>>(pagedCards, filter.PageNumber, filter.PageSize)
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages
            });
        }


        [HttpGet("{id}")]
        public ActionResult<Response<CardDetailReadDTO>> Get(int id)
        {
            var card = _cardRepository.GetById(id);
            var cardDetailReadDto = _mapper.Map<CardDetailReadDTO>(card);
            return Ok(new Response<CardDetailReadDTO>(cardDetailReadDto));
        }
    }
}
