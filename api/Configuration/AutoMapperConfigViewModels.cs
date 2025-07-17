using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.ViewModels;

namespace api.Configuration
{
    public class AutoMapperConfigViewModels : Profile
    {
        public AutoMapperConfigViewModels()
        {
            CreateMap<ProdutoViewModel, Produto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
                
    
            CreateMap<MesaViewModel, Mesa>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignora o Id ausente na ViewModel
                .ForMember(dest => dest.NumeroMesa, opt => opt.MapFrom(src => src.NumeroMesa ?? 0)); // Trata valor nulo

            CreateMap<SaborViewModel, Sabor>();
            CreateMap<DrinkViewModel, Drink>();
            CreateMap<PedidoViewModel, Pedido>();
            CreateMap<AdicionalViewModel, Adicional>();


            CreateMap<Pedido, PedidoViewModel>();
            CreateMap<Item, ItemViewModel>(); // <-- ESTE resolve seu erro
            CreateMap<Produto, ProdutoViewModel>(); // (se necessário)
            CreateMap<Sabor, SaborViewModel>(); // (se necessário)
            CreateMap<Adicional, AdicionalViewModel>(); // (se necessário)
            CreateMap<Mesa, MesaViewModel>(); // ← ENTIDADE → ViewModel

            CreateMap<CriarPedidoViewModel, Pedido>();
            CreateMap<CriarItemViewModel, Item>();
        }

    }
}