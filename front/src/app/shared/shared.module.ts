import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PluralPipe } from "./pipes/plural.pipe";
import { OrderByPipe } from './pipes/orderby.pipe';
import { StringColorPipe } from './pipes/string-color.pipe';
import { InitialPipe } from './pipes/initial.pipe';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        InitialPipe,
        PluralPipe,
        OrderByPipe,
        StringColorPipe,
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        InitialPipe,
        PluralPipe,
        OrderByPipe,
        StringColorPipe
    ]
})
export class SharedModule
{
}
