﻿using Fit.Common;
using Fit.Service.Entities.RBAC;
using Fit.Service.Repository;
using Fit.Service.Services.RBAC;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit.Service.Tests
{
  [TestFixture]
  public class AdminUserServiceTests
  {
    [Test]
    public void AddAdminUser_NormalValue_ReturnId()
    {
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.Add(Arg.Any<AdminUserEntity>()).Returns(2);
      var adminUser = new AdminUserService(repository);

      var id = adminUser.AddAdminUser("Test1", "123@123", "123");

      Assert.AreEqual(2, id);
    }
    [Test]
    public void AddAdminUser_EmailExist_ThrowException()
    {
      var email = "123@123.com";
      var data = new List<AdminUserEntity>
      {
        new AdminUserEntity{ Email=email}
      }.AsQueryable();
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetAll().Returns(data);
      var adminUser = new AdminUserService(repository);

      Assert.Throws<ArgumentException>(() => adminUser.AddAdminUser("Test1", email, "123"));
    }
    [Test]
    public void CheckLogin_CorrectValue_ReturnTrue()
    {
      string inputedPassword = "123456", inputedEmail = "123@123.com";
      var fakeEntity = new AdminUserEntity
      {
        Name = "111",
        Email = inputedEmail,
        PasswordSalt = "1234"
      };
      fakeEntity.PasswordHash = CommonHelper.CalcMD5(fakeEntity.PasswordSalt + inputedPassword);
      var data = new List<AdminUserEntity> { fakeEntity }.AsQueryable();

      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetAll().Returns(data);
      var service = new AdminUserService(repository);

      var result = service.CheckLogin(inputedEmail, inputedPassword);
      Assert.IsTrue(result);
    }
    [Test]
    public void CheckLogin_NoEmail_ReturnFalse()
    {
      string inputedPassword = "123456", inputedEmail = "123@123.com";
      var fakeEntity = new AdminUserEntity
      {
        Name = "111",
        PhoneNum = inputedEmail,
        PasswordSalt = "1234"
      };
      fakeEntity.PasswordHash = CommonHelper.CalcMD5(fakeEntity.PasswordSalt + inputedPassword);
      var data = new List<AdminUserEntity> { fakeEntity }.AsQueryable();

      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetAll().Returns(data);
      var service = new AdminUserService(repository);

      var result = service.CheckLogin("123", inputedPassword);
      Assert.IsFalse(result);
    }
    [Test]
    public void CheckLogin_WrongPwd_ReturnFalse()
    {
      string inputedPassword = "123456", inputedEmail = "123@123.com";
      var fakeEntity = new AdminUserEntity
      {
        Name = "111",
        PhoneNum = inputedEmail,
        PasswordSalt = "1234"
      };
      fakeEntity.PasswordHash = CommonHelper.CalcMD5(fakeEntity.PasswordSalt + inputedPassword);
      var data = new List<AdminUserEntity> { fakeEntity }.AsQueryable();

      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetAll().Returns(data);
      var service = new AdminUserService(repository);

      var result = service.CheckLogin(inputedEmail, "123");
      Assert.IsFalse(result);
    }
    [Test]
    public void CheckLogin_Deleted_ReturnFalse()
    {
      string inputedPassword = "123456", inputedEmail = "123@123.com";
      var fakeEntity = new AdminUserEntity
      {
        Name = "111",
        PhoneNum = inputedEmail,
        PasswordSalt = "1234",
        IsDeleted = true
      };
      fakeEntity.PasswordHash = CommonHelper.CalcMD5(fakeEntity.PasswordSalt + inputedPassword);
      var data = new List<AdminUserEntity> { fakeEntity }.AsQueryable();

      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetAll().Returns(data);
      var service = new AdminUserService(repository);

      var result = service.CheckLogin(inputedEmail, inputedEmail);
      Assert.IsFalse(result);
    }
    [Test]
    public void GetAll_ReturnArray()
    {
      string name = "123", email = "123@123.com";
      int loginErrorTime = 3;
      DateTime lastLoginErrorDateTime = new DateTime(2017, 5, 5);
      var data = new List<AdminUserEntity>
      {
        new AdminUserEntity{ Name=name, Email=email,LoginErrorTimes=loginErrorTime,LastLoginErrorDateTime=lastLoginErrorDateTime}
      }.AsQueryable();
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetAll().Returns(data);
      var adminUser = new AdminUserService(repository);

      var array = adminUser.GetAll();

      Assert.IsTrue(array.Length == 1, "array.Length=1");
      var entity = array[0];
      Assert.AreEqual(name, entity.Name, "name");
      Assert.AreEqual(email, entity.Email, "Email");
      Assert.AreEqual(loginErrorTime, entity.LoginErrorTimes, "LoginErrorTimes");
      Assert.AreEqual(lastLoginErrorDateTime, entity.LastLoginErrorDateTime, "LastLoginErrorDateTime");
    }
    [Test]
    public void GetByEmail_EmailExist_ReturnDTO()
    {
      string name = "123", email = "123@123.com";
      int loginErrorTime = 3;
      DateTime lastLoginErrorDateTime = new DateTime(2017, 5, 5);
      var data = new List<AdminUserEntity>
      {
        new AdminUserEntity{ Name=name, Email=email,LoginErrorTimes=loginErrorTime,LastLoginErrorDateTime=lastLoginErrorDateTime}
      }.AsQueryable();
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetAll().Returns(data);
      var adminUser = new AdminUserService(repository);

      var dto = adminUser.GetByEmail(email);
      Assert.AreEqual(email, dto.Email);
    }
    [Test]
    public void GetByEmail_EmailNotExist_Throw()
    {
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      var adminUser = new AdminUserService(repository);

      Assert.Throws<ArgumentException>(() => adminUser.GetByEmail("123"));
    }
    [Test]
    public void GetById_IdExist_ReturnEntity()
    {
      int entityId = 2;
      var fakeEntity = new AdminUserEntity
      {
        ID = entityId
      };

      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetById(entityId).Returns(fakeEntity);
      var service = new AdminUserService(repository);

      var result = service.GetById(entityId);
      Assert.AreEqual(entityId, result.ID);
    }
    [Test]
    public void MarkDeleted_Deleted()
    {
      int entityId = 3;
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      var service = new AdminUserService(repository);
      service.MarkDeleted(entityId);

      repository.Received().DeleteById(entityId);
    }

  }
}
