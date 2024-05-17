using AutoMapper;
using Howest.MagicCards.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly ISetRepository _setRepository;
        private readonly IMapper _mapper;

        public SetsController(ISetRepository setRepository, IMapper mapper)
        {
            _setRepository = setRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // get the first 50 sets
            return Ok(_setRepository.GetAll().ToList());
        }
    }
}
