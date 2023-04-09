using CI_PlatForm.Entities.ViewModel;
using CI_PlatForm.Repository.Interface;
using CI_PlatForm.Entities.Data;
using CI_PlatForm.Entities.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace CI_PlatForm.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly CiplatformContext _CiplatformDbContext;
    

        public UserRepository(CiplatformContext ciplatformDbContext)
        {
            _CiplatformDbContext = ciplatformDbContext;
         
        }
        public List<User> UserList()
        {
            List<User> objUserList = _CiplatformDbContext.Users.ToList();
            return objUserList;
        }
        public List<Country> CountryList()
        {
            List<Country> objCountryList = _CiplatformDbContext.Countries.ToList();
            return objCountryList;
        }
        public List<City> CityList()
        {
            List<City> objCityList = _CiplatformDbContext.Cities.ToList();
            return objCityList;
        }
        public List<MissionTheme> MissionThemeList()
        {
            List<MissionTheme> objMissionThemeList = _CiplatformDbContext.MissionThemes.ToList();
            return objMissionThemeList;
        }
        public List<Skill> SkillList()
        {
            List<Skill> objSkillList = _CiplatformDbContext.Skills.ToList();
            return objSkillList;
        }
       
        public Boolean IsEmailAvailable(string email)
        {
            return _CiplatformDbContext.Users.Any(x => x.Email == email);
        }

        public User IsPasswordAvailable(string password, string email)
        {
            return _CiplatformDbContext.Users.Where(x => x.Password == password && x.Email == email).FirstOrDefault();
        }



        public long GetUserID(string Email)
        {
            User user = _CiplatformDbContext.Users.Where(x => x.Email == Email).FirstOrDefault();
            if (user == null)
            {

                return -1;
            }
            else
            {

                return user.UserId;
            }
        }
        public void Registration(User objUser)
        
            
            {
                _CiplatformDbContext.Users.Add(objUser);
                _CiplatformDbContext.SaveChanges();
            }
        

        public bool ResetPassword(long userId, string OldPassword, string NewPassword)
        {
            try
            {
                User user = _CiplatformDbContext.Users.Find(userId);
                string pass = (user.Password);
                if (pass == OldPassword)
                {
                    user.Password = (NewPassword);
                    _CiplatformDbContext.Users.Update(user);
                    _CiplatformDbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public Boolean ChangePassword(long UserId, Reset_Password model)
        {
            User user = _CiplatformDbContext.Users.FirstOrDefault(x => x.UserId == model.UserId);
            user.Password = model.Password;
            user.UpdatedAt = DateTime.Now;
            _CiplatformDbContext.Users.Update(user);
            _CiplatformDbContext.SaveChanges();
            return true;

        }
        public User GetUser(int userID)
        {
            try
            {

                User user = _CiplatformDbContext.Users.Where(x => x.UserId == userID).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                else
                {

                    return null;

                }


            }
            catch (Exception ex)
            {

                return null;
            }
        }
        



    }



}
