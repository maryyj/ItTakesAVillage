﻿namespace ItTakesAVillage.Core.Models;

public class DinnerInvitation : BaseEvent 
{
    public string? Course { get; set; }  
    public string? Location { get; set; }
}
