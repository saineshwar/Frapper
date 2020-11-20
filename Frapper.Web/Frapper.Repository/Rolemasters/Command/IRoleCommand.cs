using Frapper.Entities.Rolemasters;

namespace Frapper.Repository.Rolemasters.Command
{
    public interface IRoleCommand
    {
        void Add(RoleMaster roleMaster);
        void Update(RoleMaster roleMaster);
        void Delete(RoleMaster roleMaster);
    }
}