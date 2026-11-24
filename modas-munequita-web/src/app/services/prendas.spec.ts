import { TestBed } from '@angular/core/testing';
import { Prendas } from './prendas';

describe('Prendas', () => {
  let service: Prendas;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Prendas);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
