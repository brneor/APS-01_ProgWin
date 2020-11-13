using System.Collections.Generic;
using APS_02.DAOs;
using APS_02.Models;
using Microsoft.AspNetCore.Mvc;

namespace APS_02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        // GET: api/<ProdutoController>
        [HttpGet]
        public IEnumerable<Produto> Get()
        {
            return (new ProdutoDAO()).RetornarTodos();
        }

        // GET api/<ProdutoController>/5
        [HttpGet("{id}")]
        public IActionResult GetId(int id)
        {
            return new ObjectResult((new ProdutoDAO()).RetornarPorId(id));
        }

        // POST api/<ProdutoController>
        [HttpPost]
        public IActionResult Post([FromBody] Produto obj)
        {
            try
            {
                // Não precisa setar ID porque a base é autoincrement
                (new ProdutoDAO()).Inserir(obj);
                return CreatedAtAction(nameof(GetId), new { id = obj.Id }, obj);
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT api/<ProdutoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Produto obj)
        {
            var dao = new ProdutoDAO();
            try
            {
                dao.Atualizar(obj);
                return NoContent();
            }
            catch
            {
                if (dao.RetornarPorId(id) == null)
                    return NotFound();

                return BadRequest();
            }
        }

        // DELETE api/<ProdutoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                (new ProdutoDAO()).RemoverPorId(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}