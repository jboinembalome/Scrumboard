import { Component, Input } from '@angular/core';

@Component({
    selector     : 'blouppy-simple-card',
    templateUrl  : './simple-card.component.html',
})
export class SimpleCardComponent {
    @Input() title: string = '';
    @Input() subtitle: string = '';
    @Input() bgColor: string = '';
    @Input() textAtLeft: string = '';
    
    constructor() {
    }
}
