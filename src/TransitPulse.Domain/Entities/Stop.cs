using TransitPulse.Domain.ValueObjects;

namespace TransitPulse.Domain.Entities;

public class Stop
{
    // Represents a physical transport stop or station.
    //
    // Private setters protect the entity from arbitrary changes.
    // State changes must occur through constructors or domain methods.
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Coordinates Coordinates { get; private set; }

    // A Stop must always have a valid name and coordinates.
    // This constructor is the only way to create a Stop.
    public Stop(string name, Coordinates coordinates)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Stop name is required.",
                nameof(name));

        Name = name;
        Coordinates = coordinates;
    }

}
