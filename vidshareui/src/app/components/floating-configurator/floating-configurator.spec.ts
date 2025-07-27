import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FloatingConfigurator } from './floating-configurator';

describe('FloatingConfigurator', () => {
  let component: FloatingConfigurator;
  let fixture: ComponentFixture<FloatingConfigurator>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FloatingConfigurator]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FloatingConfigurator);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
