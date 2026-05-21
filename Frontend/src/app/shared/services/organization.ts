import { Injectable } from '@angular/core';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { OrganizationForUpdate } from '../models/organization-for-update.model';
import { Organization } from './../models/organization.model';

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  constructor(private api: Api) {}

  public async get(): Promise<Organization | null> {
    const response: ApiResponse<Organization> = await this.api.get(ApiController.Organization, '');

    return response.data;
  }

  public async update(organization: OrganizationForUpdate): Promise<void> {
    await this.api.put(ApiController.Organization, '', organization);
  }
}
