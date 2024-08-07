import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterOutlet,Router } from '@angular/router';
interface Customer {
  id?: number;
  name: string;
  address: string;
  phoneNumber: string;
}

@Component({
  standalone:true,
  imports:[FormsModule,CommonModule, RouterOutlet],
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {
  apiUrl = 'http://localhost:5078/api/v1/customer'; // Replace with your API URL
  customers: Customer[] = [];
  skipValue: number = 0;
  takeValue: number = 10;

  newCustomer: Customer = {
    name: '',
    address: '',
    phoneNumber: ''
  };

  constructor(private http: HttpClient, private router:Router) { }

  ngOnInit(): void {
    this.fetchCustomers();
  }

  fetchCustomers(): void {
    const url = `${this.apiUrl}?skip=${this.skipValue}&take=${this.takeValue}`;
    this.http.get<Customer[]>(url)
      .subscribe(customers => {
        this.customers = customers;
      });
  }

  createCustomer(): void {
    this.http.post<Customer>(this.apiUrl, this.newCustomer)
      .subscribe(newCustomer => {
        this.fetchCustomers(); // Fetch customers after creating a new one
        this.resetNewCustomer(); // Clear form after successful creation
      });
  }

  resetNewCustomer(): void {
    this.newCustomer = {
      name: '',
      address: '',
      phoneNumber: ''
    };
  }

  navigateToHome()
  {
    this.router.navigate(['/home']);
  }
}
