using Frapper.Entities.Documents;

namespace Frapper.Repository.Documents.Command
{
    public interface IDocumentCommand
    {
        void Add(DocumentUploadedFiles documentUploaded);
        void Delete(DocumentUploadedFiles documentUploaded);
    }
}