using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wipro.Business.Models;

namespace Wipro.Business.Interfaces
{
    public interface IItemRepository
    {
        Task Add(List<Item> itens);
        Task<Item> Get();
    }
}
