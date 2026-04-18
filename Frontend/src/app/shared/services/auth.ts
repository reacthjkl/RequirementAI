import { Injectable } from '@angular/core';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { AuthRequest } from '../models/auth-request.model';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  constructor(private api: Api) {}

  public async authenticate(request: AuthRequest) {
    return await this.api.post(ApiController.Auth, 'login', request);
  }

  public async logout() {
    return await this.api.post(ApiController.Auth, 'logout');
  }

  public async refresh() {
    return await this.api.get(ApiController.Auth, 'refresh');
  }
}
