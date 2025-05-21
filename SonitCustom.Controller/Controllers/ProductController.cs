using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.Services;
using SonitCustom.Controller.Helpers;
using SonitCustom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonitCustom.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                List<ProductDTO> products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        ////GET: api/Product/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Product>> GetProduct(string id)
        //{
        //    try
        //    {
        //        var product = await _productService.GetProductByIdAsync(id);
        //        if (product == null)
        //        {
        //            return NotFound($"Product with ID {id} not found");
        //        }
        //        return Ok(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(CreateProductDTO product)
        {
            try
            {
                if (!JwtCookieHelper.IsAdmin(Request))
                {
                    return Unauthorized(new { message = "Chỉ admin mới có quyền truy cập" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdProduct = await _productService.CreateProductAsync(product);
                return Ok("Đã thêm sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductDTO product)
        {
            try
            {
                if (!JwtCookieHelper.IsAdmin(Request))
                {
                    return Unauthorized(new { message = "Chỉ admin mới có quyền truy cập" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Product updatedProduct = await _productService.UpdateProductAsync(id, product);
                return Ok("Đã chỉnh sửa sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                if (!JwtCookieHelper.IsAdmin(Request))
                {
                    return Unauthorized(new { message = "Chỉ admin mới có quyền truy cập" });
                }

                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                return Ok("Đã xoá sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}