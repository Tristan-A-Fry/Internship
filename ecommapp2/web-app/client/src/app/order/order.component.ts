import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Order, OrderResponseDto,Customer,Product,OrderItems, GetOrderDto } from '../models/all.model';



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
  // orders: Order[] = [];
  orders: GetOrderDto[] = [];
  orderItems: OrderItems[]=[];
  skipValue: number = 0;
  takeValue: number = 0;
  orderId : number = 0;
  customerId: number = 1;

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

  fetchOrders(): void {
    const url = `${this.apiUrl}?skip=${this.skipValue}&take=${this.takeValue}&customerId=${this.customerId}`;
    this.http.get<OrderResponseDto[]>(url).subscribe(
      (orderResponses) => {
        console.log('Order Responses:', orderResponses);
        this.orders = orderResponses.map(response => response.order);
        // Optionally handle the receipt file
        if (orderResponses.length > 0) {
          const receiptFile = this.base64ToBlob(orderResponses[0].receiptFile, 'text/plain');
          this.downloadReceipt(receiptFile, orderResponses[0].receiptFileName);
        }
      },
      (error) => {
        console.error('Error getting orders', error);
      }
    );
  }

  base64ToBlob(base64: string, type: string): Blob {
    const binary = atob(base64);
    const array = [];
    for (let i = 0; i < binary.length; i++) {
      array.push(binary.charCodeAt(i));
    }
    return new Blob([new Uint8Array(array)], { type });
  }

  downloadReceipt(receiptFile: Blob, receiptFileName: string): void {
    const url = window.URL.createObjectURL(receiptFile);
    const a = document.createElement('a');
    a.href = url;
    a.download = receiptFileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.URL.revokeObjectURL(url);
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
