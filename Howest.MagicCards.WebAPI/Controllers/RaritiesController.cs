using AutoMapper;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaritiesController : ControllerBase
    {
        private readonly IRarityRepository _rarityRepository;
        private readonly IMapper _mapper;

        public RaritiesController(IRarityRepository rarityRepository, IMapper mapper)
        {
            _rarityRepository = rarityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // get all rarities
            return Ok(_rarityRepository.GetAll().ToList());
        }
    }
}
