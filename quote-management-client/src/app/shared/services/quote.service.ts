import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { QuoteRequest, QuoteResponse } from '../models/models';

@Injectable({
  providedIn: 'root',
})
export class QuoteService {
  constructor(private http: HttpClient) {}

  createQuote(request: QuoteRequest): Observable<QuoteResponse> {
    return this.http.post<QuoteResponse>(
      `${environment.apiUrl}/quotes`,
      request
    );
  }

  getAllQuotes(): Observable<QuoteResponse[]> {
    return this.http.get<QuoteResponse[]>(`${environment.apiUrl}/quotes`);
  }

  getCities(country: string): Observable<string[]> {
    return this.http.get<string[]>(
      `${environment.apiUrl}/city?country=${encodeURIComponent(country)}`
    );
  }
}
