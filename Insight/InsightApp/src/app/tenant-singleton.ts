export interface Tenant {
  site: string | null,
  environment: string | null,
}

let tenant: Tenant = {
  site: null,
  environment: null
};

export function getTenant(): Tenant {
  const noTenant = JSON.stringify({ site: null, environment: null });
  return JSON.parse(localStorage.getItem('tenant') || noTenant);
}

export function setTenant(newTenant: Tenant) {
  tenant = newTenant;
}
