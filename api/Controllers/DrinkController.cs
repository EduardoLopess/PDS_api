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
    public class DrinkController : ControllerBase
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly IMapper _mapper;
        public DrinkController(IDrinkRepository drinkRepository, IMapper mapper)
        {
            _drinkRepository = drinkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var drink = await _drinkRepository.GetAllAsync();
            var drinkDTO = _mapper.Map<IList<DrinkDTO>>(drink);

            return HttpMessageOk(drinkDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0 )
                return BadRequest(new { mensagem = "ID inválido." });

            var drink = await _drinkRepository.GetByIdAsync(id);
            if (drink == null) return NotFound();

            var drinkDTO = _mapper.Map<DrinkDTO>(drink);
            return
                HttpMessageOk(drinkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] DrinkViewModel createModel)
        {
            if (!ModelState.IsValid)
                return HttpMessageOk("Dados Incorretos.");

            var drink = _mapper.Map<Drink>(createModel);
            await _drinkRepository.CreateDrinkAsync(drink);

            var drinkDTO = _mapper.Map<MesaDTO>(drink);
            return HttpMessageOk(drinkDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DrinkViewModel model)
        {
            if (!ModelState.IsValid) return HttpMessageError("Dados incorretos.");
            var drink = _mapper.Map<Drink>(model);
            drink.Id = id;
            await _drinkRepository.UpdateAsync(drink);

            var drinkDTO = _mapper.Map<DrinkDTO>(drink);
            return
                HttpMessageOk(drinkDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var drink = await _drinkRepository.GetByIdAsync(id);
            if (drink == null)
                return NotFound(new { mensagem = "Drink não econtrado." });

            await _drinkRepository.DeleteAsync(id);
            return HttpMessageOk("Drink deletado com sucesso.");
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