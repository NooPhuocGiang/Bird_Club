﻿namespace BirdClubAPI.Domain.DTOs.View.Acitivity
{
    public class AcitivityCreateViewModel
    {
        public string Name { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string ActivityType { get; set; } = null!;
        public int OwnerId { get; set; }
        public int Id { get; set; }
    }
}
