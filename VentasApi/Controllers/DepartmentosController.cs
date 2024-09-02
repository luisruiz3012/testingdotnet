using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DepartamentosLibrary;
using DepartamentosLibrary.Models;

namespace VentasApi.Controllers
{
    [ApiController]
    [Route("api/departamentos")]
    public class DepartmentosController : ControllerBase
    {
        private readonly DB _db;
        private readonly Metodos departamentosLibrary;

        public DepartmentosController()
        {
            _db = new DB();
            departamentosLibrary = new Metodos();
        }

        [HttpGet]
        [Route("")]
        public dynamic Get()
        {
            try
            {
                var request = departamentosLibrary.Get();

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

        [HttpGet]
        [Route("id")]
        public dynamic GetById(int id)
        {
            try
            {
                var request = departamentosLibrary.GetById(id);

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
        [Route("id")]
        public dynamic Delete(int id)
        {
            try
            {
                var request = departamentosLibrary.Delete(id);

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

        [HttpPost]
        [Route("")]
        public dynamic Create([FromBody] Departamento departamento)
        {
            try
            {
                var request = departamentosLibrary.Create(departamento);

                if (request == null)
                {
                    return BadRequest();
                }

                return request;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public dynamic Update(int id, [FromBody] Departamento departamento)
        {
            try
            {
                var request = departamentosLibrary.Update(id, departamento);

                if (request == null)
                {
                    return BadRequest();
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
