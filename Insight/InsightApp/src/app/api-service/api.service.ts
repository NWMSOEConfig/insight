import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { getTenant } from '../tenant-singleton';
/* Models */
import { Setting } from '../../models/setting';
import { Category } from '../../models/category';
import { Commit } from '../../models/commit';
import { Subcategory } from '../../models/subcategory';
import { Tenant } from '../../models/tenant';
import { DatabaseSetting } from '../../models/databaseSetting';
import { DatabaseTenant } from '../../models/databaseTenant';
import { QueueEntry } from '../../models/queueEntry';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  public readonly apiURL = 'https://localhost:8000/api'; // TODO: fix this - set to './api' for publishing

  constructor(private httpClient: HttpClient) {}

  public getSetting(name: string) {
    const context = getTenant();
    return this.httpClient.get<Setting>(
      `${this.apiURL}/data/livesetting?settingName=${name}&tenantName=${context.site}&environmentName=${context.environment}`
    );
  }

  public getAllSettings() {
    const context = getTenant();
    return this.httpClient.get<DatabaseSetting[]>(
      `${this.apiURL}/data/dbsettings?tenantName=${context.site}&environmentName=${context.environment}`
    );
  }

  public getAllTenants() {
    const context = getTenant();
    return this.httpClient.get<DatabaseTenant[]>(
      `${this.apiURL}/data/dbtenants`
    );
  }

  public getCategory(id: number) {
    return this.httpClient.get<Category>(
      `${this.apiURL}/data/category?id=${id}`
    );
  }

  public getSubcategory(id: number) {
    return this.httpClient.get<Subcategory>(
      `${this.apiURL}/data/subcategory?id=${id}`
    );
  }

  public getTenant(id: number) {
    return this.httpClient.get<Tenant>(`${this.apiURL}/data/tenant?id=${id}`);
  }

  public getQueue() {
    const context = getTenant();
    return this.httpClient.get<QueueEntry>(
      `${this.apiURL}/data/queue?tenantName=${context.site}&environmentName=${context.environment}`
    );
  }

  public postQueue(setting: Setting) {
    const context = getTenant();
    return this.httpClient.post(
      `${this.apiURL}/data/queue?tenantName=${context.site}&environmentName=${context.environment}`,
      setting,
      {
        responseType: 'text',
      }
    );
  }

  public postPublish(commitMessage: string, referenceId: number) {
    const context = getTenant();
    return this.httpClient.post(
      `${this.apiURL}/data/publish?tenant=${context.site}&environment=${context.environment}&commitMessage=${commitMessage}&referenceId=${referenceId}`,
      {}
    );
  }

  public postPopulate(url: string, tenant: string, environment: string) {
    return this.httpClient.post(
      `${this.apiURL}/data/populate?tenant=${tenant}&environment=${environment}`,
      JSON.stringify(url),
      {
        headers: {
          'Content-Type': 'application/json',
        },
      }
    );
  }

  public deleteSettingFromQueue(settingName: String) {
    // write similar to above to call deleteSettingFromQueue
    const context = getTenant();
    return this.httpClient.delete(
      `${this.apiURL}/data/queue?tenantName=${context.site}&environmentName=${context.environment}&settingName=${settingName}`
    );
  }

  public getAllCommits() {
    const context = getTenant();
    return this.httpClient.get<Commit[]>(
      `${this.apiURL}/data/commits?tenantName=${context.site}&environmentName=${context.environment}`
    );
  }

  public getCommit(id: number) {
    const context = getTenant();
    return this.httpClient.get<Commit>(
      `${this.apiURL}/data/commit?tenantName=${context.site}&environmentName=${context.environment}&id=${id}`
    );
  }

  public getCommitsBySetting(settingName: string) {
    const context = getTenant();
    return this.httpClient.get<Commit[]>(
      `${this.apiURL}/data/commits/setting?tenantName=${context.site}&environmentName=${context.environment}&settingName=${settingName}`
    );
  }
}
