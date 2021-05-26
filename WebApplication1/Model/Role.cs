﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Model
{
	public class Role
	{
		[Key]
		public int Id { get; set; }
		
		public string Name { get; set; }
		[Required]
		public int PermissionLevel { get; set; }
	}
}
