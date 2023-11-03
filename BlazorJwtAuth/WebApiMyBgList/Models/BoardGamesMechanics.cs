﻿using System.ComponentModel.DataAnnotations;

namespace WebApiMyBgList.Models;

public class BoardGamesMechanics
{
    [Key] [Required] public int BoardGameId { get; set; }
    [Key] [Required] public int MechanicId { get; set; }
    [Required] public DateTime CreatedDate { get; set; }
    public BoardGame? BoardGame { get; set; }
    public Mechanic? Mechanic { get; set; }
}
