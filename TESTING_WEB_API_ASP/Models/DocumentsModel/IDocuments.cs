using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTING_WEB_API_ASP.Models.DocumentsModel
{
    interface IDocuments
    {
        Documents GetDocuments(int id);
        IEnumerable<Documents> GetAllDocuments();
        Documents Add(Documents documents);
        Documents Update(Documents documentsChange);
        Documents Delete(int id);
    }
}
