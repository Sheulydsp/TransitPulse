# Reliability Score V1

## Purpose

Measure how reliable a public transport route is based on historical operational data.

## Inputs

- Average Delay (minutes)
- Cancellation Rate (%)

## Formula

Score = 100 - (AverageDelay × 2) - (CancellationRate × 5)

## Score Range

0 - 100

## Interpretation

90 - 100 : Excellent

80 - 89 : Good

70 - 79 : Fair

Below 70 : Poor

## Notes

This formula is intentionally simple for the MVP version.

Future versions may include:

- Delay variance
- Peak hour weighting
- Seasonal trends
- Prediction confidence
- Passenger impact metrics
