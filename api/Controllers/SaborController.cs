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
    public class SaborController : ControllerBase
    {
        private readonly ISaborRepository _saborRepository;
        private readonly IMapper _mapper;
        public SaborController(ISaborRepository saborRepository, IMapper mapper)
        {
            _saborRepository = saborRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sabor = await _saborRepository.GetAllAsync();
            var saborDTo = _mapper.Map<IList<SaborDTO>>(sabor);

            return HttpMessageOk(saborDTo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sabor = await _saborRepository.GetByIdAsync(id);
            if (sabor == null)
                return NotFound(new { mensagem = $"Sabor Não encontrado." });

            var saborDTO = _mapper.Map<SaborDTO>(sabor);
            return HttpMessageOk(saborDTO);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] SaborViewModel createModel)
        {
            if (createModel == null)
                return BadRequest("Dados inválidos.");

            if (string.IsNullOrWhiteSpace(createModel.NomeSabor))
                return BadRequest("Nome é obrigatorio.");

            var sabor = _mapper.Map<Sabor>(createModel);

            await _saborRepository.CreateSabor(sabor);

            var saborDTO = _mapper.Map<SaborDTO>(sabor);

            return CreatedAtAction(nameof(GetById), new { id = sabor.Id }, saborDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaborViewModel model)
        {
            if (!ModelState.IsValid) return HttpMessageError("Dados incorretos");
            var sabor = _mapper.Map<Sabor>(model);
            sabor.Id = id;
            await _saborRepository.UpdateAsync(sabor);

            var saborDTO = _mapper.Map<SaborDTO>(sabor);
            return
                HttpMessageOk(saborDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sabor = await _saborRepository.GetByIdAsync(id);
            if (sabor == null) return NotFound(new { mensagem = $"Sabor Não encontrado." });

            await _saborRepository.DeleteAsync(id);
            return
                HttpMessageOk($"Sabor deletado com sucesso.");
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