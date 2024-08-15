export interface Order
{
  id?: number;
  orderDate: Date;
  customerId: number;
  customer: Customer;
}

export interface OrderResponseDto
{
    order : GetOrderDto ;
    receiptFile: string;
    receiptFileName: string;
}

export interface GetOrderDto
{
    id?: number;
    orderDate : Date;
    customerName : string;
    customer : Customer;
}
export interface OrderItems
{
  id? : number;
  orderId: number;
  productId: number;
  quantity: number;

  product: Product;
  order: Order;
}

export interface Product
{
  id? : number;
  name : string;
  description : string;
  price : number;
  productCategoryId: number;
  supplierId: number;

  supplier : Supplier;
  productCategory : ProductCategories;
}

export interface ProductCategories{
  id? : number;
  name : string;
}

export interface Customer
{
  id?: number;
  name: string;
  address: string;
  phoneNumber: string;
}

export interface Supplier{
  id? : number;
  name : string;
}