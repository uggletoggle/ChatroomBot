import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignoutOidcComponent } from './signout-oidc.component';

describe('SignoutOidcComponent', () => {
  let component: SignoutOidcComponent;
  let fixture: ComponentFixture<SignoutOidcComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SignoutOidcComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SignoutOidcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
