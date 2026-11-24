import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MiDita } from './mi-dita';

describe('MiDita', () => {
  let component: MiDita;
  let fixture: ComponentFixture<MiDita>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MiDita],
    }).compileComponents();

    fixture = TestBed.createComponent(MiDita);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
