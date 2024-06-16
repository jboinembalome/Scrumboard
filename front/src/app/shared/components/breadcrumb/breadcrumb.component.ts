import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { Navigation } from 'app/core/navigation/models/navigation.model';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  standalone: true,
  imports: [
    RouterLink,
    MatIconModule,
  ]
})
export class BreadcrumbComponent {
  @Input() items: Navigation[] = [];
}