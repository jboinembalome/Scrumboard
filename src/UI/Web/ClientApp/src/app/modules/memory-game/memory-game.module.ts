import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ComponentModule  } from 'src/app/shared/components/component.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { CountdownModule } from 'ngx-countdown';
import { MemoryGameComponent } from './memory-game.component';
import { CardComponent } from './card/card.component';
import { ScoreComponent } from './score/score.component';
import { ResultsComponent } from './results/results.component';
import { HighScoreInputComponent } from './high-score-input/high-score-input.component';

import { counterRoutes } from './memory-game.routing';
import { MaterialModule } from 'src/app/shared/material/material.module';
import { ChatComponent } from './chat/chat.component';

@NgModule({
    declarations: [
        MemoryGameComponent,
        CardComponent,
        ScoreComponent,
        ResultsComponent,
        HighScoreInputComponent,
        ChatComponent
    ],
    imports     : [
        RouterModule.forChild(counterRoutes),
        ComponentModule,
        CountdownModule,
        MaterialModule,
        SharedModule
    ]
})
export class MemoryGameModule
{
}
