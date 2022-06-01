using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace Kletterverein.Models.DB
{
    public class RepositoryShopDB : IRepositoryShop {

        private string _connectionString = "Server=localhost;database=alpclimb;user=root;password=";
        private DbConnection _conn;

        public async Task ConnectAsync()
        {
            if (this._conn == null) {
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open) {
                await this._conn.OpenAsync();
            }
        }

        public async Task DisconnectAsync()
        {
            if ((this._conn != null) && (this._conn.State == ConnectionState.Open)) {
                await this._conn.CloseAsync();
            }
        }

        public async Task<bool> addProductToCartAsync(int userId, int productId)
        {
            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdInsert = this._conn.CreateCommand();
                cmdInsert.CommandText = "insert into cart values(null, @userId, @productId)";

                DbParameter paramUId = cmdInsert.CreateParameter();
                paramUId.ParameterName = "userId";
                paramUId.DbType = DbType.Int32;
                paramUId.Value = userId;

                DbParameter paramPId = cmdInsert.CreateParameter();
                paramPId.ParameterName = "productId";
                paramPId.DbType = DbType.Int32;
                paramPId.Value = productId;

                cmdInsert.Parameters.Add(paramUId);
                cmdInsert.Parameters.Add(paramPId);
                
                return await cmdInsert.ExecuteNonQueryAsync() == 1;
            }

            return false;
        }

        public async Task<bool> deleteProductFromCartAsync(int userId, int productId)
        {
            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdDelete = this._conn.CreateCommand();
                cmdDelete.CommandText = "delete from cart where user_id = @userId and product_id = @productId";

                DbParameter paramUId = cmdDelete.CreateParameter();
                paramUId.ParameterName = "userId";
                paramUId.DbType = DbType.Int32;
                paramUId.Value = userId;

                DbParameter paramPId = cmdDelete.CreateParameter();
                paramPId.ParameterName = "productId";
                paramPId.DbType = DbType.Int32;
                paramPId.Value = productId;

                cmdDelete.Parameters.Add(paramUId);
                cmdDelete.Parameters.Add(paramPId);

                return await cmdDelete.ExecuteNonQueryAsync() == 1;
            }

            return false;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            List<Product> products = new List<Product>();

            if (this._conn?.State == ConnectionState.Open) {

                DbCommand cmdProducts = this._conn.CreateCommand();
                cmdProducts.CommandText = "select * from product";

                using (DbDataReader reader = await cmdProducts.ExecuteReaderAsync()) {

                    while (await reader.ReadAsync()) {

                        products.Add(new Product()
                        {
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            Productname = Convert.ToString(reader["productname"]),
                            Description = Convert.ToString(reader["description"]),
                            Brand = (Brand)Convert.ToInt32(reader["brand"]),
                            Price = reader.GetDecimal(reader.GetOrdinal("price"))
                        });
                    }
                }
            }
            return products;
        }

        public async Task<List<Product>> getProductsOfCartAsync(int userId)
        {
            List<Product> products = new List<Product>();

            if (this._conn?.State == ConnectionState.Open) {

                DbCommand cmdProducts = this._conn.CreateCommand();
                cmdProducts.CommandText = "select p.product_id, productname, brand, description, price " +
                    "from product as p join cart as c on (p.product_id = c.product_id) " +
                    "join users as u on (c.user_id = u.user_id) where u.user_id = @userId";

                DbParameter paramUId = cmdProducts.CreateParameter();
                paramUId.ParameterName = "userId";
                paramUId.DbType = DbType.Int32;
                paramUId.Value = userId;

                cmdProducts.Parameters.Add(paramUId);

                using (DbDataReader reader = await cmdProducts.ExecuteReaderAsync()) {

                    while (await reader.ReadAsync()) {

                        products.Add(new Product() {
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            Productname = Convert.ToString(reader["productname"]),
                            Description = Convert.ToString(reader["description"]),
                            Brand = (Brand)Convert.ToInt32(reader["brand"]),
                            Price = reader.GetDecimal(reader.GetOrdinal("price"))
                        });
                    }
                }
            }
            return products;
        }

        public async Task<bool> productAlreadyInCartAsync(int userId, int productId)
        {

            if (this._conn?.State == ConnectionState.Open) {

                DbCommand cmdProducts = this._conn.CreateCommand();
                cmdProducts.CommandText = "select p.product_id " +
                    "from product as p join cart as c on (p.product_id = c.product_id) " +
                    "join users as u on (c.user_id = u.user_id) " +
                    "where u.user_id = @userId and p.product_id = @productId";

                DbParameter paramUId = cmdProducts.CreateParameter();
                paramUId.ParameterName = "userId";
                paramUId.DbType = DbType.Int32;
                paramUId.Value = userId;
                
                DbParameter paramPId = cmdProducts.CreateParameter();
                paramPId.ParameterName = "productId";
                paramPId.DbType = DbType.Int32;
                paramPId.Value = productId;

                cmdProducts.Parameters.Add(paramUId);
                cmdProducts.Parameters.Add(paramPId);

                using (DbDataReader reader = await cmdProducts.ExecuteReaderAsync()) {

                    if (await reader.ReadAsync()) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
