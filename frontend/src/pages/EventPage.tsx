import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router'
import { api, NotFoundError, type EventDetails, type SeatMap } from '../api'
import { SeatMapView } from '../components/SeatMapView'
import { SelectionSummary, type SelectedSeat } from '../components/SelectionSummary'
import { formatDate, formatTime } from '../format'

const REFRESH_INTERVAL_MS = 10_000

export function EventPage() {
  const { id } = useParams()

  // A new key resets the selection when navigating from one event to another.
  return id ? <EventView key={id} eventId={id} /> : null
}

type LoadStatus = 'loading' | 'ready' | 'not-found' | 'error'

function EventView({ eventId }: { eventId: string }) {
  const [status, setStatus] = useState<LoadStatus>('loading')
  const [details, setDetails] = useState<EventDetails | null>(null)
  const [seatMap, setSeatMap] = useState<SeatMap | null>(null)
  const [updatedAt, setUpdatedAt] = useState<Date | null>(null)
  const [selectedIds, setSelectedIds] = useState<ReadonlySet<string>>(new Set())

  useEffect(() => {
    const controller = new AbortController()

    Promise.all([api.getEvent(eventId, controller.signal), api.getSeatMap(eventId, controller.signal)])
      .then(([eventDetails, map]) => {
        setDetails(eventDetails)
        setSeatMap(map)
        setUpdatedAt(new Date())
        setStatus('ready')
      })
      .catch((error: unknown) => {
        if (!controller.signal.aborted) {
          setStatus(error instanceof NotFoundError ? 'not-found' : 'error')
        }
      })

    // Keep the map live: other people's reservations show up without reloading the page.
    const refresh = window.setInterval(() => {
      if (document.visibilityState !== 'visible') {
        return
      }

      api
        .getSeatMap(eventId, controller.signal)
        .then((map) => {
          setSeatMap(map)
          setUpdatedAt(new Date())
        })
        .catch(() => {
          // Keep showing the last known map; the next refresh will try again.
        })
    }, REFRESH_INTERVAL_MS)

    return () => {
      controller.abort()
      window.clearInterval(refresh)
    }
  }, [eventId])

  if (status === 'loading') {
    return (
      <main className="page">
        <p className="notice">Loading event…</p>
      </main>
    )
  }

  if (status !== 'ready' || !details || !seatMap) {
    return (
      <main className="page">
        <p className={status === 'not-found' ? 'notice' : 'notice notice-error'}>
          {status === 'not-found' ? 'This event is not on sale.' : 'Could not load the event. Is the API running?'}
        </p>
        <Link to="/">Back to events</Link>
      </main>
    )
  }

  const toggleSeat = (seatId: string) => {
    setSelectedIds((current) => {
      const next = new Set(current)
      if (next.has(seatId)) {
        next.delete(seatId)
      } else {
        next.add(seatId)
      }
      return next
    })
  }

  // Derived from the latest map, so a seat that someone else takes in the meantime drops out of the selection.
  const selectedSeats: SelectedSeat[] = seatMap.sections.flatMap((section) =>
    section.seats
      .filter((seat) => selectedIds.has(seat.seatId) && !seat.isTaken)
      .map((seat) => ({ ...seat, sectionName: section.name, price: section.price })),
  )

  return (
    <main className="page">
      <Link to="/" className="back-link">
        ← All events
      </Link>
      <header className="page-header">
        <h1>{details.title}</h1>
        <p className="event-meta">{formatDate(details.startsAt)}</p>
        <p className="muted">
          {details.venueName} · {details.address}, {details.city}
        </p>
      </header>

      <div className="event-layout">
        <section className="panel" aria-labelledby="seat-map-heading">
          <div className="panel-header">
            <h2 id="seat-map-heading">Choose your seats</h2>
            {updatedAt && (
              <span className="live-badge" title={`Refreshes every ${REFRESH_INTERVAL_MS / 1000} seconds`}>
                <span className="live-dot" aria-hidden="true" />
                Live · updated {formatTime(updatedAt)}
              </span>
            )}
          </div>
          <SeatMapView seatMap={seatMap} selectedIds={selectedIds} onToggle={toggleSeat} />
        </section>

        <SelectionSummary seats={selectedSeats} onRemove={toggleSeat} onClear={() => setSelectedIds(new Set())} />
      </div>
    </main>
  )
}
