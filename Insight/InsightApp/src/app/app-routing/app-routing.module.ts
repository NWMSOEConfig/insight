import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from '../app.component';
import { CommitPageComponent } from '../commit-page/commit-page.component';
import { ConfigurationPageComponent } from '../configuration-page/configuration-page.component';
import { HistoryPageComponent } from '../history-page/history-page.component';
import { PublishPageComponent } from '../publish-page/publish-page.component';

const routes: Routes = [
  {
    path: 'configuration',
    redirectTo: '',
    pathMatch: 'full',
  },
  {
    path: '',
    component: ConfigurationPageComponent,
  },
  {
    path: 'publish',
    component: PublishPageComponent,
  },
  {
    path: 'history',
    component: HistoryPageComponent,
  },
  {
    path: 'history/commit/:id',
    component: CommitPageComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
