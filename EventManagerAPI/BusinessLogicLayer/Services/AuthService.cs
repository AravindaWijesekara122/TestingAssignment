using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IServices;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public LoginOutputDTO OrganizerLogin(LoginDTO loginDTO)
        {
            try
            {
                var organizer = _dbContext.Organizers
                .FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);

                if (organizer == null)
                {
                    throw new Exception("Organizer Not Found");
                }

                var newAccessToken = CreateJwt(loginDTO);
                var loginOutput = new LoginOutputDTO()
                {
                    ID = organizer.OrganizerID,
                    Message = "Login Successfull..!",
                    Token = newAccessToken
                };
                //return newAccessToken;
                return (loginOutput);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoginOutputDTO AttendeeLogin(LoginDTO loginDTO)
        {
            try
            {
                var attendee = _dbContext.Attendees
                .FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);

                if (attendee == null)
                {
                    throw new Exception("Organizer Not Found");
                }

                var newAccessToken = CreateJwt(loginDTO);
                var loginOutput = new LoginOutputDTO()
                {
                    ID = attendee.AttendeeID,
                    Message = "Login Successfull..!",
                    Token = newAccessToken
                };

                return (loginOutput);
            }           
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private string CreateJwt(LoginDTO loginDTO)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysceret.....adhashdkdjkjsdfgsdjhcvsdbjgcsgsdchvcbg");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, loginDTO.Role),
                new Claim(ClaimTypes.Name,$"{loginDTO.Email}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
