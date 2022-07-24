using Microsoft.AspNetCore.Mvc.Rendering;

namespace SD_310_W22SD_Assignment.Models.ViewModels
{
    public class UserSongSelectViewModel
    {
        public List<SelectListItem> UserSelect { get; set; }
        public List<SelectListItem> SongSelect { get; set; }

        public UserSongSelectViewModel(List<Song> songs, List<User> users)
        {
            SongSelect = new List<SelectListItem>();
            UserSelect = new List<SelectListItem>();
            songs.ForEach(s => SongSelect.Add(new SelectListItem(s.Title, s.Id.ToString())));
            users.ForEach(u=> UserSelect.Add(new SelectListItem(u.Name, u.Id.ToString())));
            
        }

        
    }
}
