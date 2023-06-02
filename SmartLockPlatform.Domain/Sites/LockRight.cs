using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Sites;

public sealed class LockRight : Entity
{
    
#pragma warning disable CS8618
    private LockRight() //ORM
#pragma warning restore CS8618
    {
    }
    public LockRight(MemberGroup group, Timeframe timeframe)
    {
        Group = group;
        Timeframe = timeframe;
    }

    public MemberGroup Group { get; init; }
    public Timeframe Timeframe { get; init; }
}