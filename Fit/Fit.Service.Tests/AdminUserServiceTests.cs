﻿using Fit.Common;
using Fit.DTO.RBAC;
using Fit.IService;
using Fit.Service.Entities.RBAC;
using Fit.Service.Repository;
using Fit.Service.Services.RBAC;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit.Service.Tests
{
  [TestFixture]
  public class AdminUserServiceTests:IAdminUserService
  {
    public long AddAdminUser(string name, string phoneNum, string email, string password)
    {
      throw new NotImplementedException();
    }
    [Test]
    public void AddAdminUser_NormalValue_ReturnId()
    {
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.Add(Arg.Any<AdminUserEntity>()).Returns(2);
      var adminUser = new AdminUserService(repository);

      var id = adminUser.AddAdminUser("Test1", "123123123", "123@123", "123");

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

      Assert.Throws<ArgumentException>(() =>
      adminUser.AddAdminUser("Test1", "123123123", email, "123"));
    }


    public long? CheckLogin(string email, string password)
    {
      throw new NotImplementedException();
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

      var id = service.CheckLogin(inputedEmail, inputedPassword);
      Assert.IsTrue(id.HasValue);
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

      var id = service.CheckLogin("123", inputedPassword);
      Assert.IsFalse(id.HasValue);
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

      var id = service.CheckLogin(inputedEmail, "123");
      Assert.IsFalse(id.HasValue);
    }
    [Test]
    public void CheckLogin_Deleted_ReturnFalse()
    {
      string inputedPassword = "123456", inputedEmail = "123@123.com";
      var fakeEntity = new AdminUserEntity
      {
        Name = "111",
        Email = inputedEmail,
        PasswordSalt = "1234",
        IsDeleted = true
      };
      fakeEntity.PasswordHash = CommonHelper.CalcMD5(fakeEntity.PasswordSalt + inputedPassword);
      var data = new List<AdminUserEntity> { fakeEntity }.AsQueryable();

      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetAll().Returns(data);
      var service = new AdminUserService(repository);

      var id = service.CheckLogin(inputedEmail, inputedEmail);
      Assert.IsFalse(id.HasValue);
    }


    public AdminUserDTO[] GetAll()
    {
      throw new NotImplementedException();
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


    public AdminUserDTO GetByEmail(string email)
    {
      throw new NotImplementedException();
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


    public AdminUserDTO GetById(long id)
    {
      throw new NotImplementedException();
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
    public void GetById_IdNotExist_Throw()
    {
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      var service = new AdminUserService(repository);

      Assert.Throws<ArgumentException>(() => service.GetById(1));
    }


    public void MarkDeleted(long id)
    {
      throw new NotImplementedException();
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


    public void RecordLoginError(long id)
    {
      throw new NotImplementedException();
    }
    [Test]
    public void RecordLoginError_IdExist_ErrorTimesEqualsOne()
    {
      int entityId = 3;
      string inputedEmail = "123@123.com";
      var fakeEntity = new AdminUserEntity
      {
        ID = entityId,
        Name = "111",
        Email = inputedEmail
      };
      var data = new List<AdminUserEntity> { fakeEntity }.AsQueryable();
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetById(entityId).Returns(fakeEntity);
      var service = new AdminUserService(repository);
      var dtNow = DateTimeHelper.GetNow();
      service.RecordLoginError(entityId);

      Assert.AreEqual(1, fakeEntity.LoginErrorTimes, "LoginErrorTimes=1");
      Assert.IsTrue(dtNow <= fakeEntity.LastLoginErrorDateTime, "LastLoginErrorDateTime should be record");
    }
    [Test]
    public void RecordLoginError_IdNotExist_Throw()
    {
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      var service = new AdminUserService(repository);

      Assert.Throws<ArgumentException>(() => service.RecordLoginError(1));
    }


    public void ResetLoginError(long id)
    {
      throw new NotImplementedException();
    }
    [Test]
    public void ResetLoginError_IdExist_ErrorTimesEqualsZero()
    {
      int entityId = 3;
      string inputedEmail = "123@123.com";
      var fakeEntity = new AdminUserEntity
      {
        ID = entityId,
        Name = "111",
        Email = inputedEmail,
        LoginErrorTimes = 2
      };
      var data = new List<AdminUserEntity> { fakeEntity }.AsQueryable();
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetById(entityId).Returns(fakeEntity);
      var service = new AdminUserService(repository);
      var dtNow = DateTimeHelper.GetNow();
      service.ResetLoginError(entityId);

      Assert.AreEqual(0, fakeEntity.LoginErrorTimes, "LoginErrorTimes=0");
    }
    [Test]
    public void ResetLoginError_IdNotExist_Throw()
    {
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      var service = new AdminUserService(repository);

      Assert.Throws<ArgumentException>(() => service.ResetLoginError(1));
    }


    public void UpdateAdminUser(long id, string name, string password)
    {
      throw new NotImplementedException();
    }
    [Test]
    public void UpdateAdminUser_IdExist_SucceedLoginAfterUpdated()
    {
      var entityId = 1;
      var fakeEntity = new AdminUserEntity
      {
        ID = entityId,
        Email = "TestEmail",
        Name = string.Empty,
        PasswordSalt = "1234"
      };
      fakeEntity.PasswordHash = CommonHelper.CalcMD5(fakeEntity.PasswordSalt + "pwd");
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetById(entityId).Returns(fakeEntity);
      repository.GetAll().Returns(new List<AdminUserEntity>() { fakeEntity }.AsQueryable());
      var service = new AdminUserService(repository);

      service.UpdateAdminUser(entityId, "Updated", "pwd2");
      var id = service.CheckLogin("TestEmail", "pwd2");

      Assert.IsTrue(id.HasValue, "Try Login");
      Assert.AreEqual("Updated", fakeEntity.Name, "name=Updated");
    }
    [Test]
    public void UpdateAdminUser_IdNotExist_Throw()
    {
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      var service = new AdminUserService(repository);

      Assert.Throws<ArgumentException>(() => service.UpdateAdminUser(1, "Updated", "pwd2"));
    }


    public void Update(AdminUserDTO dto)
    {
      throw new NotImplementedException();
    }
    [Test]
    public void Update_IdExist_NameUpdated()
    {
      var dto = new AdminUserDTO
      {
        ID = 1,
        Name = "Updated",
        Email = "email"
      };
      var fakeEntity = new AdminUserEntity
      {
        ID = 1,
        Name = "Updated",
        Email = "email"
      };
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetById(Arg.Any<long>()).Returns(fakeEntity);
      var service = new AdminUserService(repository);
      service.Update(dto);

      repository.Received().Update(fakeEntity);
    }
    [Test]
    public void Update_WillUpdatePwd_PwdUpdated()
    {
      var dto = new AdminUserDTO
      {
        ID = 1,
        Name = "Updated",
        Email = "email",
        WillUpdatePwd = true,
        Password = "123"
      };
      var fakeEntity = new AdminUserEntity
      {
        ID = 1,
        Name = string.Empty,
        Email = "email"
      };
      fakeEntity.PasswordHash = CommonHelper.CalcMD5(fakeEntity.PasswordSalt + dto.Password);
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetById(Arg.Any<long>()).Returns(fakeEntity);
      var service = new AdminUserService(repository);
      service.Update(dto);
      var updatedEntity = service.GetById(1);
      var expectedPwdHash = CommonHelper.CalcMD5(dto.Password);
      Assert.AreEqual(expectedPwdHash, fakeEntity.PasswordHash);
    }
    [Test]
    public void Update_WillNotUpdatePwd_PwdnotUpdated()
    {
      var dto = new AdminUserDTO
      {
        ID = 1,
        Name = "Updated",
        WillUpdatePwd = false,
        Password = "123"
      };
      var fakeEntity = new AdminUserEntity
      {
        ID = 1,
        Name = string.Empty,
        PasswordHash = "abcd"
      };
      var repository = Substitute.For<IRepository<AdminUserEntity>>();
      repository.GetById(Arg.Any<long>()).Returns(fakeEntity);
      var service = new AdminUserService(repository);
      service.Update(dto);
      var updatedEntity = service.GetById(1);
      var expectedPwdHash = CommonHelper.CalcMD5(dto.Password);
      Assert.AreNotEqual(expectedPwdHash, fakeEntity.PasswordHash);
    }
    
    public AdminUserDTO[] GetPagedData(int startIndex, int pageSize)
    {
      throw new NotImplementedException();
    }
    [Test]
    [Ignore("Not tested")]
    public void GetPagedData() { }

    public long GetTotalCount()
    {
      throw new NotImplementedException();
    }
    [Test]
    [Ignore("Not tested")]
    public void GetTotalCount_Test() { }

    public bool CheckPermission(long adminId, string permissionName)
    {
      throw new NotImplementedException();
    }
    [Test]
    [Ignore("Not tested")]
    public void CheckPermission() { }
  }
}
