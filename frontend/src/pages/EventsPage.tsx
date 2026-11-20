import { useEffect, useState } from 'react'
import { Link } from 'react-router'
import { api, type EventSummary } from '../api'
import { formatDate } from '../format'

export function EventsPage() {
  const [events, setEvents] = useState<EventSummary[] | null>(null)
  const [failed, setFailed] = useState(false)

  useEffect(() => {
    const controller = new AbortController()

    api
      .getEvents(controller.signal)
      .then(setEvents)
      .catch(() => {
        if (!controller.signal.aborted) {
          setFailed(true)
        }
      })

    return () => controller.abort()
  }, [])

  return (
    <main className="page">
      <header className="page-header">
        <h1>Upcoming events</h1>
        <p className="muted">Pick an event to see which seats are still free.</p>
      </header>

      {failed && <p className="notice notice-error">Could not load events. Is the API running?</p>}
      {!failed && events === null && <p className="notice">Loading events…</p>}
      {events?.length === 0 && <p className="notice">No events are on sale right now.</p>}

      <ul className="event-list">
        {events?.map((event) => (
          <li key={event.id}>
            <Link to={`/events/${event.id}`} className="event-card">
              <span className="event-card-date">{formatDate(event.startsAt)}</span>
              <span className="event-card-title">{event.title}</span>
              <span className="muted">
                {event.venueName} · {event.city}
              </span>
              <span className="event-card-cta">Choose seats →</span>
            </Link>
          </li>
        ))}
      </ul>
    </main>
  )
}
