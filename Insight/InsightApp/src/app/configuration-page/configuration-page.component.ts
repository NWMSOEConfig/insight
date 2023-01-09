import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api-service/api.service';
import { getTenant } from '../tenant-singleton';

@Component({
  selector: 'app-configuration-page',
  templateUrl: './configuration-page.component.html',
  styleUrls: ['./configuration-page.component.css']
})
export class ConfigurationPageComponent implements OnInit {
  disabled = false;
  secondsLeft = 0;
  timer: any;

  constructor(private apiService: ApiService) {
  }

  ngOnInit(): void {
  }

  clickRefresh(): void {
    this.disabled = true;

    const tenant = getTenant();

    this.apiService.postPopulate('https://pauat.newworldnow.com/v7/api/applicationsettings/', tenant.site ?? '', tenant.environment ?? '').subscribe(object => {
      const dateSeconds = Date.parse(object as string) / 1000;
      const nowSeconds = Date.now() / 1000;
      this.secondsLeft = Math.ceil(dateSeconds + 300 - nowSeconds);
      this.timer = setInterval(() => this.tick(), 1000);
    });
  }

  tick(): void {
    if (this.secondsLeft <= 0) {
      this.disabled = false;
      clearInterval(this.timer);
    } else {
      this.secondsLeft--;
    }
  }
}
