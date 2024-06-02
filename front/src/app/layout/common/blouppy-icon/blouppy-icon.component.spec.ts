import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlouppyIconComponent } from './blouppy-icon.component';

describe('BlouppyIconComponent', () => {
  let component: BlouppyIconComponent;
  let fixture: ComponentFixture<BlouppyIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BlouppyIconComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BlouppyIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
