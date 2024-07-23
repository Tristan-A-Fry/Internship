import { CanActivateFn } from '@angular/router';
import { AuthService } from './auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  if(authService.isAuthorized())
  {
    return true;
  }
  else 
  {
    router.navigate(['/unauthorized']); //if not admin, re-route back to home
    return false;
  }
};
