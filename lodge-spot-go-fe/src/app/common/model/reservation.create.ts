export interface ReservationCreate {
  accommodationId: string;
  dateRange: {
    from: string;
    to: string;
  };
  status: string;
  numberOfGuests: number;
  guestId: string;
}
