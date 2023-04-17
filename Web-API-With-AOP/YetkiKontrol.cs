using AspectInjector.Broker;
using Dapper;
using System.Data.SqlClient;
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
            var yetkilist = new List<string>() { "GetAllUsers" };
            var options = method.GetCustomAttribute<YetkiKontrolParameters>();
            var _name = options?.Name ?? string.Empty;
            bool yetkisiyok = !yetkilist.Contains(_name);

            if (yetkisiyok)
            {
                throw new Exception("Furkan adlı kulllanıcının bu işleme yetkisi yoktur.");
            }
        }
    }

    public class YetkiKontrolParameters : Attribute
    {
        public string Name { get; set; }
    }
}
