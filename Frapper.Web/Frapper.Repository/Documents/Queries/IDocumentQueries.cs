using System.Linq;
using Frapper.Entities.Documents;

namespace Frapper.Repository.Documents.Queries
{
    public interface IDocumentQueries
    {
        DocumentUploadedFiles GetDocumentBydocumentId(long userid, int documentId);
        IQueryable<DocumentUploadedFiles> ShowAllDocuments(string sortColumn, string sortColumnDir, string search);
    }
}