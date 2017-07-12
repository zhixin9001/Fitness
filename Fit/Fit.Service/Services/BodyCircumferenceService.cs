﻿using Fit.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fit.DTO;
using Fit.Service.Repository;
using Fit.Service.Entities;
using Fit.Common;

namespace Fit.Service.Services
{
  public class BodyCircumferenceService : IBodyCircumferenceService
  {
    IRepository<BodyCircumferenceEntity> circumRep;

    public BodyCircumferenceService(IRepository<BodyCircumferenceEntity> bodyRep)
    {
      this.circumRep = bodyRep;
    }

    public long AddOrUpdate(BodyCircumferenceDTO dto)
    {
      if (dto.ID <= 0)   //add
      {
        var entity = new BodyCircumferenceEntity
        {
          UserID = dto.UserID,
          UpperArm = dto.UpperArm,
          LowerArm = dto.LowerArm,
          Chest = dto.Chest,
          Waist = dto.Waist,
          Hip = dto.Hip,
          UpperLeg = dto.UpperLeg,
          LowerLeg = dto.LowerLeg,
          Weight = dto.Weight
        };
        return circumRep.Add(entity);
      }
      else  //edit
      {
        var entity = circumRep.GetById(dto.ID);
        entity.UserID = dto.UserID;
        entity.UpperArm = dto.UpperArm;
        entity.LowerArm = dto.LowerArm;
        entity.Chest = dto.Chest;
        entity.Waist = dto.Waist;
        entity.Hip = dto.Hip;
        entity.UpperLeg = dto.UpperLeg;
        entity.LowerLeg = dto.LowerLeg;
        entity.Weight = dto.Weight;
        circumRep.Ctx.SaveChanges();
        return entity.ID;
      }
    }

    public BodyCircumferenceDTO GetByID(long id)
    {
      return ToDTO(circumRep.GetById(id));
    }

    public BodyCircumferenceDTO[] GetByUserID(long userID)
    {
      return circumRep.GetAll().Where(a => a.UserID == userID).ToList().Select(a => ToDTO(a)).ToArray();
    }

    public BodyCircumferenceDTO GetCurrentByUserID(long userID)
    {
      var today = DateTimeHelper.GetToday();
      var entities = circumRep.GetAll().Where(a => a.UserID == userID).ToList();
      var entity = entities.Where(a => IsToday(a.CreatedDateTime)).FirstOrDefault();
      if (entity == null)
      {
        return null;
      }
      else
      {
        return ToDTO(entity);
      }
    }

    public ChartDataDTO[] GetChestHistory(long userID)
    {
      var entities = circumRep.GetAll().Where(a => a.UserID == userID);
      if (entities == null || entities.Count() <= 0) return null;

      var list = new List<ChartDataDTO>();
      foreach (var item in entities)
      {
        var dto = new ChartDataDTO
        {
          Ticks = item.CreatedDateTime.Ticks,
          Value = item.Chest
        };
        list.Add(dto);
      }
      return list.ToArray();
    }

    public ChartDataDTO[] GetHipHistory(long userID)
    {
      var entities = circumRep.GetAll().Where(a => a.UserID == userID);
      if (entities == null || entities.Count() <= 0) return null;

      var list = new List<ChartDataDTO>();
      foreach (var item in entities)
      {
        var dto = new ChartDataDTO
        {
          Ticks = item.CreatedDateTime.Ticks,
          Value = item.Hip
        };
        list.Add(dto);
      }
      return list.ToArray();
    }

    public ChartDataDTO[] GetLowerArmHistory(long userID)
    {
      var entities = circumRep.GetAll().Where(a => a.UserID == userID);
      if (entities == null || entities.Count() <= 0) return null;

      var list = new List<ChartDataDTO>();
      foreach (var item in entities)
      {
        var dto = new ChartDataDTO
        {
          Ticks = item.CreatedDateTime.Ticks,
          Value = item.LowerArm
        };
        list.Add(dto);
      }
      return list.ToArray();
    }

    public ChartDataDTO[] GetLowerLegHistory(long userID)
    {
      var entities = circumRep.GetAll().Where(a => a.UserID == userID);
      if (entities == null || entities.Count() <= 0) return null;

      var list = new List<ChartDataDTO>();
      foreach (var item in entities)
      {
        var dto = new ChartDataDTO
        {
          Ticks = item.CreatedDateTime.Ticks,
          Value = item.LowerLeg
        };
        list.Add(dto);
      }
      return list.ToArray();
    }

    public ChartDataDTO[] GetUpperArmHistory(long userID)
    {
      var entities = circumRep.GetAll().Where(a => a.UserID == userID);
      if (entities == null || entities.Count() <= 0) return null;

      var list = new List<ChartDataDTO>();
      foreach (var item in entities)
      {
        var dto = new ChartDataDTO
        {
          Ticks = item.CreatedDateTime.Ticks,
          Value = item.UpperArm
        };
        list.Add(dto);
      }
      return list.ToArray();
    }

    public ChartDataDTO[] GetUpperLegHistory(long userID)
    {
      var entities = circumRep.GetAll().Where(a => a.UserID == userID);
      if (entities == null || entities.Count() <= 0) return null;

      var list = new List<ChartDataDTO>();
      foreach (var item in entities)
      {
        var dto = new ChartDataDTO
        {
          Ticks = item.CreatedDateTime.Ticks,
          Value = item.UpperLeg
        };
        list.Add(dto);
      }
      return list.ToArray();
    }

    public ChartDataDTO[] GetWaistHistory(long userID)
    {
      var entities = circumRep.GetAll().Where(a => a.UserID == userID);
      if (entities == null || entities.Count() <= 0) return null;

      var list = new List<ChartDataDTO>();
      foreach (var item in entities)
      {
        var dto = new ChartDataDTO
        {
          Ticks = item.CreatedDateTime.Ticks,
          Value = item.Waist
        };
        list.Add(dto);
      }
      return list.ToArray();
    }

    public ChartDataDTO[] GetWeightHistory(long userID)
    {
      var entities = circumRep.GetAll().Where(a => a.UserID == userID);
      if (entities == null || entities.Count() <= 0) return null;

      var list = new List<ChartDataDTO>();
      foreach (var item in entities)
      {
        var dto = new ChartDataDTO
        {
          Ticks = item.CreatedDateTime.Ticks,
          Value = item.Weight
        };
        list.Add(dto);
      }
      return list.ToArray();
    }

    private bool IsToday(DateTime date)
    {
      var today = DateTimeHelper.GetToday();
      var tomorrow = today.AddDays(1);

      return (date >= today && date < tomorrow);
    }

    private BodyCircumferenceDTO ToDTO(BodyCircumferenceEntity entity)
    {
      if (entity == null)
      {
        throw new ArgumentException(ExceptionMsg.GetObjNullMsg("BodyCircumferenceEntity"));
      }

      var dto = new BodyCircumferenceDTO
      {
        ID = entity.ID,
        UserID = entity.UserID,
        UpperArm = entity.UpperArm,
        LowerArm = entity.LowerArm,
        Chest = entity.Chest,
        Waist = entity.Waist,
        Hip = entity.Hip,
        UpperLeg = entity.UpperLeg,
        LowerLeg = entity.LowerLeg,
        Weight = entity.Weight
      };

      return dto;
    }
  }
}
