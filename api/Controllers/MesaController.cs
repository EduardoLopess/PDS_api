using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class MesaController : ControllerBase
    {
        private readonly IMesaRepository _mesaRepository;
        private readonly IMapper _mapper;
        public MesaController(IMesaRepository mesaRepository, IMapper mapper)
        {
            _mesaRepository = mesaRepository;
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
                return HttpMessageOk("Dados incorretos. ");

            var mesa = _mapper.Map<Mesa>(createModel);
            await _mesaRepository.CreateAsync(mesa);

            var mesaDTO = _mapper.Map<MesaDTO>(mesa);
            return HttpMessageOk(mesaDTO);

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

            if (mesa.StatusMesa)
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