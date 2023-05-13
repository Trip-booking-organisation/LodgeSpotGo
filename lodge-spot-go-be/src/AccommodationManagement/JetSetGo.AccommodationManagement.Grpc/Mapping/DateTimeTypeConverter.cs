using JetSetGo.AccommodationManagement.Domain.Accommodation.ValueObjects;

namespace JetSetGo.AccommodationManagement.Grpc.Mapping;

using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using System;

public class DateTimeTypeConverter : ITypeConverter<Timestamp, DateTime>, ITypeConverter<DateTime, Timestamp>
{
    public DateTime Convert(Timestamp source, DateTime destination, ResolutionContext context)
    {
        return source.ToDateTime();
    }

    public Timestamp Convert(DateTime source, Timestamp destination, ResolutionContext context)
    {
        return Timestamp.FromDateTime(source);
    }
}

public class DateRangeTypeConverter : ITypeConverter<DateRangeDto, DateRange>, ITypeConverter<DateRange, DateRangeDto>
{
    public DateRange Convert(DateRangeDto source, DateRange destination, ResolutionContext context)
    {
        return new DateRange
        {
            From = context.Mapper.Map<DateTime>(source.From),
            To = context.Mapper.Map<DateTime>(source.To)
        };
    }

    public DateRangeDto Convert(DateRange source, DateRangeDto destination, ResolutionContext context)
    {
        return new DateRangeDto
        {
            From = context.Mapper.Map<Timestamp>(source.From),
            To = context.Mapper.Map<Timestamp>(source.To)
        };
    }
}