using Ateo_API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ateo_API.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonsController : Controller
    {
        private readonly AteoContext _ateoContext;
        private readonly ILogger<PersonsController> _logger;
        
        public PersonsController(AteoContext ateoContext,ILogger<PersonsController> logger)
        {
            _ateoContext = ateoContext;
            _logger = logger;
        }
        [HttpGet("GetAllPersons")]
        public IEnumerable<Person> GetAllPersons()
        {
            return _ateoContext.Persons;
        }

        [HttpPost("CreatePerson")]
        public IActionResult CreatePerson([FromBody] Person person)
        {
            try
            {
                if (person == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Objet persone reçu n'est pas valide");
                    return BadRequest("Objet personne non valide");
                }

                person.Id = Guid.NewGuid();
                _ateoContext.Persons.Add(person);
                _ateoContext.SaveChanges();
                _logger.LogInformation("Objet persone créé avec succés");


                return CreatedAtRoute("PersonById", new { id = person.Id }, person);
            }
            catch (Exception ex)
            {
                _logger.LogError($"problème lors du création du personne: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePerson(Guid id, [FromBody] Person person)
        {
            try
            {
                if (person == null)
                {
                    _logger.LogError("Objet personne reçu est nulle");
                    return BadRequest("Objet personne nulle");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Objet personne reçu du client est as valide");
                    return BadRequest("Objet personne non valide");
                }


                if (person.Id != id)
                {
                    _logger.LogError($"Personne avec Id: {person.Id}, est pas trouvé dans le db.");
                    return NotFound();
                }

                _ateoContext.Entry(person).State = EntityState.Modified;
                _ateoContext.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problème lors du mise à jour des données d'utilisateur: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePerson(Guid id)
        {
            try
            {
                var person = _ateoContext.Persons.Find(id);
                if (person == null)
                {
                    _logger.LogError($"Personne avec id : {id}, existe pas dans la db.");
                    return NotFound();
                }

                _ateoContext.Persons.Remove(person);
                _ateoContext.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Probleme lors du suppression du personne : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("{id}", Name = "PersonById")]
        public IActionResult GetPersonById(Guid id)
        {
            try
            {
                var person = _ateoContext.Persons.Find(id);

                if (person == null)
                {
                    _logger.LogError($"Persone avec Id : {id}, existe pas dans le db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Retourner la personne avec  id: {id}");
                    return Ok(person);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problème lors de récupération du personne : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{id}", Name = "PersonPatch")]
        public IActionResult PartialPersonUpdate(Guid id , [FromBody] JsonPatchDocument<Person> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var person = _ateoContext.Persons.Find(id);

            if (person == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(person);

            TryValidateModel(person);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _ateoContext.Entry(person).State = EntityState.Modified;
            _ateoContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
