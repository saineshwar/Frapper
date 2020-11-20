using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Frapper.Entities.ProfileImage;

namespace Frapper.Repository.ProfileImage.Queries
{
    public class ProfileImageQueries : IProfileImageQueries
    {
        private readonly FrapperDbContext _frapperDbContext;
        public ProfileImageQueries(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public ProfileImageProperty GetProfileImageByProfileImageId(long userId)
        {
            var result = (from profileImage in _frapperDbContext.ProfileImagePropertys.AsNoTracking()
                          where profileImage.CreatedBy == userId
                          select profileImage).SingleOrDefault();

            return result;
        }


        public bool CheckProfileImageExists(long userId)
        {
            try
            {
                var result = (from profileImage in _frapperDbContext.ProfileImagePropertys.AsNoTracking()
                              where profileImage.CreatedBy == userId
                              select profileImage).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
