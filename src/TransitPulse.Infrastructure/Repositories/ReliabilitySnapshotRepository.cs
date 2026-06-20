using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;
using TransitPulse.Infrastructure.Persistence;

public class ReliabilitySnapshotRepository : IReliabilitySnapshotRepository
{
    private readonly TransitPulseDbContext _context;

    public ReliabilitySnapshotRepository(TransitPulseDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(ReliabilitySnapshot snapshot, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Saving snapshot {snapshot.Id}");
        await _context.ReliabilitySnapshots.AddAsync(snapshot, cancellationToken);
        //throw new NotImplementedException();
        await _context.SaveChangesAsync(cancellationToken);
        Console.WriteLine("Snapshot saved to PostgreSQL");
    }
}