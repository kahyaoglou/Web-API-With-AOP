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
            IEnumerable<Users> users = await SelectAllHeroes(connection);
            return Ok(users);

        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<List<Users>>> GetUser(int Id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var user = await connection.QueryFirstAsync<Users>("SELECT * FROM tbl_clubUsers WHERE userID = @Id", new { Id = Id });
            return Ok(user);

        }

        [HttpPost]
        public async Task<ActionResult<List<Users>>> CreateUser(Users user)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO tbl_clubUsers (departmentID, userFirstName, userLastName, userPosition, userCity) values (@departmentID, @userFirstName, @userLastName, @userPosition, @userCity)", user);
            return Ok(await SelectAllHeroes(connection));
        }

        private static async Task<IEnumerable<Users>> SelectAllHeroes(SqlConnection connection)
        {
            return await connection.QueryAsync<Users>("SELECT * FROM tbl_clubUsers");
        }
    }
}
