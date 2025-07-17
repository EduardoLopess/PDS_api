using api.RealtimeHubs;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdicionalController : ControllerBase
    {
        private readonly IAdicionalRepository _adicionalRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<AdicionalHub> _hubContext;

        public AdicionalController(IAdicionalRepository adicionalRepository, IMapper mapper, IHubContext<AdicionalHub> hubContext)
        {
            _adicionalRepository = adicionalRepository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adicional = await _adicionalRepository.GetAllAsync();
            if (adicional == null || !adicional.Any())
                return NotFound(new { message = "Não foi encontrado nenhum adicional." });

            var adicionalDTO = _mapper.Map<IList<AdicionalDTO>>(adicional);

            return HttpMessageOk(adicionalDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { mensagem = "ID inválido." });

            var adicional = await _adicionalRepository.GetByIdAsync(id);
            if (adicional == null)
                return NotFound(new { mensage = "Adicional não encontrado." });

            var adicionalDTO = _mapper.Map<AdicionalDTO>(adicional);
            return
                HttpMessageOk(adicionalDTO);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AdicionalViewModel createModel)
        {
            if (!ModelState.IsValid)
                return HttpMessageOk("Dados Incorretos.");

            var adicional = _mapper.Map<Adicional>(createModel);
            await _adicionalRepository.CreateAdicionalAsync(adicional);

            var adicionalDTO = _mapper.Map<AdicionalDTO>(adicional);
            return HttpMessageOk(adicionalDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, AdicionalViewModel model)
        {
            if (!ModelState.IsValid) return HttpMessageError("Dados incorretos.");
            var adicional = _mapper.Map<Adicional>(model);
            adicional.Id = id;
            await _adicionalRepository.UpdateAsync(adicional);

            var adicionalDTO = _mapper.Map<AdicionalDTO>(adicional);

            string tipoAtalizacao = string.Empty;
            if (!adicional.DisponibilidadeAdicional)
            {
                tipoAtalizacao = "AdicionalAtualizado";
            }
            else
            {
                tipoAtalizacao = "AdicionalAtualizado";
            }

            await _hubContext.Clients.All.SendAsync("AdicionalAtualizado", new
            {
                adicional = adicionalDTO,
                tipo = tipoAtalizacao
            });

            return
                HttpMessageOk(adicionalDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { mensagem = "ID inválido." });

            var adicional = await _adicionalRepository.GetByIdAsync(id);
            if (adicional == null)
                return NotFound(new { mensagem = "Adicional não encontrado." });

            await _adicionalRepository.DeleteAsync(id);
            return HttpMessageOk("Adicional deletado com sucesso.");
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