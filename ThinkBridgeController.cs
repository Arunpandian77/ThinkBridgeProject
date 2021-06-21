using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ThinkBridge.Controllers
{
    public class ThinkBridgeController : ApiController
    {

        [HttpPost]
        public IHttpActionResult addNewProd(Product prod)
        {
               if(prod == null)
                   return BadRequest("Invalid data.");

               DataSet dst = AddNewProd(prod);
               if (dst != null)
               {
                    if (dst.Tables[0].Rows[0][0].ToString() == "100")
                   return Ok(prod.ProductName + " Added Successfully");
               else
                   return InternalServerError();
               }
               else
               {
                   return InternalServerError();
               }
        }
        public IHttpActionResult getAllProducts()
        {
            IList<Product> products = null;
            products = SelectAllProduct();
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }

        public IHttpActionResult getSingleProducts(int prodId)
        {
            Product product = null;
            product = SelectSingleProduct(prodId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        public IHttpActionResult updateProduct(Product prod)
        {
            if (prod == null)
                return BadRequest("Invalid data.");

            DataSet dst = UpdateProd(prod);
            if (dst != null)
            {
                if (dst.Tables[0].Rows[0][0].ToString() == "100")
                    return Ok(prod.ProductName + " Updated Successfully");
                else
                    return InternalServerError();
            }
            else
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult deleteProduct(int prodId)
        {
            DataSet dst = DeleteProd(prodId);
            if (dst != null)
            {
                if (dst.Tables[0].Rows[0][0].ToString() == "100")
                    return Ok(" Product Deleted Successfully");
                else
                    return InternalServerError();
            }
            else
            {
                return InternalServerError();
            }
        }

        public IList<Product> SelectAllProduct()
        {
            IList<Product> prods = null;
            try
            {
                string connetionString = null;
                SqlCommand command = new SqlCommand();
                DataSet ds = new DataSet();
                connetionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                using (SqlConnection conn = new SqlConnection(connetionString))
                {
                    SqlCommand sqlComm = new SqlCommand("selectAllProd", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.Fill(ds);
                    for (int j = 0; j <= ds.Tables[0].Rows.Count; j++)
                    {
                        Product product = new Product();
                        product.ProductId = Convert.ToInt32(ds.Tables[0].Rows[j]["ProductId"]);
                        product.ProductName = ds.Tables[0].Rows[j]["ProductName"].ToString();
                        product.ProductPrice = ds.Tables[0].Rows[j]["ProductPrice"].ToString();
                        product.ProductDescription = ds.Tables[0].Rows[j]["ProductDesc"].ToString();

                        prods.Add(product);
                    }
                }
                return prods;
            }
            catch(Exception ex)
            {
                return prods;
            }
        }

        public Product SelectSingleProduct(int ProdId)
        {
            try
            {
                Product prod = null;
                string connetionString = null;
                SqlCommand command = new SqlCommand();
                DataSet ds = new DataSet();
                connetionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                using (SqlConnection conn = new SqlConnection(connetionString))
                {
                    SqlCommand sqlComm = new SqlCommand("selectSingleProd", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.Fill(ds);
                    if (ds.Tables[0] != null)
                    {
                        prod.ProductId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProductId"]);
                        prod.ProductName = ds.Tables[0].Rows[0]["ProductName"].ToString();
                        prod.ProductPrice = ds.Tables[0].Rows[0]["ProductPrice"].ToString();
                        prod.ProductDescription = ds.Tables[0].Rows[0]["ProductDesc"].ToString();
                    }
                    return prod;
                }
            }
            catch(Exception ex)
            {
                Product prod = null;
                return prod;
            }
        }

        public DataSet AddNewProd(Product prod)
        {
            try
            {
                string connetionString = null;
                SqlCommand command = new SqlCommand();
                DataSet ds = new DataSet();
                connetionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                using (SqlConnection conn = new SqlConnection(connetionString))
                {
                    SqlCommand sqlComm = new SqlCommand("addProd", conn);
                    sqlComm.Parameters.AddWithValue("@productid", prod.ProductId);
                    sqlComm.Parameters.AddWithValue("@productname", prod.ProductName);
                    sqlComm.Parameters.AddWithValue("@productprice", prod.ProductPrice);
                    sqlComm.Parameters.AddWithValue("@productdesc", prod.ProductDescription);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public DataSet UpdateProd(Product prod)
        {
            try
            {
                string connetionString = null;
                SqlCommand command = new SqlCommand();
                DataSet ds = new DataSet();
                connetionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                using (SqlConnection conn = new SqlConnection(connetionString))
                {
                    SqlCommand sqlComm = new SqlCommand("updateProd", conn);
                    sqlComm.Parameters.AddWithValue("@productid", prod.ProductId);
                    sqlComm.Parameters.AddWithValue("@productname", prod.ProductName);
                    sqlComm.Parameters.AddWithValue("@productprice", prod.ProductPrice);
                    sqlComm.Parameters.AddWithValue("@productdesc", prod.ProductDescription);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet DeleteProd(int prodId)
        {
            try
            {
                string connetionString = null;
                SqlCommand command = new SqlCommand();
                DataSet ds = new DataSet();
                connetionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                using (SqlConnection conn = new SqlConnection(connetionString))
                {
                    SqlCommand sqlComm = new SqlCommand("deleteProd", conn);
                    sqlComm.Parameters.AddWithValue("@productid", prodId);
                  
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPrice { get; set; }

    }
}
