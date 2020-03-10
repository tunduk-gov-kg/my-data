using System.Data;
using System.Data.Common;
using AutoMapper;

namespace MyData.Infrastructure.XRoad.AutoMapper
{
    // ReSharper disable once InconsistentNaming
    public class DbDataReader_XRoadLog_Converter : ITypeConverter<DbDataReader, XRoadLog>
    {
        public XRoadLog Convert(DbDataReader source, XRoadLog destination, ResolutionContext context)
        {
            return new XRoadLog
            {
                Id = source.GetInt64("id"),
                QueryId = !source.IsDBNull("queryid") ? source.GetString("queryid") : null,
                MemberClass = source.GetString("memberclass"),
                MemberCode = source.GetString("membercode"),
                SubSystemCode = !source.IsDBNull("subsystemcode") ? source.GetString("subsystemcode") : null,
                Message = source.GetString("message"),
                Time = source.GetInt64("time"),
                Attachment = !source.IsDBNull("attachment") ? source.GetInt64("attachment") : (long?) null,
                XRequestId = !source.IsDBNull("xrequestid") ? source.GetString("xrequestid") : null,
                Response = source.GetBoolean("response")
            };
        }
    }
}