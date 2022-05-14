using Blog.Models;

namespace Blog.ViewModels.Accounts
{
    public class ResultUserViewModel
    {
        
        public ResultUserViewModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Bio = user.Bio;
            Image = user.Image;
            Slug = user.Slug;
            Password = user.PasswordHash;
    }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
        public string Password { get; set; }

    }
}
