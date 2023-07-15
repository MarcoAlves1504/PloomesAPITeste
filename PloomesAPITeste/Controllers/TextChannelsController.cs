using Microsoft.AspNetCore.Mvc;
using PloomesAPITeste.Data;
using PloomesAPITeste.Models;
using Z.EntityFramework.Plus;

namespace PloomesAPITeste.Controllers {
    /// <summary>
    /// Controlador de canais de texto cadastrados no BD
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TextChannelsController : ControllerBase {
        private readonly APIDbContext _db;
        /// <summary>
        /// Inicializa o controlador com a conexão com o BD
        /// </summary>
        /// <param name="db"></param>
        public TextChannelsController(APIDbContext db) {
            this._db = db;
        }

        /// <summary>
        /// Retorna uma lista de todos os canais de texto
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Retorna a lista de canais</response>
        [HttpGet("AllChannels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllChannels() {
            IEnumerable<TextChannel> TextChannelsFromDb = _db.TextChannels.ToList();
            return Ok(TextChannelsFromDb);
        }

        /// <summary>
        /// Retorna um canal específico a partir de seu ID
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <response code="200">Retorna o canal solicitado</response>
        /// <response code="404">Canal não encontrado</response>
        [HttpGet("ChannelById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetChannelsById(int id) {
            var TextChannelFromDb = _db.TextChannels.Find(id);
            if (TextChannelFromDb == null) {
                return NotFound();
            }
            return Ok(TextChannelFromDb);
        }

        /// <summary>
        /// Retorna a lista de membros de um canal específico a partir de seu ID
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <response code="200">Retorna a lista de membros do canal solicitado</response>
        /// <response code="404">Canal não encontrado</response>
        [HttpGet("Members")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetChannelMembers(int id) {
            var TextChannelFromDb = _db.TextChannels.Find(id);
            if (TextChannelFromDb == null) {
                return NotFound();
            }
            var channelMembers = _db.UserChannelJoinsLog.Where(j => (j.ChannelId == id)).Join(_db.UserAccounts, j => j.UserId, u => u.Id, (j, u) => new {
                j.UserId,
                u.Username
            });
            return Ok(channelMembers);
        }

        /// <summary>
        /// Retorna todas as mensagens de um canal específico a partir de seu ID
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <response code="200">Retorna a lista de mensagens do canal solicitado</response>
        /// <response code="404">Canal não encontrado</response>
        [HttpGet("Messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetChannelMessages(int id) {
            var TextChannelFromDb = _db.TextChannels.Find(id);
            if (TextChannelFromDb == null) {
                return NotFound();
            }
            var channelMessages = _db.TextMessages.Where(m => m.ChannelId == id).ToList();
            return Ok(channelMessages);
        }

        /// <summary>
        /// Cria um novo canal de texto
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /NewChannel
        ///     {
        ///         "id": 1
        ///         "channelName": "Geral"
        ///         "channelDescription": "Canal geral para discussões" 
        ///         "createdAt": "2023-07-14T21:49:09.705Z"
        ///     }
        /// </remarks>
        /// <response code="201">Retorna o canal recém-criado</response>
        /// <response code="400">No caso de erros de validação do canal</response>
        [HttpPost("NewChannel")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(TextChannel chan) {
            if (ModelState.IsValid) {
                _db.TextChannels.Add(chan);
                _db.SaveChanges();
                return Created("/api/TextChannels/ChannelById?id=" + chan.Id, chan);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Faz alterações a um canal de texto
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <param name="newChannelName"></param>
        /// <param name="newChannelDesc"></param>
        /// <response code="200">Retorna o canal após as alterações</response>
        /// <response code="400">No caso de erros de validação das alterações do canal</response>
        /// <response code="404">Canal não encontrado</response>
        [HttpPatch("UpdateChannel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, string newChannelName, string? newChannelDesc) {
            var TextChannelFromDb = _db.TextChannels.Find(id);
            if (TextChannelFromDb == null) {
                return NotFound();
            }
            TextChannelFromDb.ChannelName = newChannelName;
            TextChannelFromDb.ChannelDescription = newChannelDesc;
            if (TryValidateModel(TextChannelFromDb)) {
                _db.TextChannels.Update(TextChannelFromDb);
                _db.SaveChanges();
                return Ok(TextChannelFromDb);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Exclui um canal específico e todas as mensagens nele
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <response code="204">O canal foi excluído</response>
        /// <response code="404">Canal não encontrado</response>
        [HttpDelete("DeleteChannel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id) {
            var TextChannelFromDb = _db.TextChannels.Find(id);
            if (TextChannelFromDb == null) {
                return NotFound();
            }
            _db.TextChannels.Remove(TextChannelFromDb);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
