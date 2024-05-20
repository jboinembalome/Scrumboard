import { Feedback } from "./feedback.enum";

export interface Card {
    index: number; 
    symbol: string;
    feedback: Feedback;
}