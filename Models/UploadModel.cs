using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using reenbitBlob.Controllers.Helpers;
using System.ComponentModel.DataAnnotations;

namespace reenbitBlob.Models
{
    public class UploadModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "File is required.")]
        [AllowedExtensions(new string[] { ".docx" })]
        public IFormFile File { get; set; }
    }
}
