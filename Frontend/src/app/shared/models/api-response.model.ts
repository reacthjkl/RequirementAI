export interface ApiResponse<T> {
  successful: boolean;
  data: T | null;
  message: string;
}
