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
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMesaService _mesaService;
        private readonly IPedidoService _pedidoService;
        private readonly IMapper _mapper;

        public PedidoController(IPedidoRepository pedidoRepository, IMesaService mesaService, IPedidoService pedidoService, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mesaService = mesaService;
            _pedidoService = pedidoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedido = await _pedidoRepository.GetAllAsync();
            var pedidoDTO = _mapper.Map<IList<PedidoDTO>>(pedido);

            return HttpMessageOk(pedidoDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return NotFound("Id não é vá  lido.");

            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            var pedidoDTO = _mapper.Map<PedidoDTO>(pedido);
            return
                HttpMessageOk(pedidoDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePedidoAsync([FromBody] CriarPedidoViewModel createModel)
        {
            if (!ModelState.IsValid)
                return HttpMessageOk("Dados incorretos.");

            try
            {
                var pedidoCriado = await _pedidoService.CreatePedidoService(createModel);
                await _pedidoRepository.CreatePedidoAsync(pedidoCriado);
                var pedidoDTO = _mapper.Map<CriarPedidoDTO>(pedidoCriado);
                await _mesaService.MudaStatusMesaAsync(pedidoCriado.Mesa.Id);
                return HttpMessageOk(pedidoDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro interno: " + ex.Message); 
                return BadRequest(new { mensagem = "Falha ao criar o PEDIDO.", erro = ex.Message });

            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedidoAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                return NotFound(new { message = "Pedido não encontrado." });

            await _pedidoRepository.DeleteAsync(id);
            await _mesaService.MudaStatusMesaAsync(pedido.Mesa.Id);
            return HttpMessageOk("Pedido deletado com sucesso.");
        }

        // Add 1-n Itens ao pedido
        [HttpPatch("{id}")]
        public async Task<IActionResult> AdicionarItemNovo(int id, [FromBody] CriarPedidoViewModel upateModel)
        {
            if (id <= 0)
                return NotFound("Id inválido.");

            if (!ModelState.IsValid)
                return NotFound("Dados inválidos.");

            try
            {
                var pedidoComItemNovo = await _pedidoService.AdicionarItemNovo(id, upateModel);
                var pedidoDTO = _mapper.Map<CriarPedidoDTO>(pedidoComItemNovo);
                await _pedidoRepository.UpdateAsync(pedidoComItemNovo);
                return HttpMessageOk(pedidoDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro interno: " + ex.Message);
                return BadRequest(new { mensagem = "Falha ao criar o PEDIDO.", erro = ex.Message });
            }
        }


        //REMOVE 1-N itens do pedido
        [HttpPatch("{pedidoId}/remover-itens")]
        public async Task<IActionResult> RemoverItemPedidoAsync(int pedidoId, [FromBody] List<int> itemIds)
        {
            if (itemIds == null || itemIds.Count == 0)
                return BadRequest("Nenhum item informado para remoção.");

            try
            {
                var pedidoAtualizado = await _pedidoService.RemoverItensAsync(pedidoId, itemIds);
                if (pedidoAtualizado.Itens == null || !pedidoAtualizado.Itens.Any())
                {
                    await _pedidoRepository.DeleteAsync(pedidoId);
                    return HttpMessageOk("Pedido sem itens cancelado.");
                }

                var pedidoDTO = _mapper.Map<PedidoDTO>(pedidoAtualizado);
                await _pedidoRepository.UpdateAsync(pedidoAtualizado);
                return HttpMessageOk(pedidoDTO);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Falha ao remover item do pedido.", erro = ex.Message });
            }
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