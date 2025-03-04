﻿#nullable enable
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApplication1.Model;
using WebApplication1.Utility;
using WebApplication1.Utility.PasswordHashers;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : DefaultController
	{
		public LoginController(ApplicationDbContext dbContext, ILogger<DefaultController> logger) : base(dbContext, logger: logger)
		{
			commandName = "Login";
		}

		private readonly IPasswordComparer _passwordComparer = new DefaultPasswordComparer();
		public IActionResult Index(string? login, string? password)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			string state;
			string? value = null;
			if (login == null || password == null)
			{
				state = ErrorResultsDescriptions.Failure;
				value = ErrorResultsDescriptions.InvalidCall;
				logger.LogError($"{commandName} - {state}: {value}");
			}
			else
			{
				if (dbContext.Users.Any(e => e.Login == login))
				{
					User user = dbContext.Users.First(e => e.Login == login);
					if (_passwordComparer.Compare(user.Password, password, new DefaultPasswordHasher()))
					{
						state = ErrorResultsDescriptions.Success;
						value = user.AuthKey;
						logger.LogInformation($"{commandName} - {state}: User: {user.Id}");
					}
					else
					{
						state = ErrorResultsDescriptions.Failure;
						value = ErrorResultsDescriptions.AuthenticationFailed;
						logger.LogError($"{commandName} - {state}: {value}");
					}

				}
				else
				{
					state = ErrorResultsDescriptions.Failure;
					value = ErrorResultsDescriptions.AuthenticationFailed;
					logger.LogError($"{commandName} - {state}: {value}");
				}
			}
			return Json(new { state, value });
		}
	}
}
