using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Web_API_With_AOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;
        public UsersController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetAllUsers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var users = await connection.QueryAsync<Users>("SELECT * FROM tbl_clubUsers");
            return Ok(users);

        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<List<Users>>> GetUser(int Id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var user = await connection.QueryFirstAsync<Users>("SELECT * FROM tbl_clubUsers WHERE userID = @Id", new { Id = Id });
            return Ok(user);

        }
    }
}
