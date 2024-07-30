import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

interface Order
{
  id?: number,
  orderDate: Date,
  customerId: number,
  customer: Customer,
}
interface OrderItems
{
  id? : number,
  orderId: number,
  productId: number,
  quantity: number,

  product: Product,
  order: Order
}

export interface Product
{
  id? : number;
  name : string;
  productCategoryId: number;
  supplierId: number;
  description: string;
}

export interface Customer
{
  id?: number;
  name: string;
  address: string;
  phoneNumber: string;
}

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})

export class OrderComponent implements OnInit {
  apiUrl = 'http://localhost:5078/api/v1/order';
  apiUrlOrderItem = 'http://localhost:5078/api/v1/order/';
  orders: Order[] = [];
  orderItems: OrderItems[]=[];
  skipValue: number = 0;
  takeValue: number = 0;
  orderId : number = 0;

  newOrder: Order =
  {
    orderDate: new Date(),
    customerId: 0,
    customer: 
    {
      name: '',
      address: '',
      phoneNumber: '',
    }
  }

  constructor(private http: HttpClient, private router: Router){};
  ngOnInit(): void {
    this.fetchOrders();
    this.fetchOrderItems();
  }

  fetchOrders():void
  {
    const url = `${this.apiUrl}?skip=${this.skipValue}&take=${this.takeValue}`;
    this.http.get<Order[]>(url)
      .subscribe(orders => {
        this.orders= orders;
      },
      error => {
        console.error('error getting orders', error);
      });
  }

  fetchOrderItems():void
  {
    const url = `${this.apiUrlOrderItem}${this.orderId}/items?skip=${this.skipValue}&take=${this.takeValue}` 
    this.http.get<OrderItems[]>(url)
      .subscribe(orderItems => {
        this.orderItems= orderItems;
      },
      error => {
        console.error('error getting order items', error);
      });
  }
  

  // Create order is made for testing, as I do not have a pre-exisiting db
  createOrder() : void
  {
    this.http.put<Order>(this.apiUrl, this.newOrder)
      .subscribe(newOrder =>{
        this.fetchOrders();
        this.resetNewOrder();
      });
  }
  

  resetNewOrder() : void
  {
    this.newOrder =
    {
      orderDate: new Date(),
      customerId: 0,
      customer: 
      {
        name: '',
        address: '',
        phoneNumber: '',
      }
    }
  }

   navigateToHome()
  {
    this.router.navigate(['/home']);
  }



}
