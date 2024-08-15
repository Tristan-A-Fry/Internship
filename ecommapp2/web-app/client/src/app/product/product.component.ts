import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';



interface Product{
  id? : number;
  name : string;
  productCategoryId: number;
  supplierId: number;
  description: string;
}

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit {
  apiUrl = 'http://localhost:5078/api/v1/product'; 
  products: Product[] = [];
  skipValue: number = 0;
  takeValue: number = 100;

  newProduct: Product = 
  {
    name: '',
    productCategoryId: 0,
    supplierId: 0,
    description: '',
  };
  constructor(private http: HttpClient, private router:Router){};
  ngOnInit(): void{
    this.fetchProducts();
  }

  fetchProducts(): void
  {
    const url = `${this.apiUrl}?skip=${this.skipValue}&take=${this.takeValue}`;
    this.http.get<Product[]>(url)
      .subscribe(products=>{
        this.products = products;
      },
    error => {
      console.error('error getting products', error);
    });
  }

  createProduct(): void {
    this.http.put<Product>(this.apiUrl, this.newProduct)
      .subscribe(newProduct => {
        this.fetchProducts(); 
        this.resetNewProduct(); 
      });
  }

  resetNewProduct(): void 
  {
    this.newProduct = 
  {
    name: '',
    productCategoryId: 0,
    supplierId: 0,
    description: '',

  };
  }
   navigateToHome()
  {
    this.router.navigate(['/home']);
  }
}
