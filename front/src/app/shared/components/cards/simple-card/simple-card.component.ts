import { Component, Input } from '@angular/core';
import { MatTooltip } from '@angular/material/tooltip';
import { NgClass } from '@angular/common';

@Component({
    selector: 'blouppy-simple-card',
    templateUrl: './simple-card.component.html',
    standalone: true,
    imports: [NgClass, MatTooltip],
})
export class SimpleCardComponent {
    @Input() title: string = '';
    @Input() subtitle: string = '';
    @Input() bgColor: string = '';
    @Input() textAtLeft: string = '';
    
    constructor() {
    }
}
