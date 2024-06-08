import { Component, Input } from '@angular/core';

@Component({
    selector: 'blouppy-title',
    templateUrl: './title.component.html',
    standalone: true,
})
export class TitleComponent {
    @Input() text: string = '';

    constructor() {
    }
}
