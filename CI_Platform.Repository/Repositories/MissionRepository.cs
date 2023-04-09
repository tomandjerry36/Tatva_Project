using CI_PlatForm.Entities.Data;
using CI_PlatForm.Entities.Models;
using CI_PlatForm.Entities.ViewModel;
using CI_PlatForm.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Repository.Repositories
{
    public class MissionRepository : IMissionRepository
    {
        public readonly CiplatformContext _CiplatformDbContext;
        //private object commentView;
        //private object allDetailsCard;


        public MissionRepository(CiplatformContext ciplatformDbContext)
        {
            _CiplatformDbContext = ciplatformDbContext;

        }

        //---------------------Second Nav bar Drop down----------------------------------------------------------------

        public List<Country> GetCountryData()
        {
            List<Country> objCountryList = _CiplatformDbContext.Countries.ToList();
            return objCountryList;
        }
        public List<City> GetCityFromCountry(List<string> countryId)
        {
            List<City> city = _CiplatformDbContext.Cities.Where(i => countryId.Contains(i.CountryId.ToString())).ToList();
            return city;
        }
        public List<MissionTheme> GetMissionTheme()
        {
            List<MissionTheme> missionTitle = _CiplatformDbContext.MissionThemes.ToList();
            return missionTitle;
        }
        public List<Skill> GetSkillsList()
        {
            List<Skill> skillsList = _CiplatformDbContext.Skills.ToList();
            return skillsList;
        }

        //---------------------------Mission card--------------------------------------------------------------------
        public List<Mission> GetMissionList()
        {
            List<Mission> missionDetail = _CiplatformDbContext.Missions.ToList();
            return missionDetail;
        }

        public List<Card> GetMissionList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sortBy, int paging, long user)
        {
            var pageSize = 6;



            List<Card> missions = GetMissionCard(user);
            if (search != "")
            {
                missions = missions.Where(a => a.Title.ToLower().Contains(search) || a.OrganizationName.ToLower().Contains(search)).ToList();

            }
            if (countries.Length > 0)
            {

                missions = missions.Where(a => countries.Contains(a.CountryId.ToString())).ToList();

            }
            if (cities.Length > 0)
            {

                missions = missions.Where(a => cities.Contains(a.CityId.ToString())).ToList();

            }
            if (themes.Length > 0)
            {

                missions = missions.Where(a => themes.Contains(a.ThemeId.ToString())).ToList();

            }
            if (skills.Length > 0)
            {

                missions = missions.Where(a => skills.Contains(a.skillId.ToString())).ToList();

            }

            switch (sortBy)
            {
                case 1:
                    missions = missions.OrderBy(i => i.StartDate).ToList();
                    break;

                case 2:
                    missions = missions.OrderByDescending(i => i.StartDate).ToList();
                    break;

                case 3:
                    missions = missions.OrderBy(i => i.Deadline).ToList();
                    break;

                case 4:
                    missions = missions.OrderBy(i => i.Avaibility).ToList();
                    break;

                case 5:
                    missions = missions.OrderByDescending(i => i.Avaibility).ToList();
                    break;

                case 6:
                    missions = missions.OrderBy(i => i.FavouriteMissionId).ToList();
                    break;
            }
            if (paging != null)
            {
                missions = missions.Skip((paging - 1) * pageSize).Take(6).ToList();
            }
            return missions;
        }

        public string getThemeTitle(long themeID)
        {
            var theme = _CiplatformDbContext.MissionThemes.FirstOrDefault(u => u.MissionThemeId == themeID);
            return theme.Title;
        }
        public string getCity(long CityID)
        {
            var CityName = _CiplatformDbContext.Cities.FirstOrDefault(u => u.CityId == CityID);
            return CityName.Name;
        }
        public string getMediaName(long missionID)
        {
            var imagePath = _CiplatformDbContext.MissionMedia.FirstOrDefault(u => u.MissionId == missionID);
            return imagePath.MediaName;
        }

        public string getGoalObject(long missionID)
        {
            var goaltext = _CiplatformDbContext.GoalMissions.FirstOrDefault(u => u.MissionId == missionID);
            if (goaltext != null)
                return goaltext.GoalObjectiveText;
            return null;
        }

        public int getMissionRating(long missionId)
        {

            var rating = _CiplatformDbContext.MissionRatings.Where(a => a.MissionId == missionId).Average(a => a.Rating);
            return (int)rating;
        }
        public long countVolunteers(long missionId)
        {
            long ratingVolunteers = _CiplatformDbContext.MissionRatings.Where(r => r.MissionId == missionId).Count();
            return ratingVolunteers;
        }
        public List<Card> GetMissionCard(long user_id)
        {
            List<Card> missionAllDetails = new List<Card>();
            var missions = _CiplatformDbContext.Missions.ToList();
            foreach (var allDetailsCard in missions)
            {
                Card cardDetails = new Card();
                var fav = _CiplatformDbContext.FavoriteMissions.FirstOrDefault(u => u.MissionId == allDetailsCard.MissionId && u.UserId == user_id);
                if (fav != null)
                {
                    cardDetails.checkFav = true;
                }
                else
                {
                    cardDetails.checkFav = false;
                }

                cardDetails.Theme = getThemeTitle(allDetailsCard.ThemeId);
                cardDetails.Title = allDetailsCard.Title;
                cardDetails.MissionId = allDetailsCard.MissionId;
                cardDetails.CityName = getCity(allDetailsCard.CityId);
                cardDetails.ShortDescription = allDetailsCard.ShortDescription;
                cardDetails.CityId = allDetailsCard.CityId;
                cardDetails.OrganizationName = allDetailsCard.OrganizationName;
                cardDetails.StartDate = allDetailsCard.StartDate;
                cardDetails.EndDate = allDetailsCard.EndDate;
                cardDetails.MediaName = getMediaName(allDetailsCard.MissionId);
                cardDetails.Rating = getMissionRating(allDetailsCard.MissionId);
                cardDetails.CountryId = allDetailsCard.CountryId;
                cardDetails.ThemeId = allDetailsCard.ThemeId;
                cardDetails.Avaibility = allDetailsCard.Avaibility;
                cardDetails.MissionType = allDetailsCard.MissionType;
                cardDetails.Description = allDetailsCard.Description;
                cardDetails.aboutOrganization = allDetailsCard.OrganizationDetail;
                cardDetails.GoalObjectiveText = getGoalObject(allDetailsCard.MissionId);

                var missionSkill = _CiplatformDbContext.MissionSkills.FirstOrDefault(u => u.MissionId == allDetailsCard.MissionId);
                cardDetails.skillId = missionSkill.MissionSkillId;
                missionAllDetails.Add(cardDetails);
            }

            return missionAllDetails;
        }



        //---------------------------------------Mission Volunteering----------------------------------------------------------------------


        public bool checkFavourite(long missionId, long userId)
        {
            var fav = _CiplatformDbContext.FavoriteMissions.FirstOrDefault(a => a.MissionId == missionId && a.UserId == userId);
            if (fav != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkApplied(long missionId, long userId)
        {
            var apply = _CiplatformDbContext.MissionApplications.FirstOrDefault(a => a.MissionId == missionId && a.UserId == userId);
            if (apply != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool addToFavourite(long missionId, long userId)
        {
            FavoriteMission favourite = new();
            favourite.MissionId = missionId;
            favourite.UserId = userId;

            var FavMission = _CiplatformDbContext.FavoriteMissions.FirstOrDefault(i => i.MissionId == missionId && i.UserId == userId);
            if (FavMission == null)
            {
                _CiplatformDbContext.FavoriteMissions.Add(favourite);
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            else
            {
                _CiplatformDbContext.FavoriteMissions.Remove(FavMission);
                _CiplatformDbContext.SaveChanges();
                return false;
            }
        }
        /*---------------------------------Rating------------------------------------*/


        public int getRating(long missionId, long userId)
        {
            if (_CiplatformDbContext.MissionRatings.FirstOrDefault(r => r.MissionId == missionId && r.UserId == userId) is not null)
            {
                int getRate = _CiplatformDbContext.MissionRatings.FirstOrDefault(r => r.MissionId == missionId && r.UserId == userId).Rating;
                return getRate;
            }
            else
            {
                return 0;
            }
        }
        public bool PostRating(byte rate, long missionId, long userId)
        {
            var entry = _CiplatformDbContext.MissionRatings.Where(m => m.MissionId == missionId && m.UserId == userId);
            if (entry.ToList().Count == 0)
            {
                var data = new MissionRating()
                {
                    UserId = userId,
                    MissionId = missionId,
                    Rating = rate
                };
                _CiplatformDbContext.MissionRatings.Add(data);
                _CiplatformDbContext.SaveChanges();
                return true;
            }
            else
            {
                var data = new MissionRating()
                {
                    Rating = rate,
                };
                entry.First().Rating = rate;
                entry.First().UpdatedAt = DateTime.Now;
                _CiplatformDbContext.SaveChanges();
                return true;
            }
        }

        public List<CommentViewModel> getComment(long missionId)
        {
            List<Comment> comments = _CiplatformDbContext.Comments.Where(c => c.MissionId == missionId && (c.ApprovalStatus == "PUBLISHED" || c.ApprovalStatus == "PENDING")).ToList();

            List<CommentViewModel> commentView = new List<CommentViewModel>();

            foreach (var comment in comments)
            {
                CommentViewModel commentList = new CommentViewModel();
                User user = _CiplatformDbContext.Users.FirstOrDefault(a => a.UserId == comment.UserId);
                commentList.Comment = comment.Comments;
                commentList.Month = comment.CreatedAt.ToString("MMMM");
                commentList.Time = comment.CreatedAt.ToString("h:mm tt");
                commentList.Day = comment.CreatedAt.Day.ToString();
                commentList.WeekDay = comment.CreatedAt.DayOfWeek.ToString();
                commentList.Year = comment.CreatedAt.Year.ToString();
                commentList.Avatar = user.Avatar;
                commentList.UserName = user.FirstName + " " + user.LastName;

                commentView.Add(commentList);
            }

            return commentView;
        }


        public void PostComment(string comment, long userId, long missionId)
        {
            Comment newComment = new Comment();
            newComment.Comments = comment;
            newComment.UserId = userId;
            newComment.MissionId = missionId;

            _CiplatformDbContext.Comments.Add(newComment);
            _CiplatformDbContext.SaveChanges();
        }


        public String GetSkillName(long missionId)
        {
            return string.Join(", ", _CiplatformDbContext.Missions.Where(x => x.MissionId == missionId).SelectMany(x => x.MissionSkills).Select(x => x.Skill.SkillName).ToList());
        }

        public List<CommentViewModel> GetRecentUser(long missionId)
        {
            List<MissionApplication> missionApplication = _CiplatformDbContext.MissionApplications.Where(a => a.MissionId == missionId && a.ApprovalStatus == "APPROVE").ToList();
            List<CommentViewModel> missionView = new List<CommentViewModel>();
            foreach (MissionApplication application in missionApplication)
            {
                CommentViewModel missionviewModel = new CommentViewModel();
                User user = _CiplatformDbContext.Users.FirstOrDefault(a => a.UserId == application.UserId);
                missionviewModel.MissionId = missionId;
                missionviewModel.Avatar = user.Avatar;
                missionviewModel.UserName = user.FirstName + " " + user.LastName;

                missionView.Add(missionviewModel);

            }
            return missionView;
        }


        public void AddToRecent(long missionId, long userId)
        {
            MissionApplication missionapplication = new MissionApplication();
            missionapplication.UserId = userId;
            missionapplication.MissionId = missionId;
            missionapplication.AppliedAt = DateTime.Now;
            missionapplication.ApprovalStatus = "APPROVE";
            _CiplatformDbContext.MissionApplications.Add(missionapplication);
            _CiplatformDbContext.SaveChanges();
        }
        public List<MissionApplication> getMissionApplicant()
        {
            List<MissionApplication> missionapplications = _CiplatformDbContext.MissionApplications.ToList();
            return missionapplications;
        }
        public bool Recommend(long user_id, long mission_id, List<long> co_workers)
        {
            foreach (var user in co_workers)
            {
                _CiplatformDbContext.MissionInvites.Add(new MissionInvite
                {
                    FromUserId = user_id,
                    ToUserId = Convert.ToInt64(user),
                    MissionId = mission_id
                });
            }
            _CiplatformDbContext.SaveChanges();
            User from_user = _CiplatformDbContext.Users.FirstOrDefault(c => c.UserId.Equals(user_id));
            List<string> Email_users = (from u in _CiplatformDbContext.Users
                                        where co_workers.Contains(u.UserId)
                                        select u.Email).ToList();
            foreach (var email in Email_users)
            {
                var senderEmail = new MailAddress("masterdevil075@gmail.com", "CI_PlatForm");
                var receiverEmail = new MailAddress(email, "Receiver");
                var password = "jmqvkwxafyvjjzlo";
                var sub = "Recommendation";
                var body = "Recommend By " + from_user?.FirstName + " " + from_user?.LastName + "\n" + $"https://localhost:7217/Mission/MissionVolunteering/{mission_id}";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
            }
            return true;
        }
        /*----------------------------------------Story Listing----------------------------------------------------------------*/

        public string getStoryImg(long storyId)
        {
            var imagePath = _CiplatformDbContext.StoryMedia.FirstOrDefault(u => u.StoryId == storyId);
            return imagePath.Path;
        }



        public string GetType(long storyId)
        {
            var type = _CiplatformDbContext.StoryMedia.FirstOrDefault(u => u.StoryId == storyId);
            return type.Type;
        }



        public List<StoryViewModel> GetStoryDetails()
        {
            List<Story> PublishedStory = _CiplatformDbContext.Stories.Where(s => s.Status == "PUBLISHED").ToList();


            List<StoryViewModel> stories = new List<StoryViewModel>();

            foreach (var story in PublishedStory)
            {

                StoryViewModel storyInfo = new StoryViewModel();
                User user = _CiplatformDbContext.Users.FirstOrDefault(a => a.UserId == story.UserId);
                storyInfo.Type = GetType(story.StoryId);
                storyInfo.StoryId = story.StoryId;
                storyInfo.Title = story.Title;
                storyInfo.Description = story.Description;
                storyInfo.storyImage = getStoryImg(story.StoryId);
                storyInfo.Avatar = user.Avatar;
                storyInfo.UserName = user.FirstName + " " + user.LastName;
                stories.Add(storyInfo);
            }

            return stories;
        }

        public List<StoryViewModel> GetStoryData(string? searchStory)
        {
            List<Story> story = new List<Story>();
            List<StoryViewModel> stories = GetStoryDetails();
            if (searchStory != "")
            {
                stories = stories.Where(a => a.Title.ToLower().Contains(searchStory)).ToList();
            }
            return stories;
        }


        public List<Story> GetStoryData()
        {
            List<Story> storyDetail = _CiplatformDbContext.Stories.ToList();
            return storyDetail;
        }
        public List<StoryViewModel> GetStoryList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int paging)
        {
            var pageSize = 6;

            List<StoryViewModel> stories = GetStoryDetails();
            if (search != "")
            {
                stories = stories.Where(a => a.Title.ToLower().Contains(search)).ToList();
            }
            if (countries.Length > 0)
            {
                stories = stories.Where(a => countries.Contains(a.CountryId.ToString())).ToList();
            }
            if (cities.Length > 0)
            {
                stories = stories.Where(a => cities.Contains(a.CityId.ToString())).ToList();
            }
            if (themes.Length > 0)
            {
                stories = stories.Where(a => themes.Contains(a.ThemeId.ToString())).ToList();
            }
            if (skills.Length > 0)
            {
                stories = stories.Where(a => skills.Contains(a.SkillId.ToString())).ToList();
            }
            if (paging != null)
            {
                stories = stories.Skip((paging - 1) * pageSize).Take(6).ToList();
            }
            return stories;
        }
        public List<Mission> getStoryMission(long userid)
        {
            var storymission = _CiplatformDbContext.MissionApplications.Where(i => i.UserId == userid && i.ApprovalStatus == "APPROVE").ToList();
            var story = new List<long>();
            foreach (var item in storymission)
            {
                story.Add(item.MissionId);
            }
            var storymissions = _CiplatformDbContext.Missions.Where(i => story.Contains(i.MissionId)).ToList();
            return storymissions;
        }
        public void AddStory(StoryViewModel model, long UserId, string submit)
        {
            if (model.Description != null)
            {
                Story addData = new Story();
                StoryMedium Url = new StoryMedium();
                {
                    addData.UserId = UserId;
                    addData.MissionId = model.MissionId;
                    addData.Description = model.Description;
                    addData.Title = model.Title;
                    addData.Status = "DRAFT";
                    if (submit == "Submit")
                    {
                        addData.Status = "PUBLISHED";
                    }
                    Url.Path = model.Url;
                    addData.PublishedAt = model.PublishDate;
                    _CiplatformDbContext.Add(addData);
                    _CiplatformDbContext.SaveChanges();
                    foreach (var i in model.Images)
                    {
                        StoryMedium storyMedium = new StoryMedium();
                        storyMedium.StoryId = addData.StoryId;
                        storyMedium.Type = "png";
                        storyMedium.Path = i.FileName;
                        _CiplatformDbContext.StoryMedia.Add(storyMedium);
                        _CiplatformDbContext.SaveChanges();
                        if (i.Length > 0)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/CI Platform Assets/StoryImages", i.FileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                i.CopyTo(stream);
                            }
                        }
                    }
                }
            }
        }

        public StoryViewModel getStory(long story_id, long user_id)
        {
            List<StoryMedium> medias = _CiplatformDbContext.StoryMedia.ToList();
            /*List<StoryView> views = _CiplatformDbContext.StoryViews.ToList();
            if (_CiplatformDbContext.StoryViews.FirstOrDefault(c => c.StoryId == story_id && c.UserId == user_id) is null)
            {
                _CiplatformDbContext.StoryViews.Add(new StoryView
                {
                    UserId = user_id,
                    StoryId = story_id,
                });
                _CiplatformDbContext.SaveChanges();
            }*/

            Story story = _CiplatformDbContext.Stories.FirstOrDefault(s => s.StoryId == story_id);
            var whyIVolunteer = _CiplatformDbContext.Users.FirstOrDefault(u => u.UserId == story.UserId).WhyIVolunteer;
            User user = _CiplatformDbContext.Users.FirstOrDefault(s => s.UserId == story.UserId);


            if (story == null)
            {
                return null;
            }
            else
            {
                StoryViewModel mystory = new StoryViewModel()
                {
                    UserName = user.FirstName + " " + " " + user.LastName,
                    Avatar = user.Avatar,
                    Title = story.Title,
                    Description = story.Description,
                    storymedia = story.StoryMedia.ToList(),
                    users = _CiplatformDbContext.Users.ToList(),
                    StoryId = story_id,
                    MissionId = story.MissionId,
                    WhyIVolunteer = whyIVolunteer
                };
                return mystory;
            }
        }

    }

}
