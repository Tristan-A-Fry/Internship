import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

interface User
{
  id? : Number,
  username: string,
  password: string,
  role: string
}


@Component({
  selector: 'app-user',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit{

  apiUrl = 'http://localhost:5078/api/v1/User';
  users : User[] = [];
  errorMessage: string = '';
  newUser: User =
  {
    username: '',
    password: '',
    role: ''
  }

  constructor(private http: HttpClient, private router: Router){};

  ngOnInit(): void {
    this.fetchUsers();
  }

  fetchUsers(): void
  {
    const url = this.apiUrl;
    this.http.get<User[]>(url)
      .subscribe(users =>{
        this.users = users;
      },
    error => {
      console.error('error fetching users', error);
    });
  }

  createUser(): void
  {
    this.http.post<User>(this.apiUrl, this.newUser)
      .subscribe(newUser =>{
        this.fetchUsers();
        this.resetNewUser();
        this.errorMessage = '';
      },
      error =>{
        if(error.status == 409)
          {
            this.errorMessage = 'A user with this username already exists';
          }else{
            this.errorMessage = 'An unknown error has occurred.';
          }
      }
    
    );
  }

  editUser : User | null = null; //Property to hold the user being edited
  updateUser(): void
  {
    if(this.editUser)
      {
        this.http.put<User>(`${this.apiUrl}/${this.editUser.id}`, this.editUser)
          .subscribe(updatedUser =>{
            this.fetchUsers();
            this.editUser = null;
            this.errorMessage = '';
          },
        error =>{
          this.errorMessage = 'An unknown error occured,';
        });
      } 

  }

  startEditUser(user: User): void
  {
    this.editUser = {...user}; //Make a copy of the user to edit
  }

  cancelEdit(): void 
  {
    this.editUser = null; // Cancel editing
  }

  deleteUser(user: User): void
  {
    this.http.delete<void>(`${this.apiUrl}/${user.id}`)
      .subscribe(() =>{
        this.fetchUsers();
        this.errorMessage = '';
      },
    error =>{
      this.errorMessage = 'An unknown error occurred';
    });
  }



  resetNewUser(): void
  {
    this.newUser = 
    {
      username: '',
      password:'',
      role: ''
    }
  }


   navigateToHome()
  {
    this.router.navigate(['/home']);
  }

}
