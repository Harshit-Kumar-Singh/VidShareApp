import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DragVideo } from './drag-video';

describe('DragVideo', () => {
  let component: DragVideo;
  let fixture: ComponentFixture<DragVideo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DragVideo]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DragVideo);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
