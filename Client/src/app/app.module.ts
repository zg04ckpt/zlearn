import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TestListComponent } from './components/test-list/test-list.component';
import { TestComponent } from './components/test/test.component';
import { CommonModule } from '@angular/common';
import { AboutComponent } from './components/about/about.component';
import { PracticeComponent } from './components/practice/practice.component';
import { LoginComponent } from './components/login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    TestListComponent,
    TestComponent,
    AboutComponent,
    PracticeComponent,
    LoginComponent
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
