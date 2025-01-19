namespace VCTicketTrackerAPIClassLibrary.StorageDB
{
    /// <summary>
    /// Static "thread safe" store for the connection string
    /// </summary>
    public class DatabaseConnection
    {
        
        /// <summary>
        /// Store of the active connection string for the database
        /// </summary>
        private static string ConneStr;
        /* private static FirebaseApp fbApp;
        */
        public static string conneStr
        {
            get
            {
                return ConneStr;
            }

            set
            {
                ConneStr=value;
            }
        }


        private static readonly object padlock = new object();

        private DatabaseConnection()
        {

        }

        public static string Connection
        {
            get
            {
                lock(padlock)
                {
                    return ConneStr;
                }
            }
        }
        // TODO Add Documnet Store [Feature]
    }
}
