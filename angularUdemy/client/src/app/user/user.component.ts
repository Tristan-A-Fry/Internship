import { Component, computed,signal} from '@angular/core';
import { DUMMY_USERS } from '../dummy-users';

//creating random index to pick a random user to display
const randomIndex = Math.floor(Math.random() * DUMMY_USERS.length);


@Component({
  selector: 'app-user',
  standalone: true,
  imports: [],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent {
  selectedUser = signal(DUMMY_USERS[randomIndex]);

  imagePath = computed(() => 'assets/users/' + this.selectedUser().avatar)


  //here we define a getter to allow is to use images
  // get imagePath()
  // {
  //   return 'assets/users/' + this.selectedUser.avatar
  // }

  //method to change the user when clicking
  //use a event listener in the html file
  onSelectUser()
  {
    const randomIndex = Math.floor(Math.random() * DUMMY_USERS.length);
    this.selectedUser.set(DUMMY_USERS[randomIndex]);
  }
}
