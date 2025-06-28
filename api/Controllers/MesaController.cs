using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Service;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesaController : ControllerBase
    {
        private readonly IMesaRepository _mesaRepository;
        private readonly IMapper _mapper;
        private readonly IMesaService _mesaService;
        public MesaController(IMesaRepository mesaRepository, IMesaService mesaService, IMapper mapper)
        {
            _mesaRepository = mesaRepository;
            _mesaService = mesaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var mesa = await _mesaRepository.GetAllAsync();
            var mesaDTO = _mapper.Map<IList<MesaDTO>>(mesa);

            return HttpMessageOk(mesaDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mesa = await _mesaRepository.GetByIdAsync(id);
            if (mesa == null) return NotFound();

            var mesaDTO = _mapper.Map<MesaDTO>(mesa);
            return
                HttpMessageOk(mesaDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] MesaViewModel createModel)
        {
            if (!ModelState.IsValid)
                return HttpMessageError("Dados incorretos. ");
                
            try
            {
                var mesa = await _mesaService.CreateMesaService(createModel);
                await _mesaRepository.CreateAsync(mesa);
                var mesaDTO = _mapper.Map<MesaDTO>(mesa);
                return HttpMessageOk(mesaDTO);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Falha cadastrar a MESA.", erro = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MesaViewModel model)
        {
            if (!ModelState.IsValid) return HttpMessageError("Dados incorretos.");
            var mesa = _mapper.Map<Mesa>(model);
            mesa.Id = id;
            await _mesaRepository.UpdateAsync(mesa);

            var mesaDTO = _mapper.Map<MesaDTO>(mesa);
            return
                HttpMessageOk(mesaDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mesa = await _mesaRepository.GetByIdAsync(id);
            if (mesa == null) 
                return NotFound(new { mensagem = "Mesa não encontrada." });

            var mesaOcupada = await _mesaService.MesaOcupada(id);
            if (mesaOcupada)
                return BadRequest(new { mensagem = "Mesa está ocupada e não pode ser deletada." });

            await _mesaRepository.DeleteAsync(id);
            return HttpMessageOk("Mesa deletada com sucesso.");
        }


        //METODOS EXTRAS
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AlterarStatusMesa(int id)
        {
            var resultado = await _mesaRepository.MudaStatusMesaAsync(id);

            if (!resultado)
                return NotFound("Mesa não encontrada.");

            return Ok("Status da mesa alterado com sucesso.");
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