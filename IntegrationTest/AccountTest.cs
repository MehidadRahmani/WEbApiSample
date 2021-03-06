using Application.Controllers;
using System;
using Xunit;
using Moq;
using Services.Contract.Account;
using Entities.Owin;
using Services.DTO.Account;
using Common;
using Common.Exceptions;
using System.Threading;

namespace IntegrationTest
{
    public class AccountTest
    {  
        private Mock<IAccountService> mockRepo { get; set; }
     
        public AccountTest()
        {
          mockRepo = new Mock<IAccountService>();
        }
        #region [-Login-]
        [Fact]
        [Trait("Category", "Login")]
        public async void Login_Failed_When_UserName_Is_InCorrect()
        {
            //Arrenge

            mockRepo.Setup(x => x.CheckUserAndPassword("mehrdad", "123", false));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Login("mehrdad", "123", false);

            // Assert
            Assert.Equal("نام کاربری یا رمز عبور اشتباه است", result);

        }


        [Fact]
        [Trait("Category", "Login")]
        public async void Login_Failed_When_UserName_IsCorrect_But_Password_Is_InCorrect()
        {
            //Arrenge

            mockRepo.Setup(x => x.CheckUserAndPassword("09121450554", "777007", false));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Login("09121450554", "777007", false);

            // Assert
            Assert.Equal("نام کاربری یا رمز عبور اشتباه است", result.Message);
            Assert.Equal(ApiResultStatusCode.BadRequest, result.StatusCode);
       
        }


        [Fact]
        [Trait("Category", "Login")]
        public async void Login_Succsses_When_UserName_And_Password_Is_Correct()
        {
            //Arrenge

            mockRepo.Setup(x => x.CheckUserAndPassword("09121450554", "777007a", false));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Login("09121450554", "777007a", false);

            // Assert
            Assert.True( result.IsSuccess);
            Assert.Equal(ApiResultStatusCode.Success, result.StatusCode);

        }



        #endregion

        #region [-Register-]

    
        [Fact]
        [Trait("Category", "Register")]

        public async void Register_Sucsses_When_UserDtO_Is_Correct()
        {
            //Arrenge
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
            mockRepo.Setup(x => x.Register(user, "777007"));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Create(dto);

            // Assert
            Assert.True(result.IsSuccess);


        }

        [Fact]
        [Trait("Category", "Register")]

        public async void Register_Faild_When_User_Is_Exists()
        {
            //Arrenge
            User user = new User()
            {
                UserName = "09151191052",
                Age = 30,
                Email = "mehidad@gmail.com",
                FullName = "مهرداد رحمانی",
                Gender = GenderType.Male,

            };
            UserDTO dto = new UserDTO()
            {
                UserName = "09151191052",
                Password = "a@777007",
                Age = 30,
                Email = "mehidad@gmail.com",
                FullName = "مهرداد رحمانی",
                Gender = GenderType.Male,

            };
            mockRepo.Setup(x => x.Register(user, "777007"));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Create(dto);

            // Assert
            Assert.Equal("نام کاربری تکراری است", result.Message);
            Assert.Throws<BadRequestException>(() => result.StatusCode);


        }
        #endregion


        #region [-UserManager-]
        [Fact]
        [Trait("Category", "UserManager")]

        public async void UpdateUser_Faild_When_User_Is_Not_Exists()
        {
            //Arrenge
            User user = new User()
            {
                Id=-1,
                UserName = "09151191052",
                Age = 30,
                Email = "mehidad@gmail.com",
                FullName = "مهرداد رحمانی",
                Gender = GenderType.Male,

            };
            
            CancellationTokenSource cts = new CancellationTokenSource();

            mockRepo.Setup(x => x.UpdateUser(user,cts.Token));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Update(user.Id,user,cts.Token);

            // Assert
            Assert.Equal(ApiResultStatusCode.NotFound, result.StatusCode);


        }
        [Fact]
        [Trait("Category", "UserManager")]

        public async void UpdateUser_Succsses_When_User_Is_Exists()
        {
            //Arrenge
            User user = new User()
            {
                Id =1,
                UserName = "09151191052",
                Age = 30,
                Email = "mehidad@gmail.com",
                FullName = "مهرداد رحمانی",
                Gender = GenderType.Male,

            };

            CancellationTokenSource cts = new CancellationTokenSource();

            mockRepo.Setup(x => x.UpdateUser(user, cts.Token));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Update(user.Id, user, cts.Token);

            // Assert
            Assert.Equal(ApiResultStatusCode.Success, result.StatusCode);


        }

        [Fact]
        [Trait("Category", "UserManager")]

        public async void DeleteUser_Faild_When_User_Is_Not_Exists()
        {
            //Arrenge
            User user = new User()
            {
                Id = -1,
                UserName = "09151191052",
                Age = 30,
                Email = "mehidad@gmail.com",
                FullName = "مهرداد رحمانی",
                Gender = GenderType.Male,

            };

            CancellationTokenSource cts = new CancellationTokenSource();

            mockRepo.Setup(x => x.DeleteUser(user, cts.Token));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Delete( -1, cts.Token);

            // Assert
            Assert.Equal(ApiResultStatusCode.NotFound, result.StatusCode);


        }
        [Fact]
        [Trait("Category", "UserManager")]

        public async void DeleteUser_Succsses_When_User_Is_Exists()
        {
            //Arrenge
            User user = new User()
            {
                Id = 1,
                UserName = "09151191052",
                Age = 30,
                Email = "mehidad@gmail.com",
                FullName = "مهرداد رحمانی",
                Gender = GenderType.Male,

            };

            CancellationTokenSource cts = new CancellationTokenSource();

            mockRepo.Setup(x => x.DeleteUser(user, cts.Token));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Delete(user.Id, cts.Token);

            // Assert
            Assert.Equal(ApiResultStatusCode.Success, result.StatusCode);


        }

        [Fact]
        [Trait("Category", "UserManager")]

        public async void GetUser_Succsses_When_User_Is_Exists()
        {
            //Arrenge
          

            CancellationTokenSource cts = new CancellationTokenSource();

            mockRepo.Setup(x => x.GetUser(1, cts.Token));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Get(1, cts.Token);

            // Assert
            Assert.Equal(ApiResultStatusCode.Success, result.StatusCode);


        }
        [Fact]
        [Trait("Category", "UserManager")]

        public async void GetUser_Faild_When_User_Is_Not_Exists()
        {
            //Arrenge


            CancellationTokenSource cts = new CancellationTokenSource();

            mockRepo.Setup(x => x.GetUser(-1, cts.Token));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Get(-1, cts.Token);

            // Assert
            Assert.Equal(ApiResultStatusCode.NotFound, result.StatusCode);


        }
        [Fact]
        [Trait("Category", "UserManager")]

        public async void GetUsers_Succsses_When_Send_Request()
        {
            //Arrenge


            CancellationTokenSource cts = new CancellationTokenSource();

            mockRepo.Setup(x => x.GetUsers( cts.Token));
            var controller = new AccountController(mockRepo.Object);
            //Act
            var result = await controller.Get( cts.Token);

            // Assert
            Assert.IsType<User>( result);


        }
        #endregion

    }
}
