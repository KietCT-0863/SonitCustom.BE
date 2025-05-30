using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.DTOs.Products;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.Controller.Helpers;

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
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        //////GET: api/Product/5
        ////[HttpGet("{id}")]
        ////public async Task<ActionResult<Product>> GetProduct(string id)
        ////{
        ////    try
        ////    {
        ////        var product = await _productService.GetProductByIdAsync(id);
        ////        if (product == null)
        ////        {
        ////            return NotFound($"Product with ID {id} not found");
        ////        }
        ////        return Ok(product);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return StatusCode(500, $"Internal server error: {ex.Message}");
        ////    }
        ////}

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
            catch (CategoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        // PUT: api/Product/5
        [HttpPut("{prodId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(string prodId, UpdateProductDTO product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                await _productService.UpdateProductAsync(prodId, product);
                return Ok(new { message = "Chỉnh sửa sản phẩm thành công" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (CategoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        // DELETE: api/Product/5
        [HttpDelete("{prodId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(string prodId)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                await _productService.DeleteProductAsync(prodId);
                return Ok(new { message = "Đã xoá sản phẩm thành công" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }
    }
}