import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckSplitComponent } from './check-split.component';

describe('CheckSplitComponent', () => {
  let component: CheckSplitComponent;
  let fixture: ComponentFixture<CheckSplitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CheckSplitComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CheckSplitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
