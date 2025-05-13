namespace Database_Benchmarking.Domain.Entities;

public class EntityId
{
    public string Value { get; private set; }

    public EntityId(string value)
    {
        Value = value;
    }

    private EntityId() { } // <-- Nødvendigt for EF
}
