export interface Tenant {
  site: string | null,
  environment: string | null,
}

let tenant: Tenant = {
  site: null,
  environment: null
};

export function getTenant(): Tenant {
  return tenant;
}

export function setTenant(newTenant: Tenant) {
  tenant = newTenant;
}
