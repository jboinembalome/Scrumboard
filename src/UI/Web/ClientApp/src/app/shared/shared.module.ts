import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PluralPipe } from "./pipes/plural.pipe";
import { OrderByPipe } from './pipes/orderby.pipe';
import { StringColorPipe } from './pipes/string-color.pipe';

@NgModule({
    declarations: [
        PluralPipe,
        OrderByPipe,
        StringColorPipe
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        PluralPipe,
        OrderByPipe,
        StringColorPipe
    ]
})
export class SharedModule
{
}
