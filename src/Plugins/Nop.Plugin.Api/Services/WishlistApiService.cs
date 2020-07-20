using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Api.DataStructures;

namespace Nop.Plugin.Api.Services
{
    public class WishlistApiService : IWishlistApiService
    {
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemsRepository;
        private readonly IStoreContext _storeContext;

        public WishlistApiService(IRepository<ShoppingCartItem> shoppingCartItemsRepository, IStoreContext storeContext)
        {
            _shoppingCartItemsRepository = shoppingCartItemsRepository;
            _storeContext = storeContext;
        }

        public List<ShoppingCartItem> GetWishlistItems(int? shoppingCartTypeId = null, DateTime? createdAtMin = null, DateTime? createdAtMax = null, DateTime? updatedAtMin = null, DateTime? updatedAtMax = null, int limit = 50, int page = 1)
        {
            var query = GetWishlistItemsQuery(shoppingCartTypeId, createdAtMin, createdAtMax,
                                                  updatedAtMin, updatedAtMax);

            return new ApiList<ShoppingCartItem>(query, page - 1, limit);
        }

        private IQueryable<ShoppingCartItem> GetWishlistItemsQuery(
            int? shoppingCartTypeId = null, DateTime? createdAtMin = null, DateTime? createdAtMax = null,
            DateTime? updatedAtMin = null, DateTime? updatedAtMax = null)
        {
            var query = _shoppingCartItemsRepository.Table;

            if (shoppingCartTypeId != null)
            {
                query = query.Where(shoppingCartItem => shoppingCartItem.ShoppingCartTypeId == shoppingCartTypeId);
            }

            if (createdAtMin != null)
            {
                query = query.Where(c => c.CreatedOnUtc > createdAtMin.Value);
            }

            if (createdAtMax != null)
            {
                query = query.Where(c => c.CreatedOnUtc < createdAtMax.Value);
            }

            if (updatedAtMin != null)
            {
                query = query.Where(c => c.UpdatedOnUtc > updatedAtMin.Value);
            }

            if (updatedAtMax != null)
            {
                query = query.Where(c => c.UpdatedOnUtc < updatedAtMax.Value);
            }

            // items for the current store only
            var currentStoreId = _storeContext.CurrentStore.Id;
            query = query.Where(c => c.StoreId == currentStoreId);

            query = query.OrderBy(shoppingCartItem => shoppingCartItem.Id);

            return query;
        }
    }
}
