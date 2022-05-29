using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Models.DB {
    interface IRepositoryShop {
        void Connect();
        void Disconnect();

        //bool Insert(User user);
        //bool Delete(int userId);
        //bool Update(User newUserData);
        List<Product> GetProducts();
        bool addProductToCart(int userId, int productId);
        bool deleteProductFromCart(int userId, int productId);
        List<Product> getProductsOfCart(int userId);

    }
}
