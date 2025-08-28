import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResponsiveCardComponent } from './responsive-card.component';

describe('ResponsiveCardComponent', () => {
  let component: ResponsiveCardComponent;
  let fixture: ComponentFixture<ResponsiveCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResponsiveCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResponsiveCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
