using Microsoft.AspNetCore.Mvc;
using PloomesAPITeste.Data;
using PloomesAPITeste.Models;

namespace PloomesAPITeste.Controllers {
    /// <summary>
    /// Controlador de mensagens de texto cadastradas no BD
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TextMessagesController : ControllerBase {
        private readonly APIDbContext _db;
        /// <summary>
        /// Inicializa o controlador com a conexão com o BD
        /// </summary>
        /// <param name="db"></param>
        public TextMessagesController(APIDbContext db) {
            this._db = db;
        }

        /// <summary>
        /// Retorna uma lista de todas as mensagens do banco de dados
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Retorna a lista de mensagens</response>
        [HttpGet("AllMessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllMessages() {
            IEnumerable<TextMessage> textMessagesFromDb = _db.TextMessages.ToList();
            return Ok(textMessagesFromDb);
        }

        /// <summary>
        /// Retorna uma mensagem específica a partir de seu ID
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <response code="200">Retorna a mensagem solicitada</response>
        /// <response code="404">Mensagem não encontrada</response>
        [HttpGet("MessageById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllMessages(int id) {
            var textMessagesFromDb = _db.TextMessages.Find(id);
            if (textMessagesFromDb == null) {
                return NotFound();
            }
            return Ok(textMessagesFromDb);
        }

        /// <summary>
        /// Cria uma mensagem nova, enviada num canal de texto específico por um usuário específico
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /NewMessage
        ///     {
        ///         "messageId": 1
        ///         "senderId": 2
        ///         "channelId": 3
        ///         "messageContent": "Oi, como vai?"
        ///         "messageSentTime": "2023-07-14T21:49:09.705Z"
        ///     }
        /// </remarks>
        /// <response code="201">Retorna a mensagem recém-criada</response>
        /// <response code="400">Erros de validação da mensagem</response>
        [HttpPost("NewMessage")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(TextMessage mess) {
            if (!_db.TextChannels.Any(c => c.Id == mess.ChannelId)) {
                ModelState.AddModelError("InvalidChannelID", "Mensagem não enviada - canal não existe");
            }
            if (!_db.UserAccounts.Any(u => u.Id == mess.SenderId)) {
                ModelState.AddModelError("InvalidUserID", "Mensagem não enviada - usuário não existe");
            }
            if(!_db.UserChannelJoinsLog.Any((j => (j.UserId == mess.SenderId && j.ChannelId == mess.ChannelId)))) {
                ModelState.AddModelError("UserNotInChannel", "Mensagem não enviada - remetente não tem acesso a esse canal");
            }
            if (ModelState.IsValid) {
                _db.TextMessages.Add(mess);
                _db.SaveChanges();
                return Created("/api/TextMessages/MessageById?id=" + mess.MessageId, mess);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Edita uma mensagem
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <param name="newMessageContent"></param>
        /// <response code="200">Retorna a mensagem após as alterações</response>
        /// <response code="400">No caso de erros de validação da edição da mensagem</response>
        /// <response code="404">Mensagem não encontrada</response>
        [HttpPatch("UpdateMessage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Edit(int id, string newMessageContent) {
            var textMessageFromDb = _db.TextMessages.Find(id);
            if (textMessageFromDb == null) {
                return NotFound();
            }
            textMessageFromDb.MessageContent = newMessageContent;
            if (TryValidateModel(textMessageFromDb)) {
                _db.TextMessages.Update(textMessageFromDb);
                _db.SaveChanges();
                return Ok(textMessageFromDb);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Exclui uma mensagem específica a partir de seu ID
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <response code="204">A mensagem foi excluída</response>
        /// <response code="404">Mensagem não encontrada</response>
        [HttpDelete("DeleteMessage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id) {
            var textMessageFromDb = _db.TextMessages.Find(id);
            if (textMessageFromDb == null) {
                return NotFound();
            }
            _db.TextMessages.Remove(textMessageFromDb);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
