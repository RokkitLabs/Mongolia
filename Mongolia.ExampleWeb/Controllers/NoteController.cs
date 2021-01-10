using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Mongolia.ExampleWeb.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Mongolia;

namespace Mongolia.ExampleWeb.Controllers
{
	[ApiController]
	[Microsoft.AspNetCore.Mvc.Route("[controller]")]
	public class NoteController : ControllerBase
	{
		private readonly ILogger<NoteController> logger;
		private readonly DBRepository<Note> noteRepo;

		public NoteController(ILogger<NoteController> _logger, DBRepository<Note> _noteRepo)
		{
			logger = _logger;
			noteRepo = _noteRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Note>>> Get()
		{
			IList<Note> notes = await noteRepo.FindAll();
			return Ok(notes);
		}

		[HttpPost]
		public async Task<ActionResult<ObjectId>> Post([FromBody] Note newNote)
		{
			Note createdNote = await noteRepo.Create(newNote);
			return Ok(createdNote.ID.ToString());
		}

		[HttpDelete]
		public async Task<ActionResult<bool>> Delete([FromBody] string id)
		{
			if (id == null)
				return BadRequest();

			if (!ObjectId.TryParse(id, out ObjectId noteId))
				return BadRequest();

			var note = await noteRepo.FindOne(noteId);
			
			if (note == null)
				return NotFound();

			var deleted = await note.Delete();

			if (deleted)
				return Ok();
			
			return StatusCode(500);
		}
	}
}