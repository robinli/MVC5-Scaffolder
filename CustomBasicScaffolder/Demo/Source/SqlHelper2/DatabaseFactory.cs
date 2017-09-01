namespace SqlHelper2 {
    public class DatabaseFactory {
        public static IDatabase CreateDatabase(string connectionStringName = "DefaultConnection") {
            return new ConnectionDatabase(connectionStringName);
        }
    }
}
