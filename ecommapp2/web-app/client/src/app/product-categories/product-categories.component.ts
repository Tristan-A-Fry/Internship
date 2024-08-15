import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';



interface ProductCategories{
  id? : number;
  name : string;
}


@Component({
  selector: 'app-productCategories',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './product-categories.component.html',
  styleUrl: './product-categories.component.css'
})
export class ProductCategoriesComponent implements OnInit {
  apiUrl = 'http://localhost:5078/api/v1/productcategory'; 
  productCategories:ProductCategories[] = [];
  skipValue: number = 0;
  takeValue: number = 100;

  newProductCategory: ProductCategories = 
  {
    name: '',
  };

  constructor(private http: HttpClient, private router:Router){};

  ngOnInit(): void{
    this.fetchProductCategory();
  }

  fetchProductCategory(): void
  {
    const url = `${this.apiUrl}?skip=${this.skipValue}&take=${this.takeValue}`;
    this.http.get<ProductCategories[]>(url)
      .subscribe(productCategories =>{
        this.productCategories = productCategories;
      },
    error => {
      console.error('error getting suppliers', error);
    });
  }

  createProductCategory(): void {
    this.http.put<ProductCategories>(this.apiUrl, this.newProductCategory)
      .subscribe(newProductCategory => {
        this.fetchProductCategory(); // Fetch customers after creating a new one
        this.resetNewProductCategory(); // Clear form after successful creation
      });
  }

  resetNewProductCategory(): void 
  {
    this.newProductCategory = 
    {
      name: '',
    };
  }
   navigateToHome()
  {
    this.router.navigate(['/home']);
  }
}