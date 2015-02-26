namespace Repository.Pattern.Infrastructure
{
    public enum ObjectState
    {
        Unchanged=0,
        Added=1,
        Modified=2,
        Deleted=3,
        Detached=4
    }
}