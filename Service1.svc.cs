using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfLager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public List<ProductData> GetAllProducts()
        {
            List<ProductData> returList = new List<ProductData>();
            
            using(DbModel db = new DbModel())
            {
                var tempList = db.Products.ToList();

                foreach (var item in tempList)
                {
                    ProductData p = new ProductData();
                    p.Id = item.Id;
                    p.Name = item.Name;
                    p.Quant = item.Quant;
                    p.Price = item.Price;

                    returList.Add(p);
                }
            }

            return returList;
        }




        public ProductData GetProduct(int id)
        {
            ProductData product = new ProductData();

            using(DbModel db = new DbModel())
            {
                var tempProduct = db.Products.Find(id);

                product.Id = tempProduct.Id;
                product.Name = tempProduct.Name;
                product.Price = tempProduct.Price;
                product.Quant = tempProduct.Quant;
            }

            return product;
        }







        public void Order(int id, int quant)
        {
            List<ProductData> returnList = new List<ProductData>();
            
            using (DbModel db = new DbModel())
            {
                var tempProduct = db.Products.Find(id);

                if (tempProduct.Quant <= quant)
                {
                    GrossistService.Service1Client client = new GrossistService.Service1Client();

                    tempProduct.Quant += client.SupplyStocks(quant);
                                       
                }

                //tempProduct.Quant -= quant;
                db.SaveChanges();
            }

        }
    }
}
