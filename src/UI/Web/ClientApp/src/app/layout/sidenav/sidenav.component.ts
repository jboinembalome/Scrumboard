import { BreakpointObserver, BreakpointState } from '@angular/cdk/layout';
import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Router } from '@angular/router';
import { NavigationPaths } from './sidenav.constant';
import { Observable } from 'rxjs';
import { Navigation } from './models/navigation.model';


const SMALL_WIDTH_BREAKPOINT = 720;

@Component({
  selector: 'sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit {

  public isScreenSmall: boolean;

  navigationPaths : Navigation[] = NavigationPaths;
  isDarkTheme: boolean = false;
  dir: string = 'ltr';

  constructor(
    @Inject(DOCUMENT) private _document: any,
    private breakpointObserver: BreakpointObserver,
    private router: Router) { }

  @ViewChild(MatSidenav) sidenav: MatSidenav;

  toggleTheme() {
    this.isDarkTheme = !this.isDarkTheme;

    if (this.isDarkTheme)
      this.updateScheme('dark');
    else
      this.updateScheme('light');
  }

  toggleDir() {
    this.dir = this.dir == 'ltr' ? 'rtl' : 'ltr';
  }

  ngOnInit(): void {
    this.breakpointObserver
      .observe([`(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`])
      .subscribe((state: BreakpointState) => {
        this.isScreenSmall = state.matches;
      });

    this.router.events.subscribe(() => {
      if (this.isScreenSmall) {
        this.sidenav.close();
      }
    });
  }

  /**
 * Update the selected scheme
 *
 * @private
 */
  private updateScheme(scheme): void {
    // Remove class names for all schemes
    this._document.body.classList.remove('light', 'dark');

    // Add class name for the currently selected scheme
    this._document.body.classList.add(scheme);
  }

}
