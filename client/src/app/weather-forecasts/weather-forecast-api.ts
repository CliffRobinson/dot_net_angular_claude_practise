import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { WeatherForecast } from './weather-forecast';

@Injectable({
  providedIn: 'root',
})
export class WeatherForecastApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/weatherforecast';

  get(): Observable<WeatherForecast[]> {
    return this.http.get<WeatherForecast[]>(this.baseUrl);
  }

  getByDays(days: number): Observable<WeatherForecast[]> {
    return this.http.get<WeatherForecast[]>(`${this.baseUrl}/${days}`);
  }
}
