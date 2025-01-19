using System.Security.Cryptography;
using System.Text;
using VCTicketTrackerAPIClassLibrary.Auth;

namespace VCTicketTrackerAPIClassLibrary
{
    /// <summary>
    /// Handler/Helper For Logging in and Registering User.
    /// As well as Authorisation * spelling.
    /// </summary>
    public class UserHandler
    {

        /// <summary>
        /// Checks the privide credintials against a database, for if the user exists and then
        /// checks if the usernmae and (hashed)password match to retunr the found user,
        /// With their populated details.
        /// </summary>
        /// <param name="cred"> A object contaning only username and password </param>
        /// <returns> Returns a user as the speified child class of usertype
        /// </returns>
        /*public static Users? VerifyLoginDetails(UserCredential cred)
        {
            // Verify User name and password are not empty else return results in null.
            if(cred.Username != null && cred.Password != null)
            {
                // Create user holder if the user exists
                var currUser = DatabaseAccesUtil.GetUser(cred.Username);
                if(currUser != null)
                {
                    // Weak check if password is pre hashed
                    if(cred.Password.Length <= 63)
                    {
                        if(_PasswordHasher(cred.Password, cred.Username).Equals(currUser.PasswordHash))
                        {
                            // Fetch the details of the authprised user
                            currUser.GetDetails();
                            return currUser;
                        }
                    }
                    else
                    {
                        // If password has been prehased
                        if(cred.Password.Equals(currUser.PasswordHash))
                        {
                            // Fetch the details of the authprised user
                            currUser.GetDetails();
                            return currUser;
                        }
                    }
                }
            }
            // Return if no instance or invalidated user is returned
            return null;
        }
        */
        /// <summary>
        /// Hashes the new users password if not, then Attempts to insert the user in to teh database
        /// </summary>
        /// <param name="newUser"> A user object to be inserted in th the data base </param>
        /// <returns> a bool indicating if the insert was successful </returns>
        [Obsolete]
        public static bool RegisterNewUser(ApplicationUser newUser)
        {
            // TODO Check Email Pattern [Feature]
            // add method to check the Username/username of a registering User,
            // to check they are avalid Username

            if(newUser != null)
            {

                if(newUser.PasswordHash.Length <= 63)
                {
                    newUser.PasswordHash  = _PasswordHasher(newUser.PasswordHash, newUser.UserName);
                }
                return newUser.Insert();
            }
            return false;
        }

        public static bool VerifyUserAccess()
        {
            return true;
        }

        /// <summary>
        /// Is used to convert a passed string in to a hased string of 64 bytes.
        /// Joins username and password to make the hash
        /// </summary>
        /// <param name="password"> The unhased Password </param>
        /// <param name="username"> The username to be used as a salt </param>
        /// <returns>
        /// Returns a SHA256 (64)Hased Password
        /// </returns>
        public static string _PasswordHasher(string password, string username)
        {
            // Generates a salt with the username
            string salt = username.ToLower();
            byte[] passwordByte;
            byte[] hashedPassword;
            // Create as a hash with built in SHA256 finction
            using(SHA256 HashMaker = SHA256.Create())
            {
                // Retives pass word in to bytes with salt(username)
                passwordByte = Encoding.UTF8.GetBytes(password + salt);
                // Computes the hash
                hashedPassword = HashMaker.ComputeHash(passwordByte);
                // Retuns the formated Hash
                string hash = BitConverter.ToString(hashedPassword).Replace("-", "");

                return hash;
            }
        }
    }
}
