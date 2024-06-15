import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { WeatherForecast } from './fetch-data.model';
import { Observable } from 'rxjs';
import { NgClass, AsyncPipe, DatePipe } from '@angular/common';
import { TitleComponent } from '../../shared/components/title/title.component';

@Component({
    selector: 'app-fetch-data',
    templateUrl: './fetch-data.component.html',
    standalone: true,
    imports: [
      TitleComponent, 
      NgClass, 
      AsyncPipe, 
      DatePipe]
})
export class FetchDataComponent implements OnInit {

  columns: string[] = ['Date', 'Temp. (C)', 'Temp. (F)', 'Summary'];
  forecasts: Observable<WeatherForecast[]>;

  constructor(
    private _httpClient: HttpClient, 
    @Inject('BASE_URL') private _baseUrl: string) {  
  }

  ngOnInit(): void {
    this.forecasts = this._httpClient.get<WeatherForecast[]>(this._baseUrl + '/api/weatherforecast');
  }
}