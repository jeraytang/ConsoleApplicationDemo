using System.Data;
using Dapper;

namespace ConsoleDemo.App.Extensions
{
    public class StringGuidHandler : SqlMapper.TypeHandler<string>
    {
        public override void SetValue(IDbDataParameter parameter, string value)
        {
            parameter.Value = value;
        }

        public override string Parse(object value)
        {
            return value.ToString();
        }
    }
}