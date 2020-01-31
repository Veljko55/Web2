import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AgmCoreModule } from '@agm/core';


import { AppComponent } from './app.component';
import { LoginComponent } from './auth/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { RegisterComponent } from './register/register.component';
import { StartComponent } from './start/start.component';
import { PriceListComponent } from './price-list/price-list.component';
import { UnregistredComponent } from './unregistred/unregistred.component';
import { HeaderComponent } from './header/header.component';
import { LogoutComponent } from './logout/logout.component';
import { ScheduleComponent } from './components/schedule/schedule.component';
import { ProfileViewComponent } from './profile-view/profile-view.component';
import { ValidateProfileComponent } from './validate-profile/validate-profile.component';
import { ValidateTicketComponent } from './validate-ticket/validate-ticket.component';
import { ScheduleAdminComponent } from './components/schedule-admin/schedule-admin.component';
import { JwtInterceptor } from './auth/jwt-interceptor';
import { PriceListAdminComponent } from './components/price-list-admin/price-list-admin.component';
import { AdminStationComponent } from './admin-station/admin-station.component';
import { MapComponent } from './admin-station/map/map.component';
import { LineMeshComponent } from './line-mesh/line-mesh.component';
import { LineMeshAdminComponent } from './components/line-mesh-admin/line-mesh-admin.component';
import { VehicleLocationComponent } from './vehicle-location/vehicle-location.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    StartComponent,
    PriceListComponent,
    UnregistredComponent,
    HeaderComponent,
    LogoutComponent,
    ScheduleComponent,
    ScheduleAdminComponent,
    ProfileViewComponent,
    ValidateProfileComponent,
    ValidateTicketComponent,
    AdminStationComponent,
    MapComponent,
    PriceListAdminComponent,
    LineMeshComponent,
    LineMeshAdminComponent,
    VehicleLocationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    AgmCoreModule.forRoot({apiKey: 'AIzaSyDnihJyw_34z5S1KZXp90pfTGAqhFszNJk'})

  ],
  providers: [{provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
