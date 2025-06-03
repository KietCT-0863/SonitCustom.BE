using Amazon.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.DTOs.Products;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.Controller.Helpers;

namespace SonitCustom.Controller.Controllers
{
    /// <summary>
    /// API controller để quản lý sản phẩm trong hệ thống
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Khởi tạo controller với các dependency cần thiết
        /// </summary>
        /// <param name="productService">Service xử lý logic sản phẩm</param>
        /// <param name="tokenService">Service quản lý token xác thực</param>
        public ProductController(IProductService productService, ITokenService tokenService)
        {
            _productService = productService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Lấy danh sách tất cả sản phẩm trong hệ thống
        /// </summary>
        /// <returns>Danh sách thông tin sản phẩm</returns>
        /// <response code="200">Trả về danh sách sản phẩm</response>
        /// <response code="500">Lỗi server</response>
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

        /// <summary>
        /// Tạo mới một sản phẩm
        /// </summary>
        /// <param name="productData">Thông tin sản phẩm cần tạo</param>
        /// <param name="imageUpload">Ảnh sản phẩm</param>
        /// <returns>Thông báo kết quả tạo sản phẩm</returns>
        /// <response code="200">Tạo sản phẩm thành công</response>
        /// <response code="400">Dữ liệu không hợp lệ</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="404">Không tìm thấy danh mục</response>
        /// <response code="500">Lỗi server hoặc lỗi lưu trữ ảnh</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ProductDTO>> CreateProduct(
            [FromForm] CreateProductDataDTO productData,
            [FromForm] CreateProductImageDTO imageUpload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);

                await _productService.CreateProductWithImageAsync(productData, imageUpload.ProductImage);

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
            catch (AmazonS3Exception s3Ex)
            {
                return StatusCode(500, new { message = $"Lỗi lưu trữ ảnh: {s3Ex.Message}", errorCode = s3Ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <param name="prodId">ID của sản phẩm cần cập nhật</param>
        /// <param name="productDto">Thông tin cần cập nhật</param>
        /// <param name="imageDto">Ảnh sản phẩm mới (nếu có)</param>
        /// <returns>Thông báo kết quả cập nhật</returns>
        /// <response code="200">Cập nhật sản phẩm thành công</response>
        /// <response code="400">Dữ liệu không hợp lệ</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="404">Không tìm thấy sản phẩm hoặc danh mục</response>
        /// <response code="500">Lỗi server hoặc lỗi lưu trữ ảnh</response>
        [HttpPut("{prodId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(
            string prodId,
            [FromForm] UpdateProductDTO productDto,
            [FromForm] UpdateProductImageDTO? imageDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);

                await _productService.UpdateProductWithImageAsync(prodId, productDto, imageDto);

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
            catch (AmazonS3Exception s3Ex)
            {
                return StatusCode(500, new { message = $"Lỗi lưu trữ ảnh: {s3Ex.Message}", errorCode = s3Ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        /// <summary>
        /// Xóa một sản phẩm khỏi hệ thống
        /// </summary>
        /// <param name="prodId">ID của sản phẩm cần xóa</param>
        /// <returns>Thông báo kết quả xóa sản phẩm</returns>
        /// <response code="200">Xóa sản phẩm thành công</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="404">Không tìm thấy sản phẩm</response>
        /// <response code="500">Lỗi server hoặc lỗi xóa ảnh</response>
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
            catch (AmazonS3Exception s3Ex)
            {
                return StatusCode(500, new { message = $"Lỗi xóa ảnh sản phẩm: {s3Ex.Message}", errorCode = s3Ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }
    }
}