using BolsoCheio.Business.DTOs;
using BolsoCheio.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsoCheio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Obtém todos os produtos
        /// </summary>
        /// <returns>Lista de produtos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Obtém um produto por ID
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <returns>Produto encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"Produto com ID {id} não encontrado.");

            return Ok(product);
        }

        /// <summary>
        /// Cria um novo produto
        /// </summary>
        /// <param name="createProductDto">Dados do produto a ser criado</param>
        /// <returns>Produto criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.CreateProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Atualiza um produto existente
        /// </summary>
        /// <param name="id">ID do produto a ser atualizado</param>
        /// <param name="updateProductDto">Dados atualizados do produto</param>
        /// <returns>Produto atualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.UpdateProductAsync(id, updateProductDto);
            if (product == null)
                return NotFound($"Produto com ID {id} não encontrado.");

            return Ok(product);
        }

        /// <summary>
        /// Remove um produto
        /// </summary>
        /// <param name="id">ID do produto a ser removido</param>
        /// <returns>Confirmação da remoção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
                return NotFound($"Produto com ID {id} não encontrado.");

            return NoContent();
        }

        /// <summary>
        /// Obtém apenas produtos ativos
        /// </summary>
        /// <returns>Lista de produtos ativos</returns>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetActiveProducts()
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Pesquisa produtos por nome
        /// </summary>
        /// <param name="name">Nome ou parte do nome do produto</param>
        /// <returns>Lista de produtos encontrados</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("O parâmetro 'name' é obrigatório.");

            var products = await _productService.SearchProductsByNameAsync(name);
            return Ok(products);
        }
    }
}