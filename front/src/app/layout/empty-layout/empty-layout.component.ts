import { Component, ViewEncapsulation } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'empty-layout',
    templateUrl: './empty-layout.component.html',
    styleUrls: ['./empty-layout.component.scss'],
    encapsulation: ViewEncapsulation.None,
    standalone: true,
    imports: [RouterOutlet],
})
export class EmptyLayoutComponent
{
    
}
