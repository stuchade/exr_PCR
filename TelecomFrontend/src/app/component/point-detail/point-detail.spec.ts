import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PointDetail } from './point-detail';

describe('PointDetail', () => {
  let component: PointDetail;
  let fixture: ComponentFixture<PointDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PointDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(PointDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
