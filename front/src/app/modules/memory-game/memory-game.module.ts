import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ComponentModule  } from 'app/shared/components/component.module';
import { SharedModule } from 'app/shared/shared.module';
import { CountdownModule } from 'ngx-countdown';
import { MemoryGameComponent } from './memory-game.component';
import { CardComponent } from './card/card.component';
import { ScoreComponent } from './score/score.component';
import { ResultsComponent } from './results/results.component';
import { HighScoreInputComponent } from './high-score-input/high-score-input.component';

import { counterRoutes } from './memory-game.routing';
import { MaterialModule } from 'app/shared/material/material.module';

@NgModule({
    imports: [
        RouterModule.forChild(counterRoutes),
        ComponentModule,
        CountdownModule,
        MaterialModule,
        SharedModule,
        MemoryGameComponent,
        CardComponent,
        ScoreComponent,
        ResultsComponent,
        HighScoreInputComponent
    ]
})
export class MemoryGameModule
{
}
