export interface ServiceResult<T> {
  message?: string;
  success: boolean;
  result?: T;
}
