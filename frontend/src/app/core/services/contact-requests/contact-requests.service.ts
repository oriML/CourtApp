import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContactRequest } from '../../models/contact-request.interface';
import { MonthlyReport } from '../../models/monthly-report.interface'; // Import MonthlyReport
import { environment } from '../../../../environments/environment'; // Import environment

@Injectable({
  providedIn: 'root'
})
export class ContactRequestsService {
  private baseUrl = `${environment.apiUrl}/ContactRequests`; // Use baseUrl for contact requests

  constructor(private http: HttpClient) { }

  getAllContactRequests(): Observable<ContactRequest[]> {
    return this.http.get<ContactRequest[]>(this.baseUrl);
  }

  getContactRequestById(id: string): Observable<ContactRequest> {
    return this.http.get<ContactRequest>(`${this.baseUrl}/${id}`);
  }

  createContactRequest(contactRequest: ContactRequest): Observable<ContactRequest> {
    return this.http.post<ContactRequest>(this.baseUrl, contactRequest);
  }

  updateContactRequest(id: string, contactRequest: ContactRequest): Observable<ContactRequest> {
    return this.http.put<ContactRequest>(`${this.baseUrl}/${id}`, contactRequest);
  }

  deleteContactRequest(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getMonthlyReport(): Observable<MonthlyReport[]> {
    return this.http.get<MonthlyReport[]>(`${environment.apiUrl}/ContactRequests/report/monthly`); // Use environment.apiUrl for reports
  }
}