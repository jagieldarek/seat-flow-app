import { Link, Route, Routes } from 'react-router'
import { EventPage } from './pages/EventPage'
import { EventsPage } from './pages/EventsPage'

export default function App() {
  return (
    <>
      <header className="app-header">
        <Link to="/" className="brand">
          <img src="/favicon.svg" alt="" width="28" height="28" />
          SeatFlow
        </Link>
      </header>
      <Routes>
        <Route path="/" element={<EventsPage />} />
        <Route path="/events/:id" element={<EventPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </>
  )
}

function NotFoundPage() {
  return (
    <main className="page">
      <p className="notice">This page does not exist.</p>
      <Link to="/">Back to events</Link>
    </main>
  )
}
