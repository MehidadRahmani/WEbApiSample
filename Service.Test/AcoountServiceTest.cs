using Application;
using Application.Controllers;
using Common;
using Common.Exceptions;
using Data.Contract.Owin;
using Entities.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Moq;
using Services.Contract.Account;
using Services.DTO.Account;
using Services.Services.Account;
using System;
using Xunit;

namespace Service.Test
{
    public class AcoountServiceTest  
      
    {
       
        [Fact]
        public async void Login_Failed_When_UserName_Is_InCorrect()
        {
            //Arrenge
        var mockRepo = new Mock<IAccountService>();
            mockRepo.Setup(x => x.CheckUserAndPassword("mehrdad", "123", false));
            var controller = new AccountController(mockRepo.Object);
            //Act
          var result=await controller.Login("mehrdad", "123", false);

            // Assert
            Assert.Equal("نام کاربری یا رمز عبور اشتباه است",result);
          
        }



        [Fact]
        public async void Register_Sucsses_When_UserDtO_Is_InCorrect()
        {
            //Arrenge
            var mockRepo = new Mock<IAccountService>();
            User user = new User()
            {
                UserName = "09121480884",
                Age = 30,
                Email = "mehidad@gmail.com",
                FullName = "مهرداد رحمانی",
                Gender = GenderType.Male,

            };
            UserDTO dto = new UserDTO()
            {
                UserName = "09121480884",
                 Password = "a@777007",
                Age = 30,
                Email = "mehidad@gmail.com",
                FullName = "مهرداد رحمانی",
                Gender = GenderType.Male,

            };
            mockRepo.Setup(x => x.Register(user,"777007"));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Create(dto);

            // Assert
            Assert.True( result.IsSuccess);
           

        }
    }
}
