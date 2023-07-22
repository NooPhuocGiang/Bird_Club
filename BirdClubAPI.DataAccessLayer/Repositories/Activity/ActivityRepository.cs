﻿using AutoMapper;
using BirdClubAPI.DataAccessLayer.Context;
using BirdClubAPI.Domain.DTOs.Response.Activity;
using BirdClubAPI.Domain.DTOs.Response.Member;
using BirdClubAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace BirdClubAPI.DataAccessLayer.Repositories.Activity
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly BirdClubContext _context;
        private readonly IMapper _mapper;

        public ActivityRepository(BirdClubContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ActivityResponseModel? AttendanceActivity(Domain.Entities.Attendance attendance)
        {
            try
            {
                var result = _context.Add(attendance);
                _context.SaveChanges();
                return _mapper.Map<ActivityResponseModel>(result.Entity);
            }
            catch
            {
                return null;
            }
        }

        public ActivityResponseModel? CreateActivity(Domain.Entities.Activity activity)
        {
            try
            {
                var result = _context.Add(activity);
                _context.SaveChanges();
                return _mapper.Map<ActivityResponseModel>(result.Entity);
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteAttendanceRequest(AttendanceRequest request)
        {
            try
            {
                _context.AttendanceRequests.Remove(request);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ActivityResponseModel> GetActivities(bool? isAll = false)
        {
            var activities = isAll == true ? 
                _context.Activities
                .Include(e => e.Owner)
                    .ThenInclude(e => e.User)
                .ToList() : 
                _context.Activities
                .Where(e => e.Status == true)
                .Include(e => e.Owner)
                    .ThenInclude(e => e.User)
                .ToList();
            if (activities.IsNullOrEmpty())
            {
                return new List<ActivityResponseModel>();
            }
            return _mapper.Map<List<ActivityResponseModel>>(activities);
        }

        public Domain.Entities.Activity? GetActivities(int id)
        {
            var activity = _context.Activities
                .Include(e => e.Owner)
                    .ThenInclude(e => e.User)
                .Include(e => e.Comments.Where(e => e.Type == "ACTIVITY"))
                .SingleOrDefault(e => e.Id == id);
            if (activity == null)
            {
                return null;
            }
            return activity;
        }

        public Domain.Entities.Activity? GetActivity(int activityId)
        {
            var activity = _context.Activities
               .Include(e => e.Owner)
                   .ThenInclude(e => e.User)
               .SingleOrDefault(e => e.Id == activityId);

            return activity;
        }

        public List<MemberResponseModel> GetAttendance(int activityId)
        {
            var members = _context.Attendances
                .Where(e => e.ActivityId == activityId)
                .Select(e => new MemberResponseModel
                {
                    MemberId = e.MemberId,
                    Avatar = e.Member.Avatar,
                    DisplayName = e.Member.User.DisplayName
                });
            return _mapper.Map<List<MemberResponseModel>>(members);
        }

        public List<ActivityResponseModel> GetActivitiesByOwner(int ownerId)
        {
            var activities = _context.Activities
                .Include(e => e.Owner)
                    .ThenInclude(e => e.User)
                .Where(e => e.OwnerId == ownerId)
                .ToList();
            if (activities.IsNullOrEmpty())
            {
                return new List<ActivityResponseModel>();
            }
            return _mapper.Map<List<ActivityResponseModel>>(activities);
        }

        public AttendanceRequest? GetAttendanceRequest(int memberId, int activityId)
        {
            var request = _context.AttendanceRequests.Find(memberId, activityId);
            return request;
        }

        public Attendance? PostAttendance(int memberId, int activityId)
        {
            try
            {
                var result = _context.Attendances.Add(new Attendance
                {
                    MemberId = memberId,
                    ActivityId = activityId,
                    AttendanceTime = DateTime.UtcNow.AddHours(7)
                });
                _context.SaveChanges();
                return result.Entity;
            }
            catch
            {
                return null;
            }
        }

        public AttendanceRequest? PostAttendanceRequest(int memberId, int activityId)
        {
            try
            {
                var result = _context.AttendanceRequests.Add(new AttendanceRequest
                {
                    MemberId = memberId,
                    ActivityId = activityId,
                    RequestTime = DateTime.UtcNow.AddHours(7)
                });
                _context.SaveChanges();
                return result.Entity;
            }
            catch
            {
                return null;
            }
        }

        public bool RemoveAttendanceRequest(AttendanceRequest request)
        {
            try
            {
                var result = _context.AttendanceRequests.Remove(request);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Domain.Entities.Activity? GetActivitieWithAttendance(int id)
        {
            var activity = _context.Activities
                .Include(e => e.Owner)
                .Include(e => e.Attendances)
                .Include(e => e.AttendanceRequests)
                .SingleOrDefault(e => e.Id == id);
            if (activity == null || activity.Status == false)
            {
                return null;
            }
            return activity;
        }

        public bool UpdateActivity(Domain.Entities.Activity activity)
        {
            try
            {
                _context.Activities.Attach(activity);
                _context.Entry(activity).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<AttendanceRequest> GetAttendanceRequests(int activityId)
        {
            var attendanceRequests = _context.AttendanceRequests.Where(e => e.ActivityId == activityId)
                .Include(e => e.Member)
                    .ThenInclude(e => e.User)
                .ToList();
            return attendanceRequests;
        }
    }
}
