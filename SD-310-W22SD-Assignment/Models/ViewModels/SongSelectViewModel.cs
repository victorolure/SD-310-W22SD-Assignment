using Microsoft.AspNetCore.Mvc.Rendering;

namespace SD_310_W22SD_Assignment.Models.ViewModels
{
    public class SongSelectViewModel
    {
        public List<SelectListItem> SongSelect { get; set; }
        public SongSelectViewModel(List<Song> songs)
        {
            SongSelect = new List<SelectListItem>();
            songs.ForEach(s =>
            {
                SongSelect.Add(new SelectListItem(s.Title, s.Id.ToString()));
            });
        }

    }
}
