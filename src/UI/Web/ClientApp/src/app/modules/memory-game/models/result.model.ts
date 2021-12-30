import { Player } from "./player.model";

export interface Result { 
    id: any;
    score: number;
    date: Date;
    player: Player;
}