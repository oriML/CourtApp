import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;
  let store: { [key: string]: string } = {};

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthService);

    // Mock localStorage
    spyOn(localStorage, 'setItem').and.callFake((key: string, value: string) => {
      store[key] = value;
    });
    spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      return store[key] || null;
    });
    spyOn(localStorage, 'removeItem').and.callFake((key: string) => {
      delete store[key];
    });
    spyOn(localStorage, 'clear').and.callFake(() => {
      store = {};
    });
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should store token on login', (done) => {
    const credentials = { username: 'test@example.com', password: 'password123' };
    service.login(credentials).subscribe(({token}) => {
      expect(token).toBeDefined();
      expect(localStorage.setItem).toHaveBeenCalledWith('jwt_token', token);
      expect(service.getToken()).toEqual(token);
      done();
    });
  });

  it('should return stored token', () => {
    const testToken = 'test_jwt_token';
    service.storeToken(testToken);
    expect(service.getToken()).toEqual(testToken);
  });

  it('should return true if logged in', () => {
    service.storeToken('some_token');
    expect(service.isLoggedIn()).toBeTrue();
  });

  it('should return false if not logged in', () => {
    expect(service.isLoggedIn()).toBeFalse();
  });

  it('should remove token on logout', () => {
    service.storeToken('some_token');
    service.logout();
    expect(service.getToken()).toBeNull();
  });
});