using EventApi.Data;
using EventApi.Dtos;
using EventApi.Model;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {

        private readonly EventContext _context;
        public EventController(EventContext context)
        {
            _context = context;
        }
        // GET: api/<EventController>
        [Authorize]
        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return _context.Event.OrderBy(t => t.Id).ToList();
        }

        // GET api/<EventController>/5
        [Authorize]
        [HttpGet("{id}")]
        public Event Get(int id)
        {
            return _context.Event.Where(e => e.Id == id).FirstOrDefault();
        }

        // POST api/<EventController>
        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventDto dto)
        {
            if (dto != null)
            {
                var data = new Event
                {
                    Attendees = dto.Attendees,
                    Data_Evento = dto.Data_Evento,
                    Nome_Evento = dto.Nome_Evento,

                };
                await _context.AddAsync(data);
                
                _context.SaveChanges();
                
                //var route = Url.Action("GetEventById", "Event", new { data.Id }, Request.Scheme);
                return Ok("Success");
            }

            return BadRequest("Form is null");
        }

        // PUT api/<EventController>/5
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public void Put(int id, EventDto dto)
        {
            var todoItemToUpdate = _context.Event.FirstOrDefault(item => item.Id == id);

            if (todoItemToUpdate != null)
            {
                todoItemToUpdate.Attendees = dto.Attendees;
                todoItemToUpdate.Data_Evento = dto.Data_Evento;
                todoItemToUpdate.Nome_Evento = dto.Nome_Evento;

                _context.SaveChanges();
            }
        }

        // DELETE api/<EventController>/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var itemToRemove = _context.Event.SingleOrDefault(x => x.Id == id);

            if (itemToRemove != null)
            {
                _context.Event.Remove(itemToRemove);
                _context.SaveChanges();
            }
        }
    }
}
