using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Models
{
    public class Product
    {

        private int _productId;
        private decimal _price;

        public int ProductId
        {
            get { return this._productId; }
            set
            {
                if (value >= 0)
                {
                    this._productId = value;
                }
            }
        }

        public decimal Price
        {
            get { return this._price; }
            set
            {
                if (value >= 0)
                {
                    this._price = value;
                }
            }
        }

        public string Productname { get; set; }
        public string Description { get; set; }
        public Brand Brand { get; set; }

        public Product() : this(0, "", "", Brand.notSpecified, 0.0m) { }

        public Product(int productId, string productname, string description, Brand brand, decimal price)
        {
            this.ProductId = productId;
            this.ProductId = productId;
            this.Description = description;
            this.Brand = brand;
            this.Price = price;
        }

        public String toString()
        {
            return this._productId + " " + this.Productname + " " + this.Description + " " + this.Brand + " " + this._price;
        }
    }
}
