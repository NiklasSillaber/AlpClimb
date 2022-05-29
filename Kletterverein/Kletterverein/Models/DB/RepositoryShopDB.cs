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

        public void Connect()
        {
            if (this._conn == null) {
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open) {
                this._conn.Open();
            }
        }

        public void Disconnect()
        {
            if ((this._conn != null) && (this._conn.State == ConnectionState.Open)) {
                this._conn.Close();
            }
        }

        public bool addProductToCart(int userId, int productId)
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
                
                return cmdInsert.ExecuteNonQuery() == 1;
            }

            return false;
        }

        public bool deleteProductFromCart(int userId, int productId)
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

                return cmdDelete.ExecuteNonQuery() == 1;
            }

            return false;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            if (this._conn?.State == ConnectionState.Open) {

                DbCommand cmdProducts = this._conn.CreateCommand();
                cmdProducts.CommandText = "select * from product";

                using (DbDataReader reader = cmdProducts.ExecuteReader()) {

                    while (reader.Read()) {

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

        public List<Product> getProductsOfCart(int userId)
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

                using (DbDataReader reader = cmdProducts.ExecuteReader()) {

                    while (reader.Read()) {

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
    }
}
