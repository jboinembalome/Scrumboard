/**
 * Scrumboard API
 * API for accessing Scrumboard project data.
 *
 * OpenAPI spec version: v1
 * Contact: jboinembaome@gmail.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import { AdherentDto } from './adherentDto';

export interface TeamDto { 
    id?: number;
    name?: string;
    adherents?: Array<AdherentDto>;
}