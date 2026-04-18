import { Injectable } from '@angular/core';
import { ApiController } from '../../shared/enums/api-controller.enum';
import { User } from '../../shared/models/user.model';
import { Api } from './api';

@Injectable({
  providedIn: 'root',
})
export class CurrentUser {
  constructor(private api: Api) {}

  public async get() {
    return this.api.get<User>(ApiController.User, '');
  }
}
