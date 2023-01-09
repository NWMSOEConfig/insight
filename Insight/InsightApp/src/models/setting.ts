import { Category } from "./category";
import { Parameter } from "./parameter";
import { Tenant } from "./tenant";

export interface Setting {
  name: string,
  parameters: Parameter[] | null,
  category: Category | null,
  tenant: Tenant | null,
}
