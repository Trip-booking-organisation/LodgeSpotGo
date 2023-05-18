export interface PriceData {
  price: {
    dateRange: {
      from: string;
      to: string;
    };
    price: number;
  };
  accommodationId: string;
}
