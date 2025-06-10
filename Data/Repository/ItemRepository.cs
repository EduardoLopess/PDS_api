using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Data.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _context;
        public ItemRepository(DataContext context)
        {
            _context = context;
        }

        public Task BuscarProdutosById(IList<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}