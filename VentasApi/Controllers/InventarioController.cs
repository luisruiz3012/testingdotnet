using Microsoft.AspNetCore.Mvc;
using InventarioLibrary;
using InventarioLibrary.Models;

namespace VentasApi.Controllers
{
    [ApiController]
    [Route("api/inventario")]
    public class InventarioController : ControllerBase
    {
        private readonly DB _db;
        private readonly Metodos inventarioLibrary;

        public InventarioController()
        {
            _db = new DB();
            inventarioLibrary = new Metodos();
        }

        [HttpGet]
        public dynamic Get()
        {
            try
            {
                var request = inventarioLibrary.Get();

                if (request == null) { return NotFound(); }

                return request;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public dynamic GetById(int id)
        {
            try
            {
                var request = inventarioLibrary.GetById(id);

                if (request == null) { return NotFound(); }

                return request;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("")]
        public dynamic Create([FromBody] Inventario producto)
        {
            try
            {
                var request = inventarioLibrary.Create(producto);

                if (request == null) { return BadRequest(); }

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public dynamic Update(int id, [FromBody] Inventario producto)
        {
            try
            {
                var request = inventarioLibrary.Update(id, producto);

                if (request == null) { return NotFound(); }

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public dynamic Delete(int id)
        {
            try
            {
                var request = inventarioLibrary.Delete(id);

                if (request == null) { return NotFound(); };

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
