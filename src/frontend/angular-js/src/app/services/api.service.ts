import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { SpiderFormData } from '../models/SpiderFormData';

@Injectable({
  providedIn: 'root'
})

export class ApiService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  processSpiderCommand(formData: SpiderFormData): Observable<any> {
    const request = {
      WallInput: `${formData.WallWidth} ${formData.WallHeight}`,
      SpiderInput: `${formData.SpiderX} ${formData.SpiderY} ${formData.Orientation}`,
      CommandInput: formData.Commands
    };
    
    return this.http.post(`${this.baseUrl}/api/v1/spider/process`, request);
  }
}
