import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { CustomerComponent } from './customer/customer.component';
import { HomeComponent } from './home/home.component';
import { SupplierComponent } from './supplier/supplier.component';
import { adminGuard } from './admin.guard';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { ProductComponent } from './product/product.component';
import { ProductCategoriesComponent } from './product-categories/product-categories.component';
import { OrderComponent } from './order/order.component';
import { UserComponent } from './user/user.component';
export const routes: Routes = [
    {path: 'login' , component: LoginComponent},
    {path: 'customers', component:CustomerComponent},
    {path: 'suppliers', component:SupplierComponent, canActivate: [adminGuard]},
    {path: 'products', component: ProductComponent, canActivate: [adminGuard]},
    {path: 'productcategories', component: ProductCategoriesComponent, canActivate: [adminGuard]},
    {path: 'orders', component: OrderComponent},
    {path: 'unauthorized', component: UnauthorizedComponent},
    {path: 'home', component: HomeComponent},
    {path: 'users', component: UserComponent, canActivate: [adminGuard]},
    {path: '', component: LoginComponent},
];
