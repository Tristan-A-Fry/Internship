import { Routes } from '@angular/router';
import { CheckSplitComponent } from './check-split/check-split.component';
import { RgbSliderComponent } from './rgb-slider/rgb-slider.component';
import { HomePageComponent } from './home-page/home-page.component';
import { HangmanComponent } from './hangman/hangman.component';

export const routes: Routes = [
    {
        path: '',
        component: HomePageComponent,
    },
    {
        path: 'check', 
        component: CheckSplitComponent,
    },
    {
        path: 'rgb',
        component: RgbSliderComponent,
    },
    {
        path: 'hangman',
        component: HangmanComponent,
    },
];
