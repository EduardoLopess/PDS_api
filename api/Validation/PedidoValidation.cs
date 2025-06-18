using System.Net.NetworkInformation;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Service;
using Domain.ViewModels;


namespace api.Validation
{
    public class PedidoValidation : IPedidoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ISaborRepository _saborRepository;
        private readonly IAdicionalRepository _adicionalRepository;
        private readonly IMesaRepository _mesaRepository;
        private readonly IMesaService _mesaService;
        private readonly IMapper _mapper;

        public PedidoValidation(IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository, ISaborRepository saborRepository, IAdicionalRepository adicionalRepository, IMesaRepository mesaRepository, IMesaService mesaService, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
            _saborRepository = saborRepository;
            _adicionalRepository = adicionalRepository;
            _mesaRepository = mesaRepository;
            _mesaService = mesaService;
            _mapper = mapper;
        }

        public async Task<Pedido> CreatePedidoService(CriarPedidoViewModel viewModel)
        {
            var idMesa = viewModel.MesaId;
            var mesaExiste = await _mesaService.MesaExiste(idMesa);
            if (!mesaExiste)
                throw new ArgumentException("A mesa informada não existe.");

            var mesaOcupada = await _mesaService.MesaOcupada(idMesa);
            if (mesaOcupada)
                throw new ArgumentException("A mesa já está ocupada.");

            //PEGA A MESA
            var mesa = await _mesaRepository.GetByIdAsync(idMesa);

            //PEGA OS PRODUTOS
            var produtoIds = viewModel.Itens.Select(i => i.ProdutoId).ToList();
            var produtos = await _produtoRepository.BuscarProdutoAsync(produtoIds);

            //PEGA OS ADICIONAIS
            var adicionalIds = viewModel.Itens.SelectMany(a => a.AdicionalIDs ?? new List<int?>())
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            var adicionais = await _adicionalRepository.BuscarAdicionaisAsync(adicionalIds);

            var pedido = _mapper.Map<Pedido>(viewModel);
            pedido.Mesa = mesa;



            foreach (var item in pedido.Itens)
            {
                var produto = produtos.FirstOrDefault(p => p.Id == item?.ProdutoId);
                if (produto != null)
                {
                    item.Produto = produto;
                    item.PrecoUnitario = produto.PrecoProduto;
                }

                var ItemViewModel = viewModel.Itens.FirstOrDefault(i => i.ProdutoId == item.ProdutoId && i.Qtd == item.Qtd);

                if (ItemViewModel != null && ItemViewModel.AdicionalIDs != null)
                {
                    item.Adicionals = adicionais.Where(a => ItemViewModel.AdicionalIDs
                    .Contains(a.Id)).ToList();
                }

                //SaborDrink
                if (ItemViewModel.SaborDrinkId.HasValue)
                {
                    var sabor = await _saborRepository.GetByIdAsync(ItemViewModel.SaborDrinkId.Value);
                    if (sabor != null)
                    {
                        item.SaborDrink = sabor;
                    }
                    else
                    {
                        throw new ArgumentException($"Sabor com ID {ItemViewModel.SaborDrinkId.Value} não encontrado.");
                    }
                }

            }

            return pedido;

        }

        public async Task<Pedido> AdicionarItemNovo(int pedidoId, CriarPedidoViewModel updateModel)
        {
            var pedidoExiste = await _pedidoRepository.GetByIdAsync(pedidoId)
            ?? throw new ArgumentException("Pedido não foi encontrado.");

            var mesaUpdateModel = updateModel.MesaId;
            var mesaExiste = await _mesaService.MesaExiste(mesaUpdateModel);
            if (mesaExiste == null)
                throw new ArgumentException("Mesa não foi encontrada.");

            if (pedidoExiste.Mesa == null || pedidoExiste.Mesa.Id != mesaUpdateModel)
                throw new ArgumentException("A mesa informada não corresponde à mesa do pedido.");

            var produtoIds = updateModel.Itens.Select(p => p.ProdutoId).ToList();
            var produtos = await _produtoRepository.BuscarPorIdsAsync(produtoIds);

            //PEGA OS ADICIONAIS
            var adicionalIds = updateModel.Itens.SelectMany(a => a.AdicionalIDs ?? new List<int?>())
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            var adicionais = await _adicionalRepository.BuscarAdicionaisAsync(adicionalIds);

            var novosItens = new List<Item>();

            foreach (var itemModel in updateModel.Itens)
            {
                var produto = produtos.FirstOrDefault(p => p.Id == itemModel.ProdutoId);
                if (produto == null)
                    throw new ArgumentException($"Produto com ID {itemModel.ProdutoId} não encontrado.");

                var novoItem = new Item
                {
                    ProdutoId = itemModel.ProdutoId,
                    Qtd = itemModel.Qtd,
                    PrecoUnitario = produto.PrecoProduto,
                    Produto = produto,
                };

                // Adicionais
                if (itemModel.AdicionalIDs != null)
                {
                    novoItem.Adicionals = adicionais
                        .Where(a => itemModel.AdicionalIDs.Contains(a.Id))
                        .ToList();
                }

                // Sabor
                if (itemModel.SaborDrinkId.HasValue)
                {
                    var sabor = await _saborRepository.GetByIdAsync(itemModel.SaborDrinkId.Value);
                    if (sabor == null)
                        throw new ArgumentException($"Sabor com ID {itemModel.SaborDrinkId.Value} não encontrado.");

                    novoItem.SaborDrink = sabor;
                }

                novosItens.Add(novoItem);
            }

            foreach (var novoItem in novosItens)
            {
                pedidoExiste.Itens.Add(novoItem);
            }

            return pedidoExiste;

        }

        public async Task<Pedido> RemoverItensAsync(int pedidoId, List<int> itemIds)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new ArgumentException("Pedido não foi encontrado.");

            var itensParaRemover = pedido.Itens.Where(i => itemIds.Contains(i.Id)).ToList();
            if (itensParaRemover.Count == 0)
                throw new ArgumentException("Nenhum item informado pertence ao pedido.");

            foreach (var item in itensParaRemover)
            {
                pedido.Itens.Remove(item);
            }

            return pedido;

            
        }
    }
    
}