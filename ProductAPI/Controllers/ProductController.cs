using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using System.Collections.Generic;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ILogger<ProductController> _logger;
        private APIResponse _resp;

        public ProductController(ApplicationDbContext db, ILogger<ProductController> logger)
        {
            _db = db;
            _logger = logger;
            _resp = new();
        }
        #region GetAllProducts

        [HttpGet]
        public ActionResult<APIResponse> GetAllProduct()
        {
            try
            {
                var data = _db.Products;
                if (data != null)
                {
                    _resp.IsSucess = true;
                    _resp.Result = data;
                    _resp.Message = $"Total number of products: {data.Count()}";
                    return Ok(_resp);
                }
                _resp.Message = "No Record found";
                return BadRequest(_resp);

            }
            catch (Exception ex)
            {
                _resp.Error = ex.Message;
                return BadRequest(_resp);
            }
        }

        #endregion

        #region GetProductById
        //[HttpGet("{id:int}",Name = "GetProductById")]
        //[Route("{id:int}", Name = "GetProductById")]
        [HttpGet]        
        public ActionResult<APIResponse> GetProductById(int id)
        {
            var data = _db.Products.FirstOrDefault(u => u.ProductId == id);
            if (data != null)
            {
                _resp.IsSucess = true;
                _resp.Result = data;

                return Ok(_resp);
            }
            _resp.Message = $"Product is not found with requested Id : {id}";
            return BadRequest(_resp);
        }

        #endregion

        [HttpPost]
        public ActionResult<APIResponse> CreateProduct(Product req)
        {
            if (req == null)
            {
                _resp.Message = "Please provide a valid product";
                return BadRequest(_resp);
            }

            var Name = _db.Products.FirstOrDefault(u => u.Name == req.Name);
            var Desc = _db.Products.FirstOrDefault(u => u.Description == req.Description);

            if (Name != null && Desc != null)
            {
                _logger.LogError("Record already exist , please provide a unique record");
                _resp.Message = "Record already exist , please provide a unique record";
                return BadRequest(_resp);
            }

            _db.Products.Add(req);
            _db.SaveChanges();
            _resp.IsSucess = true;
            _resp.Message = $"Product: {req.Name} created";
            return Ok(_resp);

        }

        [HttpPut]
        public ActionResult<APIResponse> UpdateProduct(int id, Product req)
        {
            if (id == 0 || req == null || id!=req.ProductId)
            {
                _resp.Message = $"Please provide a valid request";
                return BadRequest(_resp);
            }

            var product = _db.Products.AsNoTracking().FirstOrDefault(u => u.ProductId == req.ProductId);
            if (product == null)
            {
                _resp.Message = $"Not Found Any Product To Update";
                return BadRequest(_resp);
            }
            _db.Products.Update(req);
            _resp.IsSucess = true;
            _resp.Message = $"Product {req.Description} updated successfully";
            _db.SaveChanges();
            return Ok(_resp);

        }

        [HttpPut]
        public ActionResult<APIResponse> PartialUpdate(int id, string data,string field)
        {
            if (id == 0 || data == null || field==null)
            {
                _resp.Message = $"Please provide a valid request";
                return BadRequest(_resp);
            }

            var product = _db.Products.AsNoTracking().FirstOrDefault(u => u.ProductId == id);
            if (product == null)
            {
                _resp.Message = $"Not Found Any Product To Update";
                return BadRequest(_resp);
            }
            Product p1 =new();
            switch (field)
            {
                case "Category":
                    p1.ProductId = product.ProductId;
                    p1.Category = data;
                    p1.Name = product.Name;
                    p1.Description = product.Description;
                    p1.Price = product.Price;                  
                    break;
                case "Name":
                    p1.ProductId = product.ProductId;
                    p1.Category = product.Category;
                    p1.Name = data;
                    p1.Description = product.Description;
                    p1.Price = product.Price;
                    break;
                case "Description":
                    p1.ProductId = product.ProductId;
                    p1.Category = product.Category;
                    p1.Name = product.Name;
                    p1.Description = data;
                    p1.Price = product.Price;
                    break;
                case "Price":
                    p1.ProductId = product.ProductId;
                    p1.Category = product.Category;
                    p1.Name = product.Name;
                    p1.Description = product.Description;
                    p1.Price = double.Parse(data);
                    break;
            }
            _db.Update(p1);
            _resp.IsSucess = true;
            _resp.Message = $"Product {p1.Description} updated successfully";
            _db.SaveChanges();
            return Ok(_resp);

        }

        [HttpDelete]
        public ActionResult<APIResponse> DeleteProduct(int id)
        {
            if (id == 0)
            {
                _resp.Message = $"Please provide a valid request";
                return BadRequest(_resp);
            }
            var product = _db.Products.AsNoTracking().FirstOrDefault(u => u.ProductId == id);
            if (product == null)
            {
                _resp.Message = $"Not Found Any Product To Delete";
                return BadRequest(_resp);
            }
            _db.Products.Remove(product);
            _resp.IsSucess = true;
            _resp.Message = $"Product with id : {id} deleted successfully";
            _db.SaveChanges();
            return Ok(_resp);
        }
    }
}
