using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Sites;

public sealed class Timeframe : ValueObject
{
    public static readonly Timeframe Always = new()
    {
        Date = TimeframeDate.Always,
        Days = TimeframeDays.Always,
        Time = TimeframeTime.Always
    };

#pragma warning disable CS8618
    private Timeframe() //ORM
#pragma warning restore CS8618
    {
    }

    public Timeframe(TimeframeDate date, TimeframeTime time, TimeframeDays days)
    {
        Date = date;
        Time = time;
        Days = days;
    }

    public TimeframeDate Date { get; init; }
    public TimeframeTime Time { get; init; }
    public TimeframeDays Days { get; set; }

    protected override IEnumerable<IComparable?> EqualityValues
    {
        get
        {
            yield return Date;
            yield return Time;
            yield return Days;
        }
    }

    public class TimeframeTime : ValueObject
    {
        public static readonly TimeframeTime Always = new()
        {
            From = TimeOnly.MinValue,
            To = TimeOnly.MaxValue
        };

        public TimeOnly From { get; init; }
        public TimeOnly To { get; init; }

        protected override IEnumerable<IComparable?> EqualityValues
        {
            get
            {
                yield return From;
                yield return To;
            }
        }
    }

    public class TimeframeDate : ValueObject
    {
        public static readonly TimeframeDate Always = new()
        {
            From = DateOnly.FromDateTime(DateTime.UtcNow),
            To = null
        };

        public DateOnly From { get; init; }
        public DateOnly? To { get; init; }

        protected override IEnumerable<IComparable?> EqualityValues
        {
            get
            {
                yield return From;
                yield return To;
            }
        }
    }

    public class TimeframeDays : ValueObject
    {
        public static readonly TimeframeDays Always = new()
        {
            Sunday = true,
            Monday = true,
            Tuesday = true,
            Wednesday = true,
            Thursday = true,
            Friday = true,
            Saturday = true
        };

        public bool Sunday { get; init; }
        public bool Monday { get; init; }
        public bool Tuesday { get; init; }
        public bool Wednesday { get; init; }
        public bool Thursday { get; init; }
        public bool Friday { get; init; }
        public bool Saturday { get; init; }

        protected override IEnumerable<IComparable?> EqualityValues
        {
            get
            {
                yield return Sunday;
                yield return Monday;
                yield return Tuesday;
                yield return Wednesday;
                yield return Thursday;
                yield return Friday;
                yield return Saturday;
            }
        }
    }
}