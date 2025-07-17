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
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<ProdutoHub> _hubContext;

        public ProdutoController(IProdutoRepository produtoRepository, IMapper mapper, IHubContext<ProdutoHub> hubContext)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produto = await _produtoRepository.GetAllAsync();
            var produtoDTO = _mapper.Map<IList<ProdutoDTO>>(produto);

            return HttpMessageOk(produtoDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
                return NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return HttpMessageOk(produtoDTO);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProdutoViewModel createModel)
        {
            if (createModel == null)
                return BadRequest("Dados inválidos.");

            // Validação opcional de campos
            if (string.IsNullOrWhiteSpace(createModel.NomeProduto) || createModel.PrecoProduto <= 0)
                return BadRequest("Nome e preço são obrigatórios e devem ser válidos.");

            // Mapeia ViewModel para entidade
            var produto = _mapper.Map<Produto>(createModel);

            await _produtoRepository.CreateAsync(produto);
            // Mapeia de volta para DTO para retornar
            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            await _hubContext.Clients.All.SendAsync("ProdutoCriado", produtoDTO);


            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produtoDTO);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProdutoViewModel model)
        {
            if (!ModelState.IsValid) return HttpMessageError("Dados incorretos.");

            var produto = _mapper.Map<Produto>(model);
            produto.Id = id;
            await _produtoRepository.UpdateAsync(produto);

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            string tipoAtalizacao = string.Empty;
            if (!produto.DisponibilidadeProduto)
            {
                tipoAtalizacao = "ProdutoIndisponivel";
            }
            else
            {
                tipoAtalizacao = "ProdutoAtualizado";
            }


            await _hubContext.Clients.All.SendAsync("ProdutoAtualizado", new
            {
                produto = produtoDTO,
                Tipo = tipoAtalizacao
            });

            return HttpMessageOk(produtoDTO);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null) return NotFound(new { mensage = $"Produto não encontrado." });

            await _produtoRepository.DeleteAsync(id);
            return
                HttpMessageOk($"Produto deletado com sucesso.");
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