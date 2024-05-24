import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ListQuestionSetComponent } from './components/list-question-set/list-question-set.component';
import { TestComponent } from './components/test/test.component';
import { CommonModule } from '@angular/common';
import { AboutComponent } from './components/about/about.component';
import { PraticeComponent } from './components/pratice/pratice.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    ListQuestionSetComponent,
    TestComponent,
    AboutComponent,
    PraticeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
