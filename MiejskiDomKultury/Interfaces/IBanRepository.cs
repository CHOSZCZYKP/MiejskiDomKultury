﻿using MiejskiDomKultury.Dto;
using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Interfaces
{
    public interface IBanRepository
    {
        IEnumerable<BanZUzytkownikiemDto> GetAllBansWithUsers();
        void AddNewBan(Ban ban);
    }
}
