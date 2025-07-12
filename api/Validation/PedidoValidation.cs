using System.Net.NetworkInformation;
using AutoMapper;
using Domain.Entities;
using Domain.InputModels;
using Domain.Interfaces;
using Domain.Service;
using Domain.ViewModels;
using Xunit.Sdk;


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
            var adicionalIds = viewModel.Itens
                .SelectMany(i => i.Adicionais ?? new List<ItemAdicionalInputViewModel>())
                .Select(ai => ai.AdicionalId)
                .Distinct()
                .ToList();
            var adicionais = await _adicionalRepository.BuscarAdicionaisAsync(adicionalIds);

            var pedido = _mapper.Map<Pedido>(viewModel);
            pedido.Mesa = mesa;



            foreach (var item in pedido.Itens)
            {
                var vm = viewModel.Itens
                    .First(i => i.ProdutoId == item.ProdutoId && i.Qtd == item.Qtd);

                item.Produto = produtos.First(p => p.Id == vm.ProdutoId);
                item.PrecoUnitario = item.Produto.PrecoProduto;

                if (vm.Adicionais != null)
                {
                    foreach (var ai in vm.Adicionais)
                    {
                        var adicional = adicionais.FirstOrDefault(a => a.Id == ai.AdicionalId)
                            ?? throw new ArgumentException($"Adicional com ID {ai.AdicionalId} não encontrado.");
                        item.ItemAdicionais.Add(new ItemAdicional
                        {
                            AdicionalId = ai.AdicionalId,
                            Adicional = adicional,
                            Qtd = ai.Qtd
                        });
                    }
                }

                if (vm.SaborDrinkId.HasValue)
                {
                    var sabor = await _saborRepository.GetByIdAsync(vm.SaborDrinkId.Value)
                        ?? throw new ArgumentException($"Sabor com ID {vm.SaborDrinkId} não encontrado.");
                    item.SaborDrink = sabor;
                }
            }

            pedido.TotalPedido = viewModel.TotalPedido;
            return pedido;
        }

        public async Task<Pedido> AdicionarItemNovo(int pedidoId, CriarPedidoViewModel updateModel)
        {
            var pedidoExiste = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new ArgumentException("Pedido não foi encontrado.");

            var mesaUpdateModel = updateModel.MesaId;
            var mesaExiste = await _mesaService.MesaExiste(mesaUpdateModel);
            if (!mesaExiste)  // Verifica se mesaExiste é falso
                throw new ArgumentException("Mesa não foi encontrada.");

            if (pedidoExiste.Mesa == null || pedidoExiste.Mesa.Id != mesaUpdateModel)
                throw new ArgumentException("A mesa informada não corresponde à mesa do pedido.");

            var produtoIds = updateModel.Itens.Select(p => p.ProdutoId).ToList();
            var produtos = await _produtoRepository.BuscarPorIdsAsync(produtoIds);

            var adicionalIds = updateModel.Itens
                .SelectMany(i => i.Adicionais ?? new List<ItemAdicionalInputViewModel>()) // lista vazia do tipo correto
                .Select(a => a.AdicionalId) // aqui extrai o Id do adicional
                .Distinct()
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
                    ItemAdicionais = new List<ItemAdicional>() // inicializa a lista aqui
                };

                // Adicionais — aqui precisa criar objetos ItemAdicional para cada adicional selecionado
                if (itemModel.Adicionais != null)
                {
                    foreach (var adicionalId in itemModel.Adicionais)
                    {
                        var adicional = adicionais.FirstOrDefault(a => a.Id == adicionalId.AdicionalId);
                        if (adicional == null)
                            throw new ArgumentException($"Adicional com ID {adicionalId} não encontrado.");

                        novoItem.ItemAdicionais.Add(new ItemAdicional
                        {
                            AdicionalId = adicional.Id,
                            Adicional = adicional,
                            Qtd = 1 // ou a quantidade que fizer sentido — ajustar conforme modelo
                        });
                    }
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

            // Garantir que a lista de itens do pedido não seja null
            if (pedidoExiste.Itens == null)
                pedidoExiste.Itens = new List<Item>();

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
                if (item.Qtd > 1)
                {
                    item.Qtd -= 1;
                }
                else
                {
                    pedido.Itens.Remove(item);
                }
                // pedido.Itens.Remove(item);
            }

            return pedido;

        }

        public async Task<Pedido> RemoverAdicional(int pedidoId, RemoverAdicionalViewModel model)
        {
            var pedidoExiste = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new ArgumentException("Pedido não existe.");

            var produtoId = model.ProdutoId;
            var adicionalId = model.AdicionalId;

            var produto = pedidoExiste.Itens.FirstOrDefault(p => p.ProdutoId == produtoId)
                ?? throw new ArgumentException("Produto não foi encontrado.");

            var adicional = produto.ItemAdicionais.FirstOrDefault(a => a != null && a.AdicionalId == adicionalId)
                            ?? throw new ArgumentException("Adicional não foi encontrado para o produto.");

            produto.ItemAdicionais.Remove(adicional);

            return pedidoExiste;
        }

        public async Task<Pedido> AtualizarPedidoCompleto(int pedidoId, AtualizarPedidoViewModel model)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new ArgumentException("Pedido não encontrado");

            var mesaExiste = await _mesaService.MesaExiste(model.MesaId);
            if (!mesaExiste)
                throw new ArgumentException("Mesa informada não existe.");

            if (pedido.Mesa == null || pedido.Mesa.Id != model.MesaId)
            {
                var mesa = await _mesaRepository.GetByIdAsync(model.MesaId);
                if (mesa == null)
                    throw new ArgumentException("Mesa não encontrada");
                pedido.Mesa = mesa;
            }

            pedido.Itens.Clear();

            var produtoIds = model.Itens.Select(i => i.ProdutoId).ToList();
            var produtos = await _produtoRepository.BuscarPorIdsAsync(produtoIds);

            var adicionalIds = model.Itens
                .SelectMany(i => i.Adicionais ?? new List<ItemAdicionalInputViewModel>())
                .Select(a => a.AdicionalId) // pegando o ID de cada adicional
                .Distinct()
                .ToList();


            var adicionais = await _adicionalRepository.BuscarAdicionaisAsync(adicionalIds);
            foreach (var itemModel in model.Itens)
            {
                var produto = produtos.FirstOrDefault(p => p.Id == itemModel.ProdutoId);
                if (produto == null)
                    throw new ArgumentException($"Produto com ID {itemModel.ProdutoId} não encontrado.");

                var novoItem = new Item
                {
                    ProdutoId = itemModel.ProdutoId,
                    Qtd = itemModel.Qtd,
                    PrecoUnitario = produto.PrecoProduto,
                    Produto = produto
                };

                // Adicionais
                if (itemModel.Adicionais != null)
                {
                    foreach (var adicionalInput in itemModel.Adicionais)
                    {
                        var adicional = adicionais.FirstOrDefault(a => a.Id == adicionalInput.AdicionalId);
                        if (adicional == null)
                            throw new ArgumentException($"Adicional com ID {adicionalInput.AdicionalId} não encontrado.");

                        novoItem.ItemAdicionais.Add(new ItemAdicional
                        {
                            AdicionalId = adicional.Id,
                            Adicional = adicional,
                            Qtd = adicionalInput.Qtd
                        });
                    }
                }

                // Sabor (se houver)
                if (itemModel.SaborDrinkId.HasValue)
                {
                    var sabor = await _saborRepository.GetByIdAsync(itemModel.SaborDrinkId.Value);
                    if (sabor == null)
                        throw new ArgumentException($"Sabor com ID {itemModel.SaborDrinkId.Value} não encontrado.");

                    novoItem.SaborDrink = sabor;
                }

                pedido.Itens.Add(novoItem);
            }

            pedido.TotalPedido = model.TotalPedido;

            return pedido;
        }


        public async Task<bool> TentarBloquearPedidoAsync(int pedidoId, string connectionIdDoCliente)
        {
            return await _pedidoRepository.TentarBloquearPedidoAsync(pedidoId, connectionIdDoCliente);
        }

        public async Task<bool> DesbloquearPedidoAsync(int pedidoId, string connectionIdDoCliente)
        {
            return await _pedidoRepository.DesbloquearPedidoAsync(pedidoId, connectionIdDoCliente);
        }

        public async Task<Pedido> AtualizarMesaPedido(int pedidoId, int mesaId)
        {

            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new ArgumentException("Pedido não encontrado.");

            if (pedido.StatusPedido)
                throw new ArgumentException("Pedido está sendo editado.");

            var mesaAntiga = pedido.Mesa;

            var mesaNova = await _mesaRepository.GetByIdAsync(mesaId)
                ?? throw new ArgumentException("Mesa não encontrada.");

            var mesaOcupada = await _mesaService.MesaOcupada(mesaNova.Id);
            if (mesaOcupada)
                throw new InvalidOperationException("Mesa já está ocupada.");

            pedido.Mesa = mesaNova;

            await _mesaService.MudaStatusMesaAsync(mesaNova.Id);
            await _mesaService.MudaStatusMesaAsync(mesaAntiga.Id);

            await _pedidoRepository.UpdateAsync(pedido);
            return pedido;


        }
    }


}