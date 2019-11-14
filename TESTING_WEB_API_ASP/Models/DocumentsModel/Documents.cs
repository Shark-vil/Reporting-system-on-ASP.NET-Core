using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TESTING_WEB_API_ASP.Models.DocumentsModel
{
    /// <summary>
    /// Модель файла
    /// </summary>
    public class Documents
    {
        // Идентификатор
        [Key]
        public int id { get; set; }
        // Изначальное название документа
        [MaxLength(150, ErrorMessage = "Количество символов не должно превышать 150")]
        public string fileName { get; set; }
        // Тип документа ( Пример: doc, docx, xlsx и т.д. )
        [MaxLength(150, ErrorMessage = "Количество символов не должно превышать 20")]
        public string fileExp { get; set; }
        // Системный тип документа
        [Required]
        [MaxLength(20, ErrorMessage = "Количество символов не должно превышать 20")]
        public string fileType { get; set; }
        // Дата записи
        public DateTime dataWrite { get; set; }
        // Содержимое файла
        [Required]
        public byte[] fileObject { get; set; }
    }
}
