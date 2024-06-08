import { Component, Input } from '@angular/core';
import { NgClass } from '@angular/common';

@Component({
    selector: 'blouppy-toggle',
    templateUrl: './toggle.component.html',
    standalone: true,
    imports: [NgClass],
})
export class ToggleComponent {
    @Input() checked: boolean = false;

    constructor() {
    }

    updateChecked(): void {
        this.checked = !this.checked;
    }
}
