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
        private readonly IProdutoService _produtoService;
        private readonly IMesaRepository _mesaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public PedidoController(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository, IMesaRepository mesaRepository, IMesaService mesaService, IProdutoService produtoService, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mesaRepository = mesaRepository;
            _produtoRepository = produtoRepository;
            _mesaService = mesaService;
            _produtoService = produtoService;
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

            var idMesa = createModel.MesaId;
            var mesaExiste = await _mesaService.MesaExiste(idMesa);
            if (!mesaExiste)
                return BadRequest("Mesa não encontrada.");

            var mesa = await _mesaRepository.GetByIdAsync(idMesa);
            var mesaOcupada = await _mesaService.MesaOcupada(idMesa);
            if (mesaOcupada)
                return BadRequest("Mesa já está ocupada.");


            var produtoIds = createModel.Itens.Select(i => i.ProdutoId).ToList();
            var produtos = await _produtoRepository.BuscarProdutoAsync(produtoIds);

            var pedido = _mapper.Map<Pedido>(createModel);

            pedido.Mesa = mesa;

            foreach (var item in pedido.Itens)
            {
                var produto = produtos.FirstOrDefault(p => p.Id == item.ProdutoId);
                if (produto != null)
                {
                    item.Produto = produto;
                    item.PrecoUnitario = produto.PrecoProduto;
                }

            }

            await _pedidoRepository.CreatePedidoAsync(pedido);
            var criarPedidoDTO = _mapper.Map<CriarPedidoDTO>(pedido);

            var alteraStatusMesa = _mesaService.MudaStatusMesaAsync(idMesa);
            return HttpMessageOk(criarPedidoDTO);
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