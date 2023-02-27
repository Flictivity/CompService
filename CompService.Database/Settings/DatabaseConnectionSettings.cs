namespace CompService.Database.Settings;

public class DatabaseConnectionSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string UsersCollectionName { get; set; } = null!;
    public string UserVerificationCollectionName { get; set; } = null!;
}