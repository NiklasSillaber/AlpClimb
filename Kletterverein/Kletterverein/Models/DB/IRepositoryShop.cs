using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Models.DB {
    interface IRepositoryShop {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<List<Product>> GetProductsAsync();
        Task<bool> addProductToCartAsync(int userId, int productId);
        Task<bool> deleteProductFromCartAsync(int userId, int productId);
        Task<List<Product>> getProductsOfCartAsync(int userId);
        Task<bool> productAlreadyInCartAsync(int userId, int productId);

    }
}
