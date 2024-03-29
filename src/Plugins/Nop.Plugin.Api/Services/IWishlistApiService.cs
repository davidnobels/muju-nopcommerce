﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Api.Infrastructure;

namespace Nop.Plugin.Api.Services
{
    public interface IWishlistApiService
    {
        List<ShoppingCartItem> GetWishlistItems(
            int? shoppingCartTypeId = null, DateTime? createdAtMin = null, DateTime? createdAtMax = null,
            DateTime? updatedAtMin = null, DateTime? updatedAtMax = null, int limit = Constants.Configurations.DefaultLimit,
            int page = Constants.Configurations.DefaultPageValue);

        //ShoppingCartItem GetWishlist(IEnumerable shoppingCartType);
    }
}
