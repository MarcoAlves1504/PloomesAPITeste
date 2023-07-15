using Microsoft.AspNetCore.Mvc;
using PloomesAPITeste.Data;
using PloomesAPITeste.Models;

namespace PloomesAPITeste.Controllers {
    /// <summary>
    /// Controlador de contas de usuários cadastrados no BD
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserAccountsController : ControllerBase {
        private readonly APIDbContext _db;
        /// <summary>
        /// Inicializa o controlador com a conexão com o BD
        /// </summary>
        /// <param name="db"></param>
        public UserAccountsController(APIDbContext db) {
            this._db = db;
        }

        /// <summary>
        /// Retorna a lista de todos os usuários
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Retorna a lista de usuário</response>
        [HttpGet("AllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUsers() {
            IEnumerable<UserAccount> UserAccountsFromDb = _db.UserAccounts.ToList();
            return Ok(UserAccountsFromDb);
        }

        /// <summary>
        /// Retorna um usuário específico a partir de seu ID
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <response code="200">Retorna o usuário solicitado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("UserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsersById(int id) {
            var UserAccountFromDb = _db.UserAccounts.Find(id);
            if (UserAccountFromDb == null) {
                return NotFound();
            }
            return Ok(UserAccountFromDb);
        }

        /// <summary>
        /// Retorna a lista de canais aos quais um usuário possui acesso
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <response code="200">Retorna a lista de canais</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("JoinedChannels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserChannels(int id) {
            var UserAccountFromDb = _db.UserAccounts.Find(id);
            if (UserAccountFromDb == null) {
                return NotFound();
            }
            var userJoinedChannels=_db.UserChannelJoinsLog.Where(j => (j.UserId == id)).Join(_db.TextChannels,j=>j.ChannelId,c=>c.Id,(j,c)=> new {
                j.ChannelId,
                c.ChannelName
            });
            return Ok(userJoinedChannels);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /NewUser
        ///     {
        ///         "id": 1
        ///         "username": "João"
        ///         "aboutMe": "Oi, eu sou o João" 
        ///         "accountCreation": "2023-07-14T21:49:09.705Z"
        ///     }
        /// </remarks>
        /// <response code="201">Retorna o usuário recém-criado</response>
        /// <response code="400">No caso de erros de validação do usuário</response>
        [HttpPost("NewUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(UserAccount acc) {
            if (_db.UserAccounts.Any(u => u.Username == acc.Username)) {
                ModelState.AddModelError("UsernameAlreadyTaken", "This username is already taken");
            }
            if (ModelState.IsValid) {
                _db.UserAccounts.Add(acc);
                _db.SaveChanges();
                return Created("/api/UserAccounts/UserById?id=" + acc.Id, acc);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Faz alterações ao perfil de um usuário
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <param name="newUserName"></param>
        /// <param name="newAboutMe"></param>
        /// <response code="200">Retorna o perfil do usuário após as alterações</response>
        /// <response code="400">No caso de erros de validação das alterações do usuário</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpPatch("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, string newUserName, string? newAboutMe) {
            var UserAccountFromDb = _db.UserAccounts.Find(id);
            if (UserAccountFromDb == null) {
                return NotFound();
            }
            if (_db.UserAccounts.Any(u => (u.Username == newUserName && u != UserAccountFromDb))) {
                ModelState.AddModelError("UsernameAlreadyTaken", "This username is already taken");
            }
            UserAccountFromDb.Username = newUserName;
            UserAccountFromDb.AboutMe = newAboutMe;
            if (TryValidateModel(UserAccountFromDb)) {
                _db.UserAccounts.Update(UserAccountFromDb);
                _db.SaveChanges();
                return Ok(UserAccountFromDb);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Adiciona um usuário específico a um canal baseado em seus IDs
        /// </summary>
        /// <returns></returns>
        /// <param name="userId"></param>
        /// <param name="channelId"></param>
        /// <response code="200">O usuário foi adicionado ao canal de texto</response>
        /// <response code="400">No caso de erros de validação da requisição</response>
        /// <response code="404">Usuário ou canal não encontrado</response>
        [HttpPatch("JoinChannel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult JoinChannel(int userId, int channelId) {
            if (_db.UserAccounts.Find(userId) == null) {
                return NotFound("User not found");
            }
            if (_db.TextChannels.Find(channelId) == null) {
                return NotFound("Channel not found");
            }
            if (_db.UserChannelJoinsLog.Any(j => (j.UserId == userId && j.ChannelId == channelId))) {
                ModelState.AddModelError("UserAlreadyJoinedChannel", "This user has already joined this channel");
            }
            if(ModelState.IsValid) {
                UserChannelJoinLog chJoin = new(userId, channelId);
                _db.UserChannelJoinsLog.Add(chJoin);
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Remove um usuário específico de um canal baseado em seus IDs
        /// </summary>
        /// <returns></returns>
        /// <param name="userId"></param>
        /// <param name="channelId"></param>
        /// <response code="200">O usuário foi removido do canal de texto</response>
        /// <response code="400">O usuário informado não estava nesse canal</response>
        [HttpPatch("LeaveChannel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult LeaveChannel(int userId, int channelId) {
            var chJoin = _db.UserChannelJoinsLog.Find(userId, channelId);
            if (chJoin == null) {
                return BadRequest("This user isn't on this channel");
            }
            _db.UserChannelJoinsLog.Remove(chJoin);
            _db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Exclui um usuário específico
        /// </summary>
        /// <returns></returns>
        /// <param name="id"></param>
        /// <response code="204">O usuário foi excluído</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpDelete("DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id) {
            var UserAccountFromDb = _db.UserAccounts.Find(id);
            if (UserAccountFromDb == null) {
                return NotFound();
            }
            _db.UserAccounts.Remove(UserAccountFromDb);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
