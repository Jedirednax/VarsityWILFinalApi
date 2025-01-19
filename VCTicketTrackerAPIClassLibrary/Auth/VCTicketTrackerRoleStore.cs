using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using VCTicketTrackerAPIClassLibrary.StorageDB;

namespace VCTicketTrackerAPIClassLibrary.Auth
{
    public class VCTicketTrackerRoleStore : IRoleStore<ApplicationRole>
    {
        private bool disposedValue;

        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Create Role", "RoleStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"Role Name:\n\t{role.Name}");
            Debug.IndentLevel--;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Insert the role into the database
                bool success = DatabaseAccesUtil.InsertStaffType(role);

                // Check the result of the insert operation
                if(!success)
                {
                    Debug.WriteLine("Failed to create role", "RoleStore Warning");
                    return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Failed to create role." }));
                }

                return Task.FromResult(IdentityResult.Success);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Role Store CreateRole Error:\n{ex.Message}", "RoleStore Error");
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "An error occurred while creating the role." }));
            }
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Delete Role", "RoleStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"Role ID:\n\t{role.Id}");
            Debug.IndentLevel--;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Logic to delete the role from the database
                bool success = DatabaseAccesUtil.DeleteRole(role.Id);

                if(!success)
                {
                    Debug.WriteLine("Failed to delete role", "RoleStore Warning");
                    return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Failed to delete role." }));
                }

                return Task.FromResult(IdentityResult.Success);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Role Store DeleteRole Error:\n{ex.Message}", "RoleStore Error");
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "An error occurred while deleting the role." }));
            }
        }

        public Task<ApplicationRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Find Role by Id", "RoleStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"Role ID:\n\t{roleId}");
            Debug.IndentLevel--;

            ApplicationRole result = null;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                result = DatabaseAccesUtil.GetRole(Convert.ToInt64(roleId));

                if(result == null)
                {
                    Debug.WriteLine("Role not found", "RoleStore Warning");
                    throw new NullReferenceException("Role not found");
                }

                return Task.FromResult(result);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Role Store FindById Error:\n{ex.Message}", "RoleStore Error");
                return Task.FromResult(result);
            }
        }

        public Task<ApplicationRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Find Role by Name", "RoleStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"Normalized Role Name:\n\t{normalizedRoleName}");
            Debug.IndentLevel--;

            ApplicationRole result = null;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                result = DatabaseAccesUtil.GetRole(normalizedRoleName);

                if(result == null)
                {
                    Debug.WriteLine("Role not found", "RoleStore Warning");
                    throw new NullReferenceException("Role not found");
                }

                return Task.FromResult(result);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Role Store FindByName Error:\n{ex.Message}", "RoleStore Error");
                return Task.FromResult(result);
            }
        }

        public async Task<string?> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return role.NormalizedName;
        }

        public async Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return role.Id.ToString();
        }

        public async Task<string?> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return role.Name;
        }

        public async Task SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
        }

        public async Task SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Update Role", "RoleStore");
            Debug.IndentLevel++;
            Debug.WriteLine($"Role ID:\n\t{role.Id}");
            Debug.WriteLine($"Role Name:\n\t{role.Name}");
            Debug.IndentLevel--;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Logic to update the role in the database
                bool success = DatabaseAccesUtil.UpdateRole(role);

                if(!success)
                {
                    Debug.WriteLine("Failed to update role", "RoleStore Warning");
                    return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Failed to update role." }));
                }

                return Task.FromResult(IdentityResult.Success);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Role Store UpdateRole Error:\n{ex.Message}", "RoleStore Error");
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "An error occurred while updating the role." }));
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
        // ~VCTicketTrackerRoleStore()
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
    }
}
