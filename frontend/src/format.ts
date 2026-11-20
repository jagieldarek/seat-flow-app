const dateFormat = new Intl.DateTimeFormat('en-GB', {
  dateStyle: 'full',
  timeStyle: 'short',
  timeZone: 'Europe/Warsaw',
})

const timeFormat = new Intl.DateTimeFormat('en-GB', { timeStyle: 'medium' })

const priceFormat = new Intl.NumberFormat('pl-PL', { style: 'currency', currency: 'PLN' })

export const formatDate = (iso: string) => dateFormat.format(new Date(iso))

export const formatTime = (date: Date) => timeFormat.format(date)

export const formatPrice = (value: number) => priceFormat.format(value)
