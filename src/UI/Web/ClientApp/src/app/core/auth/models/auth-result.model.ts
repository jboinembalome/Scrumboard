import { AuthResultStatus } from "./auth-result-status.enum";

export type IAuthResult =
  AuthResultSuccess |
  AuthResultFailure |
  AuthResultRedirect;

export interface AuthResultSuccess {
  status: AuthResultStatus.Success;
  state: any;
}

export interface AuthResultFailure {
  status: AuthResultStatus.Fail;
  message: string;
}

export interface AuthResultRedirect {
  status: AuthResultStatus.Redirect;
}