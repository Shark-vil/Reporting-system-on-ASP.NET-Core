using System;
using System.Collections.Generic;
using SIO = System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TESTING_WEB_API_ASP.Contexts;
using TESTING_WEB_API_ASP.Models.DocumentsModel;
using Spire.Doc;
using System.Security.Cryptography;
using TESTING_WEB_API_ASP.Helpers;
using NLog;

namespace TESTING_WEB_API_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private DocumentsRepository repository;

        public DocumentsController(DatabaseContext context)
        {
            _context        = context;
            repository      = new DocumentsRepository(_context);
        }

        // GET: api/Documents
        [HttpGet]
        public IEnumerable<Documents> GetDocuments()
        {
            return repository.GetAllDocuments();
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocuments([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var documents = repository.GetDocuments(id);

            if (documents == null)
            {
                return NotFound();
            }

            // Название папки для хранения временных файлов
            string tempDirectory = LConfig.temp_dir;

            // Создание папки, если не существует
            if (!SIO.Directory.Exists(tempDirectory))
            {
                SIO.Directory.CreateDirectory(tempDirectory);
                HLogger.log.Info("Создание каталога:" + tempDirectory);
            }

            // Определение названия временного файла
            string tempFileHash = Hashing.GetMD5FileHash(documents.fileObject);
            string tempFileName = "temp_" + tempFileHash;

            // Определение относительного пути временного файла
            string tempFilePath = tempDirectory + "/" + tempFileName + ".temp";
            HLogger.log.Info("Генерация имени временного файла:" + tempFilePath);

            // Получение условного типа файла
            string fileType = HFile.getFileType(documents.fileExp);
            HLogger.log.Debug("Тип файла:" + fileType);

            // Создание временного потока в памяти для хранения конвертированного файла
            SIO.MemoryStream resultAnswer = null;

            // Если тип файла невалидный, тогда отказ
            if (fileType == null)
            {
                HLogger.log.Warn("Невалидный тип файла");
                return NoContent();
            }
            else
            // Иначе конвертируем в PDF
            {
                // Если файл относится к типу Word
                if (fileType == "doc")
                    resultAnswer = HFile.getConvertFileDoc(documents.fileObject, tempFilePath);
            }

            // Если поток не пустой, тогда отправляем файл пользователю
            if (resultAnswer != null)
                return File(resultAnswer, documents.fileType, documents.fileName + ".pdf");
            else
            {
                HLogger.log.Error("Потока с результатом вывода не существует");
                return NoContent();
            }
        }

        // PUT: api/Documents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocuments([FromRoute] int id, [FromBody] Documents documents)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != documents.id)
            {
                return BadRequest();
            }

            repository.Update(documents);

            return NoContent();
        }

        // POST: api/Documents
        [HttpPost]
        public async Task<IActionResult> PostDocuments(IFormFile fileObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Documents documents = new Documents();

            if (fileObject.Length > 0)
            {
                documents.fileName = SIO.Path.GetFileNameWithoutExtension(fileObject.FileName);
                documents.fileType = fileObject.ContentType;
                documents.dataWrite = DateTime.Today;
                documents.fileExp = SIO.Path.GetExtension(fileObject.FileName);

                using (var ms = new SIO.MemoryStream())
                {
                    fileObject.CopyTo(ms);
                    documents.fileObject = ms.ToArray();
                }
            }
            else
                return NoContent();

            repository.Add(documents);

            return CreatedAtAction("GetDocuments", new { documents.id }, documents);
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocuments([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Documents documents = repository.Delete(id);

            return Ok(documents);
        }
    }
}