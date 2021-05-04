using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;
using Models.ViewModels.Views;
using Services.Interface;
using Utilities;

namespace Services
{
    public class DraftService : IDraftService
    {
        private readonly AppDbContext _dbContext;

        public DraftService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }



    }
}
