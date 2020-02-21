using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestSQLite.Repository.SQLite;
using TestSQLite.Repository.SQLite.Entities;
using TestSQLite.WebAPI.ViewModels;

namespace testMediator.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class DocumentObjectController : ControllerBase
    {
        private readonly ILogger<DocumentObjectController> _logger;
        private readonly SQLiteContext _context;

        public DocumentObjectController(ILogger<DocumentObjectController> logger, SQLiteContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(IList<DocumentObject>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<DocumentObject>> Get(Guid id)
        {
            this._logger.LogDebug($"{this.GetType().Name}.{nameof(this.Get)}", new { id });

            var result = await this._context.DocumentObject.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
                return this.NotFound("Resource not found.");

            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<DocumentObject>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DocumentObject>>> Get(Guid? userId, DateTime? created, bool? active)
        {
            this._logger.LogDebug($"{this.GetType().Name}.{nameof(this.Get)}", new { userId });
            
            var query = this._context.DocumentObject.AsQueryable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            if (created != null)
                query = query.Where(x => x.Created == created);
            if (active != null)
                query = query.Where(x => x.Active == active);

            var result = await query.ToListAsync();

            return this.Ok(result);
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(IList<DocumentObject>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<string>> Delete(Guid id)
        {
            this._logger.LogDebug($"{this.GetType().Name}.{nameof(this.Get)}", new { id });

            var result = this._context.DocumentObject.FirstOrDefault(x => x.Id == id);
            if (result == null)
                return this.NotFound("Resource not found.");

            this._context.DocumentObject.Remove(result);
            var removed = await this._context.SaveChangesAsync() > 0;

            return this.Ok("Removed resource.");
        }

        [HttpPost]
        [ProducesResponseType(typeof(DocumentObject), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> Post([FromBody]DocumentObjectViewModel request)
        {
            try
            {
                var entity = request.GetEntity();
                this._context.Add(entity);
                var saved = await this._context.SaveChangesAsync();

                var result = await this._context.DocumentObject.FirstOrDefaultAsync(x => x.Id == entity.Id);
                return this.Created(result.Id.ToString(), result);
            }
            catch (Exception ex)
            {
                return this.BadRequest($"Message: {ex.Message} InnerMessage: {ex.InnerException.Message}");
            }         
        }
    }
}
