import type { SeatMap, SeatMapSeat } from '../api'
import { formatPrice } from '../format'

type SeatMapViewProps = {
  seatMap: SeatMap
  selectedIds: ReadonlySet<string>
  onToggle: (seatId: string) => void
}

export function SeatMapView({ seatMap, selectedIds, onToggle }: SeatMapViewProps) {
  return (
    <div className="seat-map">
      <div className="stage">Stage</div>

      {/* Most expensive sections are usually closest to the stage, so they go on top. */}
      {seatMap.sections.toSorted((a, b) => b.price - a.price).map((section) => {
        const freeCount = section.seats.filter((seat) => !seat.isTaken).length

        return (
          <section key={section.sectionId} className="section-block" aria-label={section.name}>
            <header className="section-header">
              <h3>{section.name}</h3>
              <span className="section-price">{formatPrice(section.price)}</span>
              <span className="muted">
                {freeCount} of {section.seats.length} free
              </span>
            </header>

            <div className="seat-rows">
              {groupByRow(section.seats).map(([row, seats]) => (
                <div key={row} className="seat-row">
                  <span className="row-label" aria-hidden="true">
                    {row}
                  </span>
                  {seats.map((seat) => {
                    const isSelected = selectedIds.has(seat.seatId) && !seat.isTaken
                    const state = seat.isTaken ? 'taken' : isSelected ? 'selected' : 'free'

                    return (
                      <button
                        key={seat.seatId}
                        type="button"
                        className={`seat seat-${state}`}
                        disabled={seat.isTaken}
                        aria-pressed={isSelected}
                        aria-label={`${section.name}, row ${row}, seat ${seat.number}${seat.isTaken ? ', taken' : ''}`}
                        title={`Row ${row}, seat ${seat.number}`}
                        onClick={() => onToggle(seat.seatId)}
                      >
                        {seat.number}
                      </button>
                    )
                  })}
                </div>
              ))}
            </div>
          </section>
        )
      })}

      <ul className="legend" aria-label="Legend">
        <li>
          <span className="seat seat-free" aria-hidden="true" /> Free
        </li>
        <li>
          <span className="seat seat-selected" aria-hidden="true" /> Selected
        </li>
        <li>
          <span className="seat seat-taken" aria-hidden="true" /> Taken
        </li>
      </ul>
    </div>
  )
}

// The API returns seats ordered by row and number, so grouping keeps that order.
function groupByRow(seats: SeatMapSeat[]): [string, SeatMapSeat[]][] {
  const rows = new Map<string, SeatMapSeat[]>()

  for (const seat of seats) {
    const row = rows.get(seat.row) ?? []
    row.push(seat)
    rows.set(seat.row, row)
  }

  return [...rows.entries()]
}
