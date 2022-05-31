using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wipro.Business.Interfaces;
using Wipro.Business.Models;
using Wipro.Data.Context;

namespace Wipro.Data.Repository
{
    public class ItemRepository : IItemRepository
    {
        protected readonly MyDbContext myDb;

        public ItemRepository(MyDbContext db)
        {
            myDb = db;
        }
        public async Task Add(List<Item> itens)
        {
            var newList = new List<Item>();

            itens.ForEach(x =>
            {
                newList.Add(new Item()
                {
                    Moeda = x.Moeda,
                    DataInicio = x.DataInicio,
                    DataFim = x.DataFim
                });
            });

            myDb.Itens.AddRange(newList);

            await myDb.SaveChangesAsync();
        }

        public async Task<Item> Get()
        {
            var lastItem = myDb.Itens.ToList().LastOrDefault();

            if(lastItem != null)
            {
                var deleteLast = myDb.Itens.Find(lastItem.Id);
                myDb.Itens.Remove(deleteLast);

                await myDb.SaveChangesAsync();

                return lastItem;
            }

            return lastItem;
        }
    }
}
