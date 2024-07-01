import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-rgb-slider',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './rgb-slider.component.html',
  styleUrls: ['./rgb-slider.component.css']
})
export class RgbSliderComponent {
  red: number = 0;
  green: number = 0;
  blue: number = 0;
  currentColor: string = 'rgb(0, 0, 0)';

  updateColor(): void {
    this.currentColor = `rgb(${this.red}, ${this.green}, ${this.blue})`;
  }

  generateRandomColor(): void {
    this.red = Math.floor(Math.random() * 256);
    this.green = Math.floor(Math.random() * 256);
    this.blue = Math.floor(Math.random() * 256);
    this.updateColor();
  }

  copyHexColor(): void {
    const hexColor = this.rgbToHex(this.red, this.green, this.blue);
    navigator.clipboard.writeText(hexColor).then(() => {
      alert('Hex color copied to clipboard: ' + hexColor);
    }, (err) => {
      console.error('Failed to copy text: ', err);
    });
  }

  rgbToHex(r: number, g: number, b: number): string {
    return '#' + [r, g, b].map(x => {
      const hex = x.toString(16);
      return hex.length === 1 ? '0' + hex : hex;
    }).join('');
  }
}
