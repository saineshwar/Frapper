using Frapper.Entities.Rolemasters;
using Microsoft.EntityFrameworkCore;

namespace Frapper.Repository.Rolemasters.Command
{
    public class RoleCommand : IRoleCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public RoleCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public void Delete(RoleMaster roleMaster)
        {
            _frapperDbContext.Entry(roleMaster).State = EntityState.Deleted;
        }

        public void Add(RoleMaster roleMaster)
        {
            _frapperDbContext.RoleMasters.Add(roleMaster);
        }

        public void Update(RoleMaster roleMaster)
        {
            _frapperDbContext.Entry(roleMaster).State = EntityState.Modified;
        }
    }
}