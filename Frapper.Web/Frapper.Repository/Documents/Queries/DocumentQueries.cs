using System;
using System.Linq.Dynamic.Core;
using System.Linq;
using Frapper.Entities.Documents;

namespace Frapper.Repository.Documents.Queries
{
    public class DocumentQueries : IDocumentQueries
    {
        private readonly FrapperDbContext _frapperDbContext;
        public DocumentQueries(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public DocumentUploadedFiles GetDocumentBydocumentId(long userid, int documentId)
        {
            var registerVerification = (from du in _frapperDbContext.DocumentUploadedFiles
                                        where du.CreatedBy == userid && du.DocumentId == documentId
                                        select du).FirstOrDefault();

            return registerVerification;
        }

        public IQueryable<DocumentUploadedFiles> ShowAllDocuments(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesdocuments = (from documentUploaded in _frapperDbContext.DocumentUploadedFiles
                                           select documentUploaded).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesdocuments = queryablesdocuments.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesdocuments = queryablesdocuments.Where(m => m.Name.Contains(search) || m.Name.Contains(search));
                }

                return queryablesdocuments;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}