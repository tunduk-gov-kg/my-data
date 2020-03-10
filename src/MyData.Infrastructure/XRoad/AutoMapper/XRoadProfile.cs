using System.Data.Common;
using AutoMapper;

namespace MyData.Infrastructure.XRoad.AutoMapper
{
    // ReSharper disable once UnusedType.Global
    public class XRoadProfile : Profile
    {
        public XRoadProfile()
        {
            CreateMap<DbDataReader, XRoadLog>().ConvertUsing<DbDataReader_XRoadLog_Converter>();
        }
    }
}