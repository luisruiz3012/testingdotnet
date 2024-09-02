using Microsoft.AspNetCore.Mvc;
using ClientesLibrary.Models;
using ClientesLibrary;

namespace VentasApi.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly DB _db;
        private readonly Metodos clientesLibrary;
        public ClientesController()
        {
            _db = new DB();
            clientesLibrary = new Metodos();
        }

        [HttpGet]
        public dynamic Get()
        {
            try
            {
                var clientes = clientesLibrary.Get();

                if (clientes.Count > 0)
                {
                    return Ok(clientes);
                }

                return NotFound();
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
                var cliente = clientesLibrary.GetById(id);

                if (cliente == null) { return NotFound(); }

                return cliente;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("")]
        public dynamic Create([FromBody] Cliente cliente)
        {
            try
            {
                var requestCliente = clientesLibrary.Create(cliente);

                if (requestCliente == new { error = "There was an error with your request" })
                {
                    return BadRequest();
                }

                return requestCliente;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public dynamic Update(int id, [FromBody] Cliente cliente)
        {
            try
            {
               var request = clientesLibrary.Update(id, cliente);

                if (request == null)
                {
                    return NotFound();
                }

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
                var request = clientesLibrary.Delete(id);

                if (request == null)
                {
                    return NotFound();
                }

                return request;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
