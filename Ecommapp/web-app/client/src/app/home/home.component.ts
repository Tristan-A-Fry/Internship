import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-home',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{

  username: string | null = null;
  roles : string[] = [];
  token : string | null = null;
  authenticated : boolean = false;
  authorized : boolean = false;

  constructor(private authService: AuthService, private router: Router){}

  ngOnInit(): void {
      this. authenticated = this.authService.isAuthenticated();
      this.authorized = this.authService.isAuthorized();
      this.token = this.authService.getToken();
      this.username = this.authService.getUsername();
      this.roles = this.authService.getRole();
      
  }

  navigateToOrders()
  {
    this.router.navigate(['orders']);
  }
  navigateToProducts ()
  {
    this.router.navigate(['products']);
  }

  navigateToSuppliers ()
  {

    this.router.navigate(['suppliers']);
  }

  navigateToProductCategories()
  {

    this.router.navigate(['productcategories']);
  }

  navigateToCustomers()
  {

    this.router.navigate(['customers']);
  }

  navigateToUsers()
  {
    this.router.navigate(['users']);
  }

  logout(){
    this.authService.logout();
  }
}
