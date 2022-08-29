using AutoMapper;
using Blessings.Data.Entities;
using Blessings.Models;
using Blessings.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blessings.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SetController : ControllerBase
    {
        private readonly ISetService _setService;
        private readonly IMapper _mapper;
        public SetController(ISetService setService, 
                             IMapper mapper)
        {
            _setService = setService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSets()
        {
            var sets = await _setService.GetSetsAsync();

            return Ok(sets);

        }

        [HttpPost]
        public async Task<IActionResult> AddSet([FromBody] SetModel setModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var set = _mapper.Map<Set>(setModel);

            await _setService.AddSetAsync(set);

            return Ok();

        }

        [HttpPost("set/assortment")]
        public async Task<IActionResult> AddAssortment([FromBody] AssortmentModel assortmentModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var assortment = _mapper.Map<Assortment>(assortmentModel);

            await _setService.AddAssortmentAsync(assortment);

            return Ok();

        }
    }
}
