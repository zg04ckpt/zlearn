import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { TokenInterceptor } from './core/interceptors/token.interceptor';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { ApiInterceptor } from './core/interceptors/api.interceptor';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations"
import { ToastComponent } from './shared/components/toast/toast.component';
import { MessageComponent } from './shared/components/message/message.component';
import { SidebarComponent } from './core/layout/sidebar/sidebar.component';
import { FooterComponent } from './core/layout/footer/footer.component';
import { HeaderComponent } from './core/layout/header/header.component';
import { DetailComponent } from './core/auth/components/detail/detail.component';
import { LoginComponent } from './core/auth/components/login/login.component';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@NgModule({
    declarations: [
        AppComponent,
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: TokenInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ApiInterceptor,
            multi: true
        },
    ],
    bootstrap: [AppComponent],
    imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    ToastComponent,
    MessageComponent,
    SidebarComponent,
    FooterComponent,
    HeaderComponent,
    CommonModule,
]
})
export class AppModule { }
