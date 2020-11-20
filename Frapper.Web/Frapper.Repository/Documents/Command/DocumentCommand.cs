using Frapper.Entities.Documents;
using Frapper.Entities.Menus;
using Microsoft.EntityFrameworkCore;

namespace Frapper.Repository.Documents.Command
{
    public class DocumentCommand : IDocumentCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public DocumentCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public void Add(DocumentUploadedFiles documentUploaded)
        {
            _frapperDbContext.DocumentUploadedFiles.Add(documentUploaded);
        }

        public void Delete(DocumentUploadedFiles documentUploaded)
        {
            _frapperDbContext.Entry(documentUploaded).State = EntityState.Deleted;
        }

    }
}