using BirdClubAPI.Domain.DTOs.Response.Activity;
using BirdClubAPI.Domain.DTOs.Response.Member;
using BirdClubAPI.Domain.Entities;

namespace BirdClubAPI.DataAccessLayer.Repositories.Activity
{
    public interface IActivityRepository
    {
        ActivityResponseModel? AttendanceActivity(Domain.Entities.Attendance requestModel);
        ActivityResponseModel? CreateActivity(Domain.Entities.Activity requestModel);
        bool DeleteAttendanceRequest(AttendanceRequest request);
        List<ActivityResponseModel> GetActivities(bool? isAll = false);
        Domain.Entities.Activity? GetActivities(int id);
        Domain.Entities.Activity? GetActivity(int activityId);
        List<MemberResponseModel> GetAttendance(int activityId);
        List<ActivityResponseModel> GetActivitiesByOwner(int ownerId);
        AttendanceRequest? GetAttendanceRequest(int memberId, int activityId);
        Attendance? PostAttendance(int memberId, int activityId);
        AttendanceRequest? PostAttendanceRequest(int memberId, int activityId);
        bool RemoveAttendanceRequest(AttendanceRequest request);
        Domain.Entities.Activity? GetActivitieWithAttendance(int id);
        bool UpdateActivity(Domain.Entities.Activity activity);
        List<AttendanceRequest> GetAttendanceRequests(int activityId);
    }
}
