export interface DatabaseEnvironment {
    Name: string
}

export interface DatabaseTenant {
    Name: string,
    Environments: DatabaseEnvironment[]
}
