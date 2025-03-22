namespace DeveloperStore.Domain.Common;

public abstract class Entity
{
    public long Id { get; protected set; }

    public override bool Equals(object obj)
    {
        var entity = obj as Entity;
        if (entity == null)
            return false;

        if (GetType() != entity.GetType())
            return false;

        if (Id == 0 || entity.Id == 0)
            return false;

        return Id == entity.Id;
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }
}