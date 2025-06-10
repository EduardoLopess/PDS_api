using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public PedidoController(IPedidoRepository pedidoRepository, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedido = await _pedidoRepository.GetAllAsync();
            var pedidoDTO = _mapper.Map<IList<PedidoDTO>>(pedido);

            return HttpMessageOk(pedidoDTO);
        }

        private IActionResult HttpMessageOk(dynamic data = null)
        {
            if (data == null)
                return NoContent();
            else
                return Ok(new { data });
        }

        private IActionResult HttpMessageError(string message = "")
        {
            return BadRequest(new { message });
        }

    }
}