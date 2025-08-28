import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ContactRequestsListComponent } from './list.component';
import { ContactRequestsService } from '../../../core/services/contact-requests/contact-requests.service';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { ContactRequest } from '../../../core/models/contact-request.interface';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatListModule } from '@angular/material/list';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ContactRequestsListComponent', () => {
  let component: ContactRequestsListComponent;
  let fixture: ComponentFixture<ContactRequestsListComponent>;
  let contactRequestsServiceSpy: jasmine.SpyObj<ContactRequestsService>;
  let dialogSpy: jasmine.SpyObj<MatDialog>;
  let routerSpy: jasmine.SpyObj<Router>;

  const mockContactRequests: ContactRequest[] = [
    {
      id: '1',
      name: 'John Doe',
      email: 'john@example.com',
      phone: '1234567890',
      departments: ['Legal'],
      description: 'Inquiry about case.',
      status: 'pending',
      createdAt: new Date().toISOString()
    },
    {
      id: '2',
      name: 'Jane Smith',
      email: 'jane@example.com',
      phone: '0987654321',
      departments: ['HR', 'Finance'],
      description: 'Question about payroll.',
      status: 'resolved',
      createdAt: new Date().toISOString()
    },
  ];

  beforeEach(async () => {
    contactRequestsServiceSpy = jasmine.createSpyObj('ContactRequestsService', [
      'getAllContactRequests',
      'getContactRequestById',
      'deleteContactRequest',
    ]);
    dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [
        ContactRequestsListComponent,
        CommonModule,
        MatCardModule,
        MatButtonModule,
        MatIconModule,
        MatGridListModule,
        MatListModule,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: ContactRequestsService, useValue: contactRequestsServiceSpy },
        { provide: MatDialog, useValue: dialogSpy },
        { provide: Router, useValue: routerSpy },
      ],
    }).compileComponents();

    contactRequestsServiceSpy.getAllContactRequests.and.returnValue(of(mockContactRequests));
    fixture = TestBed.createComponent(ContactRequestsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch and display contact requests', () => {
    expect(contactRequestsServiceSpy.getAllContactRequests).toHaveBeenCalled();
    fixture.detectChanges();
    const cards = fixture.nativeElement.querySelectorAll('mat-card');
    expect(cards.length).toBe(mockContactRequests.length);

    expect(cards[0].textContent).toContain(mockContactRequests[0].name);
    expect(cards[0].textContent).toContain(mockContactRequests[0].email);
    expect(cards[0].textContent).toContain(mockContactRequests[0].phone);
    expect(cards[0].textContent).toContain(mockContactRequests[0].departments?.join(', '));
    expect(cards[0].textContent).toContain(mockContactRequests[0].status);
    expect(cards[0].textContent).toContain(mockContactRequests[0].description);
  });

  it('should open dialog for adding a request', () => {
    dialogSpy.open.and.returnValue({ afterClosed: () => of(true) } as any);
    component.addRequest();
    expect(dialogSpy.open).toHaveBeenCalled();
    expect(contactRequestsServiceSpy.getAllContactRequests).toHaveBeenCalledTimes(2); // Initial call + after dialog close
  });

  it('should open dialog for editing a request', () => {
    contactRequestsServiceSpy.getContactRequestById.and.returnValue(of(mockContactRequests[0]));
    dialogSpy.open.and.returnValue({ afterClosed: () => of(true) } as any);

    component.editRequest('1');

    expect(contactRequestsServiceSpy.getContactRequestById).toHaveBeenCalledWith('1');
    expect(dialogSpy.open).toHaveBeenCalled();
    expect(contactRequestsServiceSpy.getAllContactRequests).toHaveBeenCalledTimes(2); // Initial call + after dialog close
  });

  it('should open confirmation dialog and delete request on confirm', () => {
    dialogSpy.open.and.returnValue({ afterClosed: () => of(true) } as any);
    contactRequestsServiceSpy.deleteContactRequest.and.returnValue(of(undefined));

    component.deleteRequest('1');

    expect(dialogSpy.open).toHaveBeenCalled();
    expect(contactRequestsServiceSpy.deleteContactRequest).toHaveBeenCalledWith('1');
    expect(contactRequestsServiceSpy.getAllContactRequests).toHaveBeenCalledTimes(2); // Initial call + after deletion
  });

  it('should open confirmation dialog but not delete request on cancel', () => {
    dialogSpy.open.and.returnValue({ afterClosed: () => of(false) } as any);

    component.deleteRequest('1');

    expect(dialogSpy.open).toHaveBeenCalled();
    expect(contactRequestsServiceSpy.deleteContactRequest).not.toHaveBeenCalled();
    expect(contactRequestsServiceSpy.getAllContactRequests).toHaveBeenCalledTimes(1); // Only initial call
  });
});