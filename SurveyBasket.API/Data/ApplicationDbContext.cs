﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Resources;
using System.Reflection;

namespace SurveyBasket.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//أي كلاس يبانبلمنت الانرفيس (أ انتتي كونفجريشن)هيضيفه هنا بدل مكنت هضيف عشروميت كلاس لو عندي عشروميت تابل 
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}
