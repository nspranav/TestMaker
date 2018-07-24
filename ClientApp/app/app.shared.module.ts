import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from "@angular/common/http";

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { QuizListComponent } from "./components/quiz/quiz-list.component";
import { QuizComponent } from "./components/quiz/quiz.component";
import { AboutComponent } from "./components/about/about.component";
import { LoginComponent } from './components/login/login.component';
import { PageNotFoundComponent } from './components/pagenotfound/pagenotfound.component';
import { QuizEditComponent } from './components/quiz/quiz-edit.component';


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        QuizListComponent,
        QuizComponent,
        AboutComponent,
        LoginComponent,
        PageNotFoundComponent,
        QuizEditComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'quiz/create', component: QuizEditComponent },
            { path: 'quiz/:id', component: QuizComponent },
            { path: 'login', component: LoginComponent },
            { path: 'about', component: AboutComponent },
            { path: '**', component: PageNotFoundComponent }
        ])
    ]
})
export class AppModuleShared {
}
