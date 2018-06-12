namespace SqlHelper2 {
    public class DatabaseFactory {
        public static IDatabaseAsync CreateDatabase(string connectionStringName = "DefaultConnection") {
            return new ConnectionDatabase(connectionStringName);
        }
    }
}
