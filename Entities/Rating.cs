﻿namespace course_edu_api.Entities;

public class Rating
{
    public long Id { get; set; }
    public string Content { get; set; }
    public DateTime RateAt { get; set; } = new DateTime();
    public int Star  { get; set; }
}