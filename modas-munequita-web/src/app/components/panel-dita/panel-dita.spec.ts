import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PanelDita } from './panel-dita';

describe('PanelDita', () => {
  let component: PanelDita;
  let fixture: ComponentFixture<PanelDita>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PanelDita],
    }).compileComponents();

    fixture = TestBed.createComponent(PanelDita);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
