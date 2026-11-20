import type { SeatMapSeat } from '../api'
import { formatPrice } from '../format'

export type SelectedSeat = SeatMapSeat & {
  sectionName: string
  price: number
}

type SelectionSummaryProps = {
  seats: SelectedSeat[]
  onRemove: (seatId: string) => void
  onClear: () => void
}

export function SelectionSummary({ seats, onRemove, onClear }: SelectionSummaryProps) {
  const total = seats.reduce((sum, seat) => sum + seat.price, 0)

  return (
    <aside className="panel summary" aria-labelledby="summary-heading">
      <h2 id="summary-heading">Your selection</h2>

      {seats.length === 0 ? (
        <p className="muted">Click a free seat on the map to select it.</p>
      ) : (
        <>
          <ul className="summary-list">
            {seats.map((seat) => (
              <li key={seat.seatId}>
                <span className="summary-seat">
                  <span>
                    Row {seat.row}, seat {seat.number}
                  </span>
                  <span className="muted small">{seat.sectionName}</span>
                </span>
                <span className="summary-price">{formatPrice(seat.price)}</span>
                <button
                  type="button"
                  className="icon-button"
                  aria-label={`Remove row ${seat.row}, seat ${seat.number}`}
                  onClick={() => onRemove(seat.seatId)}
                >
                  ×
                </button>
              </li>
            ))}
          </ul>
          <div className="summary-total">
            <span>
              Total ({seats.length} {seats.length === 1 ? 'seat' : 'seats'})
            </span>
            <strong>{formatPrice(total)}</strong>
          </div>
          <button type="button" className="text-button" onClick={onClear}>
            Clear selection
          </button>
        </>
      )}

      <button type="button" className="primary-button" disabled>
        Reserve seats
      </button>
      <p className="muted small">Reservations are coming soon.</p>
    </aside>
  )
}
