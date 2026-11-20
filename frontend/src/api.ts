// Types mirror the DTOs returned by SeatFlow.Api (SeatFlow.Application/Events).

export type EventSummary = {
  id: string
  title: string
  startsAt: string
  venueName: string
  city: string
}

export type SectionPrice = {
  sectionId: string
  sectionName: string
  price: number
}

export type EventDetails = {
  id: string
  title: string
  startsAt: string
  venueName: string
  city: string
  address: string
  prices: SectionPrice[]
}

export type SeatMapSeat = {
  seatId: string
  row: string
  number: number
  isTaken: boolean
}

export type SeatMapSection = {
  sectionId: string
  name: string
  price: number
  seats: SeatMapSeat[]
}

export type SeatMap = {
  eventId: string
  sections: SeatMapSection[]
}

export class NotFoundError extends Error {}

async function getJson<T>(path: string, signal?: AbortSignal): Promise<T> {
  const response = await fetch(path, { signal, headers: { Accept: 'application/json' } })

  if (response.status === 404) {
    throw new NotFoundError(`${path} was not found`)
  }
  if (!response.ok) {
    throw new Error(`GET ${path} failed with status ${response.status}`)
  }

  return (await response.json()) as T
}

export const api = {
  getEvents: (signal?: AbortSignal) => getJson<EventSummary[]>('/api/events', signal),
  getEvent: (id: string, signal?: AbortSignal) => getJson<EventDetails>(`/api/events/${id}`, signal),
  getSeatMap: (id: string, signal?: AbortSignal) => getJson<SeatMap>(`/api/events/${id}/seats`, signal),
}
