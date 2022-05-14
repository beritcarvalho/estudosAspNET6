using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Category
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O campo Name é necessário.")]
        [StringLength (40, MinimumLength = 3, ErrorMessage = "Máximo 40 caractere mínimo 3")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O campo Slug é necessário.")]
        public string Slug { get; set; }
    }
}
