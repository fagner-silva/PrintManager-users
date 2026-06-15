namespace PrintManager.Users.Infra.Settings
{
    public class MongoDbSettings
    {
        public const string SectionName = "MongoDb";

        public string ConnectionString { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        public string UsersCollectionName { get; set; } = "users";

        public string CompaniesCollectionName { get; set; } = "companies";

        public string CompanyMembersCollectionName { get; set; } = "company-members";
    }
}
