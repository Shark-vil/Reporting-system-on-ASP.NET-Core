using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTING_WEB_API_ASP.Models.DocumentsModel
{
    /// <summary>
    /// Репозиторий доккументов
    /// </summary>
    public class DocumentsRepository : IDocuments
    {
        // Контекст
        internal Contexts.DatabaseContext context;
        // Таблица
        internal DbSet<Documents> db;

        /// <summary>
        /// Инициализация класса
        /// </summary>
        /// <param name="_context">Контекст</param>
        public DocumentsRepository(Contexts.DatabaseContext _context)
        {
            context = _context;
            db = context.Set<Documents>();
        }

        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="documents">Объект записи</param>
        /// <returns>Добавленная запись</returns>
        public Documents Add(Documents documents)
        {
            if (documents != null)
            {
                db.Add(documents);
                context.SaveChanges();
            }
            return documents;
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Удалённая запись</returns>
        public Documents Delete(int id)
        {
            Documents documents = context.Documents.FirstOrDefault(e => e.id == id);
            if (documents != null)
            {
                db.Remove(documents);
                context.SaveChanges();
            }
            return documents;
        }

        /// <summary>
        /// Получить список всех записей
        /// </summary>
        /// <returns>Все записи</returns>
        public IEnumerable<Documents> GetAllDocuments()
        {
            return db;
        }

        /// <summary>
        /// Получить запись по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Запись</returns>
        public Documents GetDocuments(int id)
        {
            Documents documents = context.Documents.FirstOrDefault(e => e.id == id);
            return documents;
        }

        /// <summary>
        /// Обновить запись по идентификатору
        /// </summary>
        /// <param name="documentsChange">Объект записи</param>
        /// <returns>Обновлённое значение</returns>
        public Documents Update(Documents documentsChange)
        {
            Documents documents = db.FirstOrDefault(e => e.id == documentsChange.id);
            if (documents != null)
            {
                documents.dataWrite = documentsChange.dataWrite;
                documents.fileObject = documentsChange.fileObject;
                documents.fileType = documentsChange.fileType;
                context.SaveChanges();
            }
            return documents;
        }
    }
}
