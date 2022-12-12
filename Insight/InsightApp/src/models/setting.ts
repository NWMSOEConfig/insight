import { Category } from "./category";
import { Tenant } from "./tenant";

export interface Setting {
  name: string,
  parameterIds: number[] | null,
  category: Category | null,
  tenant: Tenant | null,
}
