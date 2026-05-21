import { Injectable } from '@angular/core';
import { ApiController } from '../../shared/enums/api-controller.enum';
import { User } from '../../shared/models/user.model';
import { Api } from './api';

@Injectable({
  providedIn: 'root',
})
export class CurrentUserService {
  constructor(private api: Api) {}

  public async get() {
    const response = await this.api.get<User>(ApiController.User, '');

    return response.data;
  }
}
