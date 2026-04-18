import { Injectable, signal } from '@angular/core';
import { Api } from '../../core/services/api';
import { CurrentUser } from '../../core/services/current-user';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { AuthRequest } from '../models/auth-request.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  public loggedIn = signal(false);

  constructor(
    private api: Api,
    private cu: CurrentUser,
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
    return await this.api.get(ApiController.Auth, 'refresh');
  }

  public async checkIsLoggedIn() {
    const response: ApiResponse<User> = await this.cu.get();

    const exists: boolean = response.data !== null;

    if (exists) {
      this.loggedIn.set(true);
    }

    return exists;
  }
}
