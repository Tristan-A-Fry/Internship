import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';



interface Supplier{
  id? : number;
  name : string;
}


@Component({
  selector: 'app-supplier',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './supplier.component.html',
  styleUrl: './supplier.component.css'
})
export class SupplierComponent implements OnInit {
  apiUrl = 'http://localhost:5078/api/v1/Supplier'; 
  suppliers: Supplier[] = [];
  skipValue: number = 0;
  takeValue: number = 100;

  newSupplier: Supplier = 
  {
    name: '',
  };
  constructor(private http: HttpClient, private router: Router){};
  ngOnInit(): void{
    this.fetchSuppliers();
  }

  fetchSuppliers(): void
  {
    const url = `${this.apiUrl}?skip=${this.skipValue}&take=${this.takeValue}`;
    this.http.get<Supplier[]>(url)
      .subscribe(suppliers =>{
        this.suppliers = suppliers;
      },
    error => {
      console.error('error getting suppliers', error);
    });
  }

  createSupplier(): void {
    this.http.put<Supplier>(this.apiUrl, this.newSupplier)
      .subscribe(newSupplier => {
        this.fetchSuppliers(); // Fetch customers after creating a new one
        this.resetNewSuppliers(); // Clear form after successful creation
      });
  }

  resetNewSuppliers(): void 
  {
    this.newSupplier = 
    {
      name: '',
    };
  }
   navigateToHome()
  {
    this.router.navigate(['/home']);
  }
}
