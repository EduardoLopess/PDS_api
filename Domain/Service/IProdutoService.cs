using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Service
{
    public interface IProdutoService
    {
        Task<bool> ProdutosExiste(IList<int> produtoIds);
    }
}