using CI_PlatForm.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Entities.ViewModel
{
    public class StoryViewModel
    {
        public long StoryId { get; set; }

        public long UserId { get; set; }

        public long MissionId { get; set; }

        public string? storyImage { get; set; }

        public string Theme { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Avatar { get; set; }

        public string? UserName { get; set; }
        public long CountryId { get; set; }

        public long CityId { get; set; }
        public long SkillId { get; set; }

        public long ThemeId { get; set; }

        [Required(ErrorMessage = "Please choose date")]
        public DateTime? PublishDate { get; set; }
        public string Type { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string? Url { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "You can upload maximum 20 images")]

        public List<IFormFile>? Images { get; set; }
        public List<StoryMedium>? storymedia { get; set; }
        public string? WhyIVolunteer { get; set; }

        public List<User>? users { get; set; }

    }
}
