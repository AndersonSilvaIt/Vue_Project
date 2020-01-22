using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSchool_API.Data;
using ProjectSchool_API.Models;

namespace ProjectSchool_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : Controller
    {
        public IRepository _repo { get; }
        public ProfessorController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repo.GetAllProfessoresAsync(true);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpGet("{ProfessorId}")]
        public async Task<IActionResult> getByProfessorId(int ProfessorId)
        {
            try
            {
                var results = await _repo.GetProfessorAsyncById(ProfessorId, true);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Professor model)
        {
            try
            {
                _repo.Add(model);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/aluno/{model.Id}", model);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return BadRequest();
        }


        [HttpPut("{ProfessorId}")]
        public async Task<IActionResult> put(int ProfessorId, Professor model)
        {
            try
            {
               var professor = _repo.GetProfessorAsyncById(ProfessorId, false);
               if(professor == null) return NotFound();

               _repo.Update(professor);
               if(await _repo.SaveChangesAsync()){
                   return Created($"/api/Professor/{model.Id}", model);
               }

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return BadRequest();
        }


        [HttpDelete("{ProfessorId}")]
        public async Task<IActionResult> delete(int ProfessorId)
        {
            try
            {
               var professor = _repo.GetProfessorAsyncById(ProfessorId, false);
               if(professor == null) return NotFound();

               _repo.Delete(professor);

               if(await _repo.SaveChangesAsync()){
                  return Ok();
               }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

            return BadRequest();
        }
    }
}