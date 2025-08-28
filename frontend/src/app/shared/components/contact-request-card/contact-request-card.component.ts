import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ContactRequest } from '../../../core/models/contact-request.interface';

@Component({
  selector: 'app-contact-request-card',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './contact-request-card.component.html',
  styleUrl: './contact-request-card.component.scss'
})
export class ContactRequestCardComponent {
  @Input() request!: ContactRequest;
  @Output() edit = new EventEmitter<string>();
  @Output() delete = new EventEmitter<string>();

  onEdit(): void {
    this.edit.emit(this.request.id);
  }

  onDelete(): void {
    this.delete.emit(this.request.id);
  }
}