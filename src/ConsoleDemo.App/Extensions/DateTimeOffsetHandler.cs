using System;
using System.Data;
using Dapper;

namespace ConsoleDemo.App.Extensions
{
    /// <summary>
    /// dapper 读写时候用UTC时区
    /// </summary>
    public class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
        {
            var dateTime = DateTime.SpecifyKind(value.UtcDateTime, DateTimeKind.Utc);
            var datetimeOffset = dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                ? DateTimeOffset.MinValue
                : new DateTimeOffset(dateTime);
            parameter.Value = datetimeOffset;
        }

        public override DateTimeOffset Parse(object value)
        {
            var dateTime = DateTime.SpecifyKind(Convert.ToDateTime(value), DateTimeKind.Utc);
            var datetimeOffset = dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                ? DateTimeOffset.MinValue
                : new DateTimeOffset(dateTime);
            return datetimeOffset;
        }
    }
}