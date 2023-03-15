﻿namespace CompService.Database.Settings;

public class DatabaseConnectionSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string UsersCollectionName { get; set; } = null!;
    public string UserVerificationCollectionName { get; set; } = null!;
    public string SourcesCollectionName { get; set; } = null!;
    public string ClientsCollectionName { get; set; } = null!;
    public string DefectsCollectionName { get; set; } = null!;
    public string BrandsCollectionName { get; set; } = null!;
    public string AppearancesCollectionName { get; set; } = null!;
    public string DeviceTypesCollectionName { get; set; } = null!;
}