using api.RealtimeHubs;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.InputModels;
using Domain.Interfaces;
using Domain.Service;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        private readonly IHubContext<PedidoHub> _hubContext;

        public PedidoController(IPedidoRepository pedidoRepository, IMesaService mesaService, IPedidoService pedidoService, IMapper mapper, IHubContext<PedidoHub> hubContext)
        {
            _pedidoRepository = pedidoRepository;
            _mesaService = mesaService;
            _pedidoService = pedidoService;
            _mapper = mapper;
            _hubContext = hubContext; // Injetar aqui!
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
                return NotFound("Id não é válido.");

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
                await _hubContext.Clients.All.SendAsync("PedidoCriado", pedidoDTO);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPedido(int id, [FromBody] AtualizarPedidoViewModel model)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");

            try
            {
                var pedidoAtualizado = await _pedidoService.AtualizarPedidoCompleto(id, model);
                await _pedidoRepository.UpdateAsync(pedidoAtualizado);
                var pedidoDTO = _mapper.Map<PedidoDTO>(pedidoAtualizado);
                await _hubContext.Clients.All.SendAsync("PedidoAtualizado", pedidoDTO);
                return Ok(new { data = pedidoDTO });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao atualizar pedido.", erro = ex.Message });
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
            await _hubContext.Clients.All.SendAsync("PedidoCancelado", id);
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
                await _hubContext.Clients.All.SendAsync("PedidoAtualizado", pedidoDTO);

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

                await _pedidoRepository.UpdateAsync(pedidoAtualizado);
                var pedidoDTO = _mapper.Map<PedidoDTO>(pedidoAtualizado);
                await _hubContext.Clients.All.SendAsync("PedidoAtualizado", pedidoDTO);
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


        //Remover Adicional
        [HttpPatch("{pedidoId}/remover-adicional")]
        public async Task<IActionResult> RemoverAdicionalAsync(int pedidoId, [FromBody] RemoverAdicionalViewModel model)
        {
            if (pedidoId <= 0 || model.AdicionalId <= 0 || model.ProdutoId <= 0)
                return BadRequest("Algum Id inválido");

            try
            {
                var pedidoAtualizado = await _pedidoService.RemoverAdicional(pedidoId, model);
                await _pedidoRepository.UpdateAsync(pedidoAtualizado);
                var pedidoDTO = _mapper.Map<PedidoDTO>(pedidoAtualizado);
                await _hubContext.Clients.All.SendAsync("PedidoAtualizado", pedidoDTO);
                return HttpMessageOk(pedidoDTO);

            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Falha ao remover o adicional.", erro = ex.Message });
            }
        }

        [HttpGet("mesa/{mesaId}")]
        public async Task<IActionResult> GetPedidoByMesa(int mesaId)
        {

            try
            {
                var pedido = await _pedidoRepository.BuscarPedidoAtivoMesaAsync(mesaId);
                if (pedido == null)
                    return NotFound("Nenhum pedido ativo para essa mesa.");
                var viewModel = _mapper.Map<PedidoViewModel>(pedido);
                return HttpMessageOk(viewModel);

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


        //CONTROLE DE EDIÇÂO

        [HttpPost("{id}/desbloquear")]
        public async Task<IActionResult> LiberarPedidoEdicao(int id, [FromBody] ConnectionIdRequestModel request)
        {

            if (id <= 0)
                return BadRequest("Id inválido");

            if (request == null || string.IsNullOrWhiteSpace(request.ConnectionIdDoCliente))
                return BadRequest("Id da conexão é obragatório.");

            var liberadoPedido = await _pedidoService.DesbloquearPedidoAsync(id, request.ConnectionIdDoCliente);

            if (!liberadoPedido)
            {
                var pedidoAtual = await _pedidoRepository.GetByIdAsync(id);
                if (pedidoAtual == null)
                {
                    return NotFound("Pedido não encontrado.");
                }
                else if (pedidoAtual.StatusPedido == true && pedidoAtual.ConnectionId != request.ConnectionIdDoCliente)
                {
                    return BadRequest("Sem permissão para liberar este pedido");
                }

                return StatusCode(500, "Falha interna ao tentar liberar o pedido");
            }

            var pedidoDTO = _mapper.Map<PedidoDTO>(await _pedidoRepository.GetByIdAsync(id));
            await _hubContext.Clients.All.SendAsync("PedidoStatusAlterado", pedidoDTO);
            return HttpMessageOk(new { message = "Pedido liberadoo com sucesso.", data = pedidoDTO });


        }

        [HttpPost("{id}/bloquear")]
        public async Task<IActionResult> BloquearPedidoEdicao(int id, [FromBody] ConnectionIdRequestModel request)
        {

            if (id <= 0)
                return BadRequest("ID do pedido inválido.");
            if (request == null || string.IsNullOrWhiteSpace(request.ConnectionIdDoCliente))
                return BadRequest("ID da conexão do cliente obrigatória.");

            try
            {
                var desbloqueadoCoSucesso = await _pedidoRepository.TentarBloquearPedidoAsync(id, request.ConnectionIdDoCliente);

                if (!desbloqueadoCoSucesso)
                {
                    var pedidoAtual = await _pedidoRepository.GetByIdAsync(id);

                    if (pedidoAtual == null)
                    {
                        return NotFound("Pedido não encontrado.");
                    }
                    else if (pedidoAtual.StatusPedido == true && pedidoAtual.ConnectionId != request.ConnectionIdDoCliente)
                    {
                        return Conflict("Pedido já está em edição.");
                    }
                    return StatusCode(500, "Falha ao tentar bloquear o pedido.");
                }

                var pedidoDTO = _mapper.Map<PedidoDTO>(await _pedidoRepository.GetByIdAsync(id));
                await _hubContext.Clients.All.SendAsync("PedidoStatusAlterado", pedidoDTO);
                return HttpMessageOk(new { message = "Pedido bloquado com sucesso.", data = pedidoDTO });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Falha ao bloquear pedido.", erro = ex.Message });
            }



        }

        [HttpPatch("{pedidoId}/mudar/mesa")]
        public async Task<IActionResult> MudarMesaPedido(int pedidoId, [FromBody] int mesaId)
        {
            if (pedidoId < 0)
                return BadRequest("Id do pedido inválido.");

            if (mesaId < 0)
                return BadRequest("Id mesa inválido.");

            try
            {
                var atualizarMesaPedido = await _pedidoService.AtualizarMesaPedido(pedidoId, mesaId);
                var pedidoDTO = _mapper.Map<PedidoDTO>(atualizarMesaPedido);
                await _hubContext.Clients.All.SendAsync("MesaPedidoAtualizada", pedidoDTO);
                
                return HttpMessageOk(new { message = "Pedido atualizado com sucesso.", data = pedidoDTO });

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