import { Component } from '@angular/core';
import { TitleComponent } from '../../shared/components/title/title.component';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    standalone: true,
    imports: [TitleComponent],
})
export class HomeComponent {
}
