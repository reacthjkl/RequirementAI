import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiController } from '../../shared/enums/api-controller.enum';
import { ApiResponse } from '../../shared/models/api-response.model';

@Injectable({
  providedIn: 'root',
})
export class Api {
  private url = (controller: ApiController, resource: string) =>
    environment.apiUrl + '/' + controller + '/' + resource;

  constructor(private http: HttpClient) {}

  public async get<T>(controller: ApiController, resource: string) {
    return await firstValueFrom(this.http.get<ApiResponse<T>>(this.url(controller, resource)));
  }

  public async put<ReqT, ResT>(controller: ApiController, resource: string, body?: ReqT) {
    return await firstValueFrom(
      this.http.put<ApiResponse<ResT>>(this.url(controller, resource), body),
    );
  }

  public async post<T>(controller: ApiController, resource: string, body?: T) {
    return await firstValueFrom(
      this.http.post<ApiResponse<null>>(this.url(controller, resource), body),
    );
  }

  public async delete(controller: ApiController, resource: string) {
    return await firstValueFrom(
      this.http.delete<ApiResponse<null>>(this.url(controller, resource)),
    );
  }
}
