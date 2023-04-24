using AspectInjector.Broker;
using Dapper;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Web_API_With_AOP
{
    [Aspect(Scope.Global)]
    [Injection(typeof(YetkiKontrol))]
    public class YetkiKontrol : Attribute
    {

        public YetkiKontrol()
        {

        }

        [Advice(Kind.Before)]
        public void LogEnter([Argument(Source.Method)] MethodBase method)
        {

            var options = method.GetCustomAttribute<YetkiKontrolParameters>();


            using var connection = new SqlConnection("Server=FURKANKHP; Database=footballClub; Trusted_Connection=true;");
            var yetkisiyok = connection.QueryFirstOrDefault<int>("SELECT permissionID FROM tbl_departmentPermission WHERE permissionID = @permissionID and departmentID = @departmentID", new { permissionID =(int)options.Name, departmentID = options.DepartmentId }) != null;


            if (yetkisiyok)
            {
                throw new Exception("Bu kulllanıcının buradaki işleme yetkisi yoktur.");
            }
        }
    }

    public class YetkiKontrolParameters : Attribute
    {
        public YetkiKontrolType Name { get; set; }
        public int DepartmentId { get; set; }

    }
    public enum YetkiKontrolType
    {
        GetAllUsers = 1,
        GetUser = 2,
        CreateUser = 3,
        UpdateUser = 4,
        DeleteUser = 5
    }

}
