using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Inmuebles;
using NetKubernetes.Dtos.InmuebleDtos;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using System.Net;

namespace NetKubernetes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InmueblesController : ControllerBase
    {
        private readonly IInmuebleRepository _inmuebleRepository;
        private readonly IMapper _mapper;

        public InmueblesController(IInmuebleRepository inmuebleRepository, IMapper mapper)
        {
            _inmuebleRepository = inmuebleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InmuebleResponseDto>>> GetInmuebles(){
            var inmuebles = await _inmuebleRepository.GetAllInmuebles();
            return Ok(_mapper.Map<IEnumerable<InmuebleResponseDto>>(inmuebles));
        }

        [HttpGet("{id}", Name = "GetInmuebleById")]
        public async Task<ActionResult<InmuebleResponseDto>> GetInmuebleById(int id)
        {
            var inmueble = await _inmuebleRepository.GetInmuebleById(id);
            if (inmueble is null) {
                throw new MiddlewareException(HttpStatusCode.NotFound, new { mensaje = $"No se encontro el inmueble por este id {id}" });
            }
            return Ok(_mapper.Map<InmuebleResponseDto>(inmueble));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<InmuebleResponseDto>> CreateInmueble([FromBody] InmuebleRequestDto request) {
            var InmuebleModel = _mapper.Map<Inmueble>(request);
            await _inmuebleRepository.CreateInmueble(InmuebleModel);
            await _inmuebleRepository.SaveChanges();
            var inmuebleResponseDto = _mapper.Map<InmuebleResponseDto>(InmuebleModel);
            return CreatedAtRoute(nameof(GetInmuebleById), new { inmuebleResponseDto.Id }, inmuebleResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInmueble(int id) {
            await _inmuebleRepository.DeleteInmueble(id);
            await _inmuebleRepository.SaveChanges();
            return Ok();
        }
    }
}
