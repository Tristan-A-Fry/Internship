import { Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-hangman',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './hangman.component.html',
  styleUrl: './hangman.component.css'
})

export class HangmanComponent implements OnInit {
  word: string = '';
  displayedWord: string[] = [];
  guessedLetters: string[] = [];
  wrongGuesses: number = 0;
  maxErrors: number = 6;
  alphabet: string[] = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split('');

  constructor() {
    this.resetGame(); //initalized the game when the componenet is created
  }

  ngOnInit(): void {}

  getImagePath(): string {
    //just gets the image path
    return `./assets/hangman/img${this.wrongGuesses}.jpg`;
  }

  guessLetter(letter: string): void {
    // we have these or statemetns to check if the game is won or lost so we can disable all buttons depending on game state
    if (this.guessedLetters.includes(letter) || this.isGameWon() || this.isGameLost()) {
      return; //if letter has already been guessed do nothing
    }

    this.guessedLetters.push(letter); // add guessed letter to array

    if (this.word.includes(letter)) {
      //if guessed letter is in the word
      this.word.split('').forEach((char, index) => {
        if (char === letter) {
          this.displayedWord[index] = letter; //reveal letter in the displayed word
        }
      });
    } else {
      this.wrongGuesses++;
    }
  }

  isGameWon(): boolean {
    return this.displayedWord.join('') === this.word;
  }

  isGameLost(): boolean {
    return this.wrongGuesses >= this.maxErrors;
  }

  resetGame(): void {
    const words  = ['KYLIE IS A VAMPIRE'];
    // const words = ['ANGULAR', 'JAVASCRIPT', 'TYPESCRIPT', 'HANGMAN', 'TALOS',' KYLIE IS A VAMPIRE'];
    this.word = words[Math.floor(Math.random() * words.length)];
    this.displayedWord = Array(this.word.length).fill('_');
    this.guessedLetters = [];
    this.wrongGuesses = 0;
  }
}