using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;

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
        [YetkiKontrol, YetkiKontrolParameters(Name = YetkiKontrolType.GetAllUsers, DepartmentId = 3)]
        public async Task<ActionResult<List<Users>>> GetAllUsers(/**/)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Users> users = await SelectAllUsers(connection);
            return Ok(users);

        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<List<Users>>> GetUsers(int Id)
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
            return Ok(await SelectAllUsers(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<Users>>> UpdateUser(Users user)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE tbl_clubUsers SET departmentID = @departmentID, userFirstName = @userFirstName, userLastName = @userLastName, userPosition = @userPosition, userCity = @userCity WHERE userID = @userID", user);
            return Ok(await SelectAllUsers(connection));
        }

        [HttpDelete("{userID}")]
        public async Task<ActionResult<List<Users>>> DeleteUser(int userID)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("DELETE FROM tbl_clubUsers WHERE userID = @userID", new { userID = userID });
            return Ok(await SelectAllUsers(connection));
        }

        private static async Task<IEnumerable<Users>> SelectAllUsers(SqlConnection connection)
        {
            return await connection.QueryAsync<Users>("SELECT * FROM tbl_clubUsers");
        }
    }
}
