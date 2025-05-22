using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Interface;
using SonitCustom.Controller.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonitCustom.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ITokenService _tokenService;

        public ProductController(IProductService productService, ITokenService tokenService)
        {
            _productService = productService;
            _tokenService = tokenService;
        }

        // GET: api/Product
        [HttpGet]
        [AllowAnonymous]
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
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ProductDTO>> CreateProduct(CreateProductDTO product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                await _productService.CreateProductAsync(product);
                return Ok(new { message = "Thêm sản phẩm thành công" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductDTO product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                await _productService.UpdateProductAsync(id, product);
                return Ok(new { message = "Chỉnh sửa sản phẩm thành công" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                bool result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound($"Không tìm thấy sản phẩm có mã {id}");
                }

                return Ok(new { message = "Đã xoá sản phẩm thành công" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}