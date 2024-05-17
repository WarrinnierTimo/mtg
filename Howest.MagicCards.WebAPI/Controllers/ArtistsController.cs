using AutoMapper;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistsController(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var artists = _artistRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<ArtistReadDTO>>(artists));
        }
    }
}
