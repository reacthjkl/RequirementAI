import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  public isLoading = signal<boolean>(false);
  private totalRequests: number = 0;

  public incrementRequests() {
    this.totalRequests++;

    if (this.totalRequests > 0) this.isLoading.set(true);
  }

  public decrementRequests() {
    this.totalRequests = Math.max(0, this.totalRequests - 1);

    if (this.totalRequests === 0) this.isLoading.set(false);
  }
}
