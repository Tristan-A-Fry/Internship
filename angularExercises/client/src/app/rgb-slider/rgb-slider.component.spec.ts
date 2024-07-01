import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RgbSliderComponent } from './rgb-slider.component';

describe('RgbSliderComponent', () => {
  let component: RgbSliderComponent;
  let fixture: ComponentFixture<RgbSliderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RgbSliderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RgbSliderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
