import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-blouppy-icon',
  standalone: true,
  imports: [],
  templateUrl: './blouppy-icon.component.html',
  styleUrl: './blouppy-icon.component.scss'
})
export class BlouppyIconComponent {
  @Input() svgClass?: string;
}
