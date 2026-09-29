import { TestBed } from '@angular/core/testing';
import { HttpErrorResponse, provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';

import { WeatherForecastApi } from './weather-forecast-api';
import { WeatherForecast } from './weather-forecast';

describe('WeatherForecastApi', () => {
  let api: WeatherForecastApi;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()], // real HttpClient, fake network
    });
    api = TestBed.inject(WeatherForecastApi);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify()); // fails if a request was made that no test handled

  it('should be created', () => {
    expect(api).toBeTruthy();
  });

  it('get GETs /api/weatherforecast and emits the forecasts', () => {
    const forecasts: WeatherForecast[] = [];
    let received: WeatherForecast[] | undefined;

    api.get().subscribe((wfc) => (received = wfc));

    const req = http.expectOne('/api/weatherforecast');

    expect(req.request.method).toBe('GET');
    req.flush(forecasts);

    expect(received).toEqual(forecasts);
  });

  it('getByDays GETs /api/weatherforecast/{days} and emits the forecasts', () => {
    const forecasts: WeatherForecast[] = [];
    let received: WeatherForecast[] | undefined;

    api.getByDays(5).subscribe((wfc) => (received = wfc));

    const req = http.expectOne('/api/weatherforecast/5');

    expect(req.request.method).toBe('GET');
    req.flush(forecasts);

    expect(received).toEqual(forecasts);
  });

  it('getByDays passes a 400 to the error callback', () => {
    let error: HttpErrorResponse | undefined;

    api.getByDays(999).subscribe({ error: (e) => (error = e) });

    http
      .expectOne('/api/weatherforecast/999')
      .flush('Bad Request', { status: 400, statusText: 'Bad Request' });

    expect(error?.status).toBe(400);
  });
});
