using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Service;

namespace Data.Validation
{
    public class ProdutoValidation : IProdutoService
    {
        public Task<bool> ProdutosExiste(IList<int> produtoIds)
        {
            throw new NotImplementedException();
        }
    }
}