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
            CreateMap<ProdutoViewModel, Produto>();
            CreateMap<MesaViewModel, Mesa>();
            CreateMap<SaborViewModel, Sabor>();
            CreateMap<DrinkViewModel, Drink>();
            CreateMap<PedidoViewModel, Pedido>();
            CreateMap<AdicionalViewModel, Adicional>();
        }
        
    }
}