using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace api.Configuration
{
    public class AutoMapperConfigDTOs : Profile
    {
        public AutoMapperConfigDTOs()
        {
            CreateMap<Produto, ProdutoDTO>()
                .ForMember(dest => dest.CategoriaProduto, opt => opt.MapFrom(src => src.CategoriaProduto.ToString()))
                .ForMember(dest => dest.TipoProduto, opt => opt.MapFrom(src => src.TipoProduto.ToString()))
                .ForMember(dest => dest.PrecoProdutoFormatado, opt =>
                     opt.MapFrom(src => src.PrecoProduto.ToString("N2", new System.Globalization.CultureInfo("pt-BR"))));

            CreateMap<Mesa, MesaDTO>();
            CreateMap<Sabor, SaborDTO>();
            CreateMap<Drink, DrinkDTO>();
            CreateMap<Item, ItemDTO>()
                .ForMember(dest => dest.Adicionais, opt => opt.MapFrom(src => src.Adicionals));
            CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.DateTimeFormatado, opt =>
                    opt.MapFrom(src => src.DateTime.ToString(("dd/MM/yyyy HH:mm"))))
                .ForMember(dest => dest.NumeroMesa, opt =>
                    opt.MapFrom(src => src.Mesa != null ? src.Mesa.NumeroMesa : 0))
                .ForMember(dest => dest.MesaId, opt =>
                    opt.MapFrom(src => src.Mesa != null ? src.Mesa.Id : 0));
            


            CreateMap<Adicional, AdicionalDTO>()
                .ForMember(dest => dest.PrecoAdicionalFormatado, opt =>
                    opt.MapFrom(src => src.PrecoAdicional.ToString("N2", new System.Globalization.CultureInfo("pt-BR"))));

            CreateMap<Pedido, CriarPedidoDTO>()
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens));

            CreateMap<Item, CriarItemDTO>()
                .ForMember(dest => dest.Adicionals, opt => opt.MapFrom(src => src.Adicionals))
                .ForMember(dest => dest.SaborDrink, opt => opt.MapFrom(src => src.SaborDrink));

            

        }
    }
}
