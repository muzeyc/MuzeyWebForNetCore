using System.ComponentModel.DataAnnotations;

namespace MuzeyAngular.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}