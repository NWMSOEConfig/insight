import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ConfigurationPageComponent } from './configuration-page/configuration-page.component';

const routes: Routes = [
  {
    path: '',
    component: ConfigurationPageComponent,
  },
  {
    path: 'publish',
    component: ConfigurationPageComponent
  },
  {
    path: 'history',
    component: ConfigurationPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
