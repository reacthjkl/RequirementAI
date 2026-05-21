import { Injectable, signal } from '@angular/core';
import { Api } from '../../core/services/api';
import { CurrentUserService } from '../../core/services/current-user.service';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { AuthRequest } from '../models/auth-request.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public loggedIn = signal(false);

  private refreshInProgress: Promise<boolean> | null = null;

  constructor(
    private api: Api,
    private cu: CurrentUserService,
  ) {}

  public async authenticate(request: AuthRequest) {
    const response: ApiResponse<null> = await this.api.post(ApiController.Auth, 'login', request);

    this.loggedIn.set(response.successful);

    return response;
  }

  public async logout() {
    const response: ApiResponse<null> = await this.api.post(ApiController.Auth, 'logout');

    this.loggedIn.set(false);

    return response;
  }

  public async refresh() {
    return await this.api.get<boolean>(ApiController.Auth, 'refresh');
  }

  public async checkIsLoggedIn() {
    const response: User | null = await this.cu.get();

    const exists: boolean = response !== null;

    if (exists) {
      this.loggedIn.set(true);
    }

    return exists;
  }

  public async refreshTokenOrLogout() {
    // if refresh is active, return promise, so the caller waites for the existing request
    if (this.refreshInProgress) {
      return this.refreshInProgress;
    }

    // perform request and cache it
    this.refreshInProgress = (async () => {
      const response: ApiResponse<boolean> = await this.refresh();

      if (response.successful) {
        return true;
      }

      await this.logout();
      return false;
    })();

    try {
      return await this.refreshInProgress;
    } finally {
      // remove from cache, when request was performed
      this.refreshInProgress = null;
    }
  }
}
