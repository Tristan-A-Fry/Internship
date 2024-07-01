import { Component, Renderer2 } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-check-split',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './check-split.component.html',
  styleUrl: './check-split.component.css'
})
export class CheckSplitComponent implements OnInit {
  billAmount: number = 0;
  tipPercentage: number = 0;
  numPeople: number = 1;
  subtotal: number = 0;
  tipAmount: number = 0;
  totalPerPerson: number = 0;

  constructor(private renderer: Renderer2) {}

  ngOnInit(): void {
    this.renderer.addClass(document.body, 'check-list-bg');
    this.calculate();
  }
  ngOnDestroy(): void{
    this.renderer.removeClass(document.body, 'check-list-bg');
  }

  calculate(): void {
    this.subtotal = this.billAmount;
    this.tipAmount = (this.billAmount * this.tipPercentage) / 100;
    const total = this.subtotal + this.tipAmount;
    this.totalPerPerson = total / this.numPeople;
  }

  setTip(percentage: number): void {
    this.tipPercentage = percentage;
    this.calculate();
  }

  increasePeople(): void {
    this.numPeople++;
    this.calculate();
  }

  decreasePeople(): void {
    if (this.numPeople > 1) {
      this.numPeople--;
      this.calculate();
    }
  }
}
