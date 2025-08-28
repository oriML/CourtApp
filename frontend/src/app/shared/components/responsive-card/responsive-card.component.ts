import { Component, input, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-responsive-card',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './responsive-card.component.html',
  styleUrl: './responsive-card.component.scss'
})
export class ResponsiveCardComponent {
  title = input<string>('');
  subtitle = input<string>('');
}