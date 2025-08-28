import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ContactRequest } from '../../models/contact-request.interface';
import { MonthlyReport } from '../../models/monthly-report.interface';
import { ContactRequestsService } from './contact-requests.service';

describe('ContactRequestsService', () => {
  let service: ContactRequestsService;
  let httpTestingController: HttpTestingController;
  const apiUrl = '/api/contact-requests';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ContactRequestsService]
    });
    service = TestBed.inject(ContactRequestsService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify(); // Ensure that no outstanding requests are pending.
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve all contact requests', () => {
    const dummyRequests: ContactRequest[] = [
      { id: '1', name: 'John Doe', email: 'john@example.com', description: 'Test', status: 'pending', createdAt: '' },
      { id: '2', name: 'Jane Doe', email: 'jane@example.com', description: 'Test', status: 'resolved', createdAt: '' }
    ];

    service.getAllContactRequests().subscribe(requests => {
      expect(requests).toEqual(dummyRequests);
    });

    const req = httpTestingController.expectOne(apiUrl);
    expect(req.request.method).toEqual('GET');
    req.flush(dummyRequests);
  });

  it('should retrieve a contact request by ID', () => {
    const dummyRequest: ContactRequest = { id: '1', name: 'John Doe', email: 'john@example.com', description: 'Test', status: 'pending', createdAt: '' };

    service.getContactRequestById('1').subscribe(request => {
      expect(request).toEqual(dummyRequest);
    });

    const req = httpTestingController.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toEqual('GET');
    req.flush(dummyRequest);
  });

  it('should create a new contact request', () => {
    const newRequest: ContactRequest = { id: '3', name: 'New User', email: 'new@example.com', description: 'New', status: 'pending', createdAt: '' };

    service.createContactRequest(newRequest).subscribe(request => {
      expect(request).toEqual(newRequest);
    });

    const req = httpTestingController.expectOne(apiUrl);
    expect(req.request.method).toEqual('POST');
    expect(req.request.body).toEqual(newRequest);
    req.flush(newRequest);
  });

  it('should update an existing contact request', () => {
    const updatedRequest: ContactRequest = { id: '1', name: 'John Updated', email: 'john@example.com', description: 'Updated', status: 'resolved', createdAt: '' };

    service.updateContactRequest('1', updatedRequest).subscribe(request => {
      expect(request).toEqual(updatedRequest);
    });

    const req = httpTestingController.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toEqual('PUT');
    expect(req.request.body).toEqual(updatedRequest);
    req.flush(updatedRequest);
  });

  it('should delete a contact request', () => {
    service.deleteContactRequest('1').subscribe(response => {
      expect(response).toBeUndefined(); // For void response
    });

    const req = httpTestingController.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toEqual('DELETE');
    req.flush(null); // Simulate successful deletion with no content
  });

  it('should retrieve monthly reports', () => {
    const dummyReports: MonthlyReport[] = [
      { id: '1', month: 1, year: 2023, totalRequests: 10, resolvedRequests: 5, unresolvedRequests: 5, departmentBreakdown: { 'Legal': 5, 'HR': 5 } },
      { id: '2', month: 2, year: 2023, totalRequests: 15, resolvedRequests: 10, unresolvedRequests: 5, departmentBreakdown: { 'IT': 10, 'Finance': 5 } }
    ];

    service.getMonthlyReport().subscribe(reports => {
      expect(reports).toEqual(dummyReports);
    });

    const req = httpTestingController.expectOne(`${apiUrl}/report/monthly`);
    expect(req.request.method).toEqual('GET');
    req.flush(dummyReports);
  });
});