using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.UserFactoryClasses;

namespace VCTicketTrackerAPIClassLibrary.Auth
{
    public class VCTicketTrackerUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>
    {
        private bool disposedValue;

        public VCTicketTrackerUserStore()
        {
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Create", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                //result = DatabaseAccesUtil.GetUser(Convert.ToInt64(userId));
                //UserClient t = new UserClient(user);

                //var result = t.Get().Insert();
                var result = user.Insert();
                if(result)
                {
                    return Task.FromResult(IdentityResult.Success);
                }
                else
                {

                    Debug.WriteLine("Failed to insert user into the data store.", "UserStore Error");
                    return Task.FromResult(IdentityResult.Failed(new IdentityError {
                        Description = "Failed to insert user into the data store."
                    }));
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "UserStore Error");
                return Task.FromResult(IdentityResult.Failed(new IdentityError {
                    Description = $"An error occurred while creating the user: {ex.Message}"
                }));
            }
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Delete User:\n\t{user}", "UserStore");
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Find by Id", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{userId}");
            Debug.IndentLevel--;

            ApplicationUser? result = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                result = (ApplicationUser?)DatabaseAccesUtil.GetUser(Convert.ToInt64(userId));
                if(result != null)
                {
                    UserClient t = new UserClient(result);

                    result = t.Get();
                    return Task.FromResult(result);
                }
                else
                {
                    Task.FromResult<ApplicationUser>(null);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"User Store FindBy Id Error:\n{ex.Message}", "UserStore Error");
                //return Task.FromResult(result);

            }
            return Task.FromResult<ApplicationUser>(null);

        }
        /*
        public Task<ApplicationUser?> FindByIdAsync(long userId, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Find by Id", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{userId}");
            Debug.IndentLevel--;

            ApplicationUser result = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                result = DatabaseAccesUtil.GetUser(userId);
                UserClient t = new UserClient(result);

                result = t.Get();
                if(result != null)
                {
                    return Task.FromResult(result);
                }
                else
                {
                    throw new NullReferenceException("User not found");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"User Store FindBy Id Error:\n{ex.Message}", "UserStore Error");
                return Task.FromResult(result);

            }

        }
        */
        public Task<ApplicationUser?>? FindByNameAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Find by NormName", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{normalizedEmail}");
            Debug.IndentLevel--;
            ApplicationUser? result = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                result = DatabaseAccesUtil.GetUser(normalizedEmail);
                if(result != null)
                {
                    UserClient t = new UserClient(result);

                    result = t.Get();
                    return Task.FromResult(result);
                }
                else
                {
                    //throw new NullReferenceException("No user with that email exits in the database");
                    //return null;
                    return Task.FromResult<ApplicationUser?>(null);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"User Store Find by NormName Error:\n{ex.ToString()}", "UserStore Error");
                return Task.FromResult<ApplicationUser?>(null);
            }
        }

        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Get norm user name", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //var result = DatabaseAccesUtil.GetUser(user.Id);

                if(user != null)
                {
                    return Task.FromResult(user.NormalizedEmail);
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"User Store FindBy Id Error:\n{ex.ToString()}", "UserStore Error");
                return null;
            }
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Get Password", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();


                if(user != null)
                {
                    return Task.FromResult(user.PasswordHash);
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"User Store FindBy Id Error:\n{ex.ToString()}", "UserStore Error");
                return null;
            }
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Get User ID", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();


                if(user != null)
                {
                    return Task.FromResult(user.Id.ToString());
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Get by Id ({user.Id}):\n{ex.ToString()}", "UserStore Error");
                return null;
            }
        }

        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Get Username", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if(user != null)
                {
                    return Task.FromResult(user.Email);
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Get by Username ({user.UserName}):\n\t{ex.ToString()}", "UserStore Error");
                return null;
            }
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Chekc Has password", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();


                if(user != null)
                {
                    return Task.FromResult(user.PasswordHash != null);
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"User Store FindBy Id Error:\n\t{ex.ToString()}", "UserStore Error");
                return null;
            }
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Set Norm userName", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"normal Usename:{normalizedName}");
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //var result = DatabaseAccesUtil.SetNormUsername(user.Id,normalizedName);

                if(user != null)
                {
                    user.NormalizedUserName = normalizedName.ToUpper().Split('@')[0];
                    return Task.CompletedTask;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"User Store FindBy Id Error:\n{ex.ToString()}", "UserStore Error");
                return Task.CompletedTask;
            }
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Set HashPassword", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"Password:{passwordHash}");
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //var result = DatabaseAccesUtil.SetPassword(user.Id,passwordHash);

                if(user != null)
                {
                    //user.PasswordHash = UserHandler._PasswordHasher(passwordHash, user.Email);
                    user.PasswordHash = passwordHash;
                    return Task.CompletedTask;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"User Store FindBy Id Error:\n{ex.ToString()}", "UserStore Error");
                return Task.CompletedTask;
            }
        }

        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Set Username", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //var result = DatabaseAccesUtil.SetUsername(user.Id,userName);

                if(user != null)
                {
                    user.Email =userName;
                    user.UserName =userName;
                    return Task.CompletedTask;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Faild to Set Username:\n{ex.ToString()}", "UserStore Error");
                return Task.CompletedTask;
            }
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Update Full", "UserStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = DatabaseAccesUtil.UpdateUsers(user);

                if(result != null)
                {
                    return Task.FromResult(IdentityResult.Success);
                }
                else
                {
                    return Task.FromResult(IdentityResult.Failed(new IdentityError {
                        Description = "Failed to update user into the data store."
                    }));
                }
            }
            catch(Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError {
                    Description = "Failed to update user into the data store."
                }));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue=true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~VCTicketTrackerUserStore()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Add User to Role:", "UserStore-AddToRole");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.WriteLine($"Normalized Role Name:{roleName}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                DatabaseAccesUtil.AddUserToRole(user.Id, roleName);
                return Task.CompletedTask;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "AddToRoleError");
                throw;
            }
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Remove User from Role:", "UserStore-RemoveFromRole");
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                DatabaseAccesUtil.RemoveUserFromRole(user.Id, roleName);
                return Task.CompletedTask;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "RemoveFromRoleError");
                throw;
            }
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Get Roles for User:", "UserStore-GetRoles");
            Debug.IndentLevel++;
            Debug.WriteLine($"User ID:{user.Id}");
            Debug.WriteLine($"User:\n\t{user}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var roles = DatabaseAccesUtil.GetRolesForUser(user.Id);
                Debug.WriteLine(roles.Count);
                for(int i = 0; i < roles.Count; i++)
                {
                    Debug.WriteLine(roles[i]);
                }
                return Task.FromResult(roles);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetRolesError");
                throw;
            }
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Check if User is in Role:", "UserStore-IsInRole");
            Debug.IndentLevel++;
            Debug.WriteLine($"User:\n\t{user}");
            Debug.WriteLine($"Normalized Role Name:{roleName}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                bool isInRole = DatabaseAccesUtil.IsUserInRole(user.Id, roleName);
                return Task.FromResult(isInRole);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "IsInRoleError");
                throw;
            }
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Get User by to Role:", "UserStore-GetRole");
            Debug.IndentLevel++;
            Debug.WriteLine($"Normalized Role Name:{roleName}");
            Debug.IndentLevel--;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var users = DatabaseAccesUtil.GetUsersInRole(roleName);
                return Task.FromResult(users);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetUsersInRoleError");
                throw;
            }
        }
    }
}
