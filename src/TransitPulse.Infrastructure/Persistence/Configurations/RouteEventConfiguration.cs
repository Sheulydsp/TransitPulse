using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;

public class RouteEventConfiguration : IEntityTypeConfiguration<RouteEvent>
{
    public void Configure(EntityTypeBuilder<RouteEvent> builder)
    {
        builder.ToTable("route_events");

        builder.HasKey(
            routeEvent => routeEvent.Id);

        builder.Property(
            routeEvent => routeEvent.RouteId)
            .IsRequired();

        builder.Property(
            routeEvent => routeEvent.StopId)
            .IsRequired();

        builder.Property(
            routeEvent => routeEvent.ScheduledTime)
            .IsRequired();

        builder.Property(
            routeEvent => routeEvent.ActualTime)
            .IsRequired();

        builder.Property(
            routeEvent => routeEvent.IsCancelled)
            .IsRequired();
    }
}