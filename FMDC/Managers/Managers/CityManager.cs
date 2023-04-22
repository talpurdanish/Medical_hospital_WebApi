using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FMDC.Managers.Managers
{
    public class CityManager : ICityManager
    {
        private readonly MedicalContext _context;
        public CityManager(MedicalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CityViewModel>> GetCities(int id = -1)
        {
            var query = from city in _context.Cities
                        join province in _context.Provinces on city.ProvinceId equals province.ProvinceId
                        select new CityViewModel()
                        {
                            id = city.CityId,
                            Name = city.Name,
                            ProvinceId = province.ProvinceId,
                            ProvinceName = province.Name

                        };

            var cities = await query.ToListAsync();

            if (id > 0)
            {
                cities = cities.Where(c => c.ProvinceId == id).ToList();
            }

            return cities;
        }
    }
}
